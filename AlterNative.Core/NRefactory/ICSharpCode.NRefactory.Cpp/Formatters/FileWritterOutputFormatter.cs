//// Copyright (c) AlphaSierraPapa for the SharpDevelop Team
//// 
//// Permission is hereby granted, free of charge, to any person obtaining a copy of this
//// software and associated documentation files (the "Software"), to deal in the Software
//// without restriction, including without limitation the rights to use, copy, modify, merge,
//// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
//// to whom the Software is furnished to do so, subject to the following conditions:
//// 
//// The above copyright notice and this permission notice shall be included in all copies or
//// substantial portions of the Software.
//// 
//// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
//// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
//// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
//// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
//// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
//// DEALINGS IN THE SOFTWARE.

//using System;
//using System.IO;

//namespace ICSharpCode.NRefactory.Cpp
//{
//    /// <summary>
//    /// Writes Cpp code into a TextWriter.
//    /// </summary>
//    public class FileWriterOutputFormatter : IOutputFormatter
//    {
//        readonly StreamWriter fileWriter;
//        int indentation;
//        bool needsIndent = true;
//        bool isAtStartOfLine = true;

//        public int Indentation
//        {
//            get
//            {
//                return this.indentation;
//            }
//            set
//            {
//                this.indentation = value;
//            }
//        }

//        public string IndentationString { get; set; }

//        public FileWriterOutputFormatter(string file)
//        {
//            if (file == null)
//                throw new ArgumentNullException("file");
//            Formatters.FileWritterManager.getInstance().FileName = file;
//            this.fileWriter = Formatters.FileWritterManager.getInstance().streamWriter;
//            this.IndentationString = "\t";
//        }

//        public void WriteIdentifier(string ident)
//        {
//            WriteIndentation();
//            fileWriter.Write(ident);
//            isAtStartOfLine = false;
//        }

//        public void WriteKeyword(string keyword)
//        {
//            WriteIndentation();
//            fileWriter.Write(keyword);
//            isAtStartOfLine = false;
//        }

//        public void WriteToken(string token)
//        {
//            WriteIndentation();
//            fileWriter.Write(token);
//            isAtStartOfLine = false;
//        }

//        public void Space()
//        {
//            WriteIndentation();
//            fileWriter.Write(' ');
//        }

//        public void OpenBrace(BraceStyle style)
//        {
//            switch (style)
//            {
//                case BraceStyle.DoNotChange:
//                case BraceStyle.EndOfLine:
//                    WriteIndentation();
//                    if (!isAtStartOfLine)
//                        fileWriter.Write(' ');
//                    fileWriter.Write('{');
//                    break;
//                case BraceStyle.EndOfLineWithoutSpace:
//                    WriteIndentation();
//                    fileWriter.Write('{');
//                    break;
//                case BraceStyle.NextLine:
//                    if (!isAtStartOfLine)
//                        NewLine();
//                    WriteIndentation();
//                    fileWriter.Write('{');
//                    break;

//                case BraceStyle.NextLineShifted:
//                    NewLine();
//                    Indent();
//                    WriteIndentation();
//                    fileWriter.Write('{');
//                    NewLine();
//                    return;
//                case BraceStyle.NextLineShifted2:
//                    NewLine();
//                    Indent();
//                    WriteIndentation();
//                    fileWriter.Write('{');
//                    break;
//                default:
//                    throw new ArgumentOutOfRangeException();
//            }
//            Indent();
//            NewLine();
//        }

//        public void CloseBrace(BraceStyle style)
//        {
//            switch (style)
//            {
//                case BraceStyle.DoNotChange:
//                case BraceStyle.EndOfLine:
//                case BraceStyle.EndOfLineWithoutSpace:
//                case BraceStyle.NextLine:
//                    Unindent();
//                    WriteIndentation();
//                    fileWriter.Write('}');
//                    isAtStartOfLine = false;
//                    break;
//                case BraceStyle.NextLineShifted:
//                    WriteIndentation();
//                    fileWriter.Write('}');
//                    isAtStartOfLine = false;
//                    Unindent();
//                    break;
//                case BraceStyle.NextLineShifted2:
//                    Unindent();
//                    WriteIndentation();
//                    fileWriter.Write('}');
//                    isAtStartOfLine = false;
//                    Unindent();
//                    break;
//                default:
//                    throw new ArgumentOutOfRangeException();
//            }
//        }

//        protected void WriteIndentation()
//        {
//            if (needsIndent)
//            {
//                needsIndent = false;
//                for (int i = 0; i < indentation; i++)
//                {
//                    fileWriter.Write(this.IndentationString);
//                }
//            }
//        }

//        public void NewLine()
//        {
//            fileWriter.WriteLine();
//            needsIndent = true;
//            isAtStartOfLine = true;
//        }

//        public void Indent()
//        {
//            indentation++;
//        }

//        public void Unindent()
//        {
//            indentation--;
//        }

//        public void WriteComment(CommentType commentType, string content)
//        {
//            WriteIndentation();
//            switch (commentType)
//            {
//                case CommentType.SingleLine:
//                    fileWriter.Write("//");
//                    fileWriter.WriteLine(content);
//                    needsIndent = true;
//                    isAtStartOfLine = true;
//                    break;
//                case CommentType.MultiLine:
//                    fileWriter.Write("/*");
//                    fileWriter.Write(content);
//                    fileWriter.Write("*/");
//                    isAtStartOfLine = false;
//                    break;
//                case CommentType.Documentation:
//                    fileWriter.Write("///");
//                    fileWriter.WriteLine(content);
//                    needsIndent = true;
//                    isAtStartOfLine = true;
//                    break;
//                default:
//                    fileWriter.Write(content);
//                    break;
//            }
//        }

//        public void WritePreProcessorDirective(PreProcessorDirectiveType type, string argument)
//        {
//            // pre-processor directive must start on its own line
//            if (!isAtStartOfLine)
//                NewLine();
//            WriteIndentation();
//            fileWriter.Write('#');
//            fileWriter.Write(type.ToString().ToLowerInvariant());
//            if (!string.IsNullOrEmpty(argument))
//            {
//                fileWriter.Write(' ');
//                fileWriter.Write(argument);
//            }
//            NewLine();
//        }

//        public virtual void StartNode(AstNode node)
//        {
//            // Write out the indentation, so that overrides of this method
//            // can rely use the current output length to identify the position of the node
//            // in the output.
//            WriteIndentation();
//        }

//        public virtual void EndNode(AstNode node)
//        {
//        }


//        public void ChangeFile(string path)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
