// Copyright (c) 2011 AlphaSierraPapa for the SharpDevelop Team
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.ILAst;
using Mono.Cecil;
using ICSharpCode.NRefactory.CSharp;


namespace ICSharpCode.Decompiler.Ast
{
    /// <summary>
    /// Description of CppTextOutputFormatter.
    /// </summary>
    public class FileTextOutputFormatter : IOutputFormatter
    {
        int indentation;
        bool needsIndent = true;
        bool isAtStartOfLine = true;
        public string IndentationString { get; set; }

        readonly ITextOutput output;
        readonly Stack<AstNode> nodeStack = new Stack<AstNode>();

        public FileTextOutputFormatter(ITextOutput output)
        {
            if (output == null)
                throw new ArgumentNullException("output");
            this.output = output;
            this.IndentationString = "\t";
        }

        public void ChangeFile(string newFile)
        {
            if (!(output is FileTextOutput))
            {
                output.WriteLine();
                WriteComment(CommentType.SingleLine,"File: " + newFile);
                output.WriteLine();
            }

            if (output is FileTextOutput)
                (output as FileTextOutput).NewFile(newFile);
        }

        public void Close()
        {
            if (!(output is FileTextOutput))
            {
                output.WriteLine();
                WriteComment(CommentType.SingleLine, "End of file");
                output.WriteLine();
            }

            if (output is FileTextOutput)
                (output as FileTextOutput).Close();
        }

        public void StartNode(AstNode node)
        {
            //			var ranges = node.Annotation<List<ILRange>>();
            //			if (ranges != null && ranges.Count > 0)
            //			{
            //				// find the ancestor that has method mapping as annotation
            //				if (node.Ancestors != null && node.Ancestors.Count() > 0)
            //				{
            //					var n = node.Ancestors.FirstOrDefault(a => a.Annotation<MemberMapping>() != null);
            //					if (n != null) {
            //						MemberMapping mapping = n.Annotation<MemberMapping>();
            //
            //						// add all ranges
            //						foreach (var range in ranges) {
            //							mapping.MemberCodeMappings.Add(new SourceCodeMapping {
            //							                               	ILInstructionOffset = range,
            //							                               	SourceCodeLine = output.CurrentLine,
            //							                               	MemberMapping = mapping
            //							                               });
            //						}
            //					}
            //				}
            //			}

            nodeStack.Push(node);
        }

        public void EndNode(AstNode node)
        {
            if (nodeStack.Pop() != node)
                throw new InvalidOperationException();
        }

        public void WriteIdentifier(string identifier)
        {
            var definition = GetCurrentDefinition();
            if (definition != null)
            {
                output.WriteDefinition(identifier, definition);
                return;
            }

            object memberRef = GetCurrentMemberReference();
            if (memberRef != null)
            {
                output.WriteReference(identifier, memberRef);
                return;
            }

            definition = GetCurrentLocalDefinition();
            if (definition != null)
            {
                output.WriteDefinition(identifier, definition);
                return;
            }

            memberRef = GetCurrentLocalReference();
            if (memberRef != null)
            {
                output.WriteReference(identifier, memberRef, true);
                return;
            }

            output.Write(identifier);
        }

        MemberReference GetCurrentMemberReference()
        {
            AstNode node = nodeStack.Peek();
            MemberReference memberRef = node.Annotation<MemberReference>();
            if (memberRef == null && node.Role == Roles.TargetExpression && (node.Parent is InvocationExpression || node.Parent is ObjectCreateExpression))
            {
                memberRef = node.Parent.Annotation<MemberReference>();
            }
            return memberRef;
        }

        object GetCurrentLocalReference()
        {
            AstNode node = nodeStack.Peek();
            ILVariable variable = node.Annotation<ILVariable>();
            if (variable != null)
            {
                if (variable.OriginalParameter != null)
                    return variable.OriginalParameter;
                //if (variable.OriginalVariable != null)
                //    return variable.OriginalVariable;
                return variable;
            }
            return null;
        }

        object GetCurrentLocalDefinition()
        {
            AstNode node = nodeStack.Peek();
            var parameterDef = node.Annotation<ParameterDefinition>();
            if (parameterDef != null)
                return parameterDef;

            if (node is VariableInitializer || node is CatchClause)// || node is ForEachStatement)
            {
                var variable = node.Annotation<ILVariable>();
                if (variable != null)
                {
                    if (variable.OriginalParameter != null)
                        return variable.OriginalParameter;
                    //if (variable.OriginalVariable != null)
                    //    return variable.OriginalVariable;
                    return variable;
                }
                else
                {

                }
            }

            return null;
        }

        object GetCurrentDefinition()
        {
            if (nodeStack == null || nodeStack.Count == 0)
                return null;

            var node = nodeStack.Peek();
            if (IsDefinition(node))
                return node.Annotation<MemberReference>();

            node = node.Parent;
            if (IsDefinition(node))
                return node.Annotation<MemberReference>();

            return null;
        }

        public void WriteKeyword(string keyword)
        {
            output.Write(keyword);
        }

        public void WriteToken(string token)
        {
            // Attach member reference to token only if there's no identifier in the current node.
            MemberReference memberRef = GetCurrentMemberReference();
            if (memberRef != null && nodeStack.Peek().GetChildByRole(Roles.Identifier).IsNull)
                output.WriteReference(token, memberRef);
            else
                output.Write(token);
        }

        public void Space()
        {
            output.Write(' ');
        }

        public void Indent()
        {
            output.Indent();
        }

        public void Unindent()
        {
            output.Unindent();
        }

        public void NewLine()
        {
            output.WriteLine();
        }

        public void WriteComment(bool isDocumentation, string content)
        {
            if (isDocumentation)
                output.Write("'''");
            else
                output.Write("'");
            output.WriteLine(content);
        }

        public void MarkFoldStart()
        {
            output.MarkFoldStart();
        }

        public void MarkFoldEnd()
        {
            output.MarkFoldEnd();
        }

        private static bool IsDefinition(AstNode node)
        {
            return
                node is FieldDeclaration ||
                node is ConstructorDeclaration ||
                node is EventDeclaration ||
                node is DelegateDeclaration ||
                node is OperatorDeclaration ||
                node is TypeDeclaration;
        }


        public void OpenBrace(BraceStyle style)
        {
            switch (style)
            {
                case BraceStyle.DoNotChange:
                case BraceStyle.EndOfLine:
                    WriteIndentation();
                    if (!isAtStartOfLine)
                        output.Write(' ');
                    output.Write('{');
                    break;
                case BraceStyle.EndOfLineWithoutSpace:
                    WriteIndentation();
                    output.Write('{');
                    break;
                case BraceStyle.NextLine:
                    if (!isAtStartOfLine)
                        NewLine();
                    WriteIndentation();
                    output.Write('{');
                    break;

                case BraceStyle.NextLineShifted:
                    NewLine();
                    Indent();
                    WriteIndentation();
                    output.Write('{');
                    NewLine();
                    return;
                case BraceStyle.NextLineShifted2:
                    NewLine();
                    Indent();
                    WriteIndentation();
                    output.Write('{');
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            Indent();
            NewLine();
        }

        public void CloseBrace(BraceStyle style)
        {
            switch (style)
            {
                case BraceStyle.DoNotChange:
                case BraceStyle.EndOfLine:
                case BraceStyle.EndOfLineWithoutSpace:
                case BraceStyle.NextLine:
                    Unindent();
                    WriteIndentation();
                    output.Write('}');
                    isAtStartOfLine = false;
                    break;
                case BraceStyle.NextLineShifted:
                    WriteIndentation();
                    output.Write('}');
                    isAtStartOfLine = false;
                    Unindent();
                    break;
                case BraceStyle.NextLineShifted2:
                    Unindent();
                    WriteIndentation();
                    output.Write('}');
                    isAtStartOfLine = false;
                    Unindent();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void WriteComment(CommentType commentType, string content)
        {
            WriteIndentation();
            switch (commentType)
            {
                case CommentType.SingleLine:
                    output.Write("//");
                    output.WriteLine(content);
                    needsIndent = true;
                    isAtStartOfLine = true;
                    break;
                case CommentType.MultiLine:
                    output.Write("/*");
                    output.Write(content);
                    output.Write("*/");
                    isAtStartOfLine = false;
                    break;
                case CommentType.Documentation:
                    output.Write("///");
                    output.WriteLine(content);
                    needsIndent = true;
                    isAtStartOfLine = true;
                    break;
                default:
                    output.Write(content);
                    break;
            }
        }

        public void WritePreProcessorDirective(PreProcessorDirectiveType type, string argument)
        {
            // pre-processor directive must start on its own line
            if (!isAtStartOfLine)
                NewLine();
            WriteIndentation();
            output.Write('#');
            output.Write(type.ToString().ToLowerInvariant());
            if (!string.IsNullOrEmpty(argument))
            {
                output.Write(' ');
                output.Write(argument);
            }
            NewLine();
        }

        protected void WriteIndentation()
        {
            if (needsIndent)
            {
                needsIndent = false;
                for (int i = 0; i < indentation; i++)
                {
                    output.Write(this.IndentationString);
                }
            }
        }       
    }
}
