﻿// 
// CSharpCompletionEngineBase.cs
//  
// Author:
//       Mike Krüger <mkrueger@xamarin.com>
// 
// Copyright (c) 2011 Xamarin Inc. (http://xamarin.com)
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ICSharpCode.NRefactory.CSharp.Resolver;
using ICSharpCode.NRefactory.Editor;
using ICSharpCode.NRefactory.TypeSystem;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem.Implementation;
using ICSharpCode.NRefactory.CSharp.TypeSystem;

namespace ICSharpCode.NRefactory.CSharp.Completion
{
	/// <summary>
	/// Acts as a common base between code completion and parameter completion.
	/// </summary>
	public class CSharpCompletionEngineBase
	{
		protected IDocument document;
		protected int offset;
		protected TextLocation location;
		protected IUnresolvedTypeDefinition currentType;
		protected IUnresolvedMember currentMember;
		
		#region Input properties
		public CSharpTypeResolveContext ctx { get; private set; }

		public CompilationUnit Unit { get; private set; }

		public CSharpParsedFile CSharpParsedFile { get; private set; }

		public IProjectContent ProjectContent { get; private set; }
		
		ICompilation compilation;

		protected ICompilation Compilation {
			get {
				if (compilation == null)
					compilation = ProjectContent.Resolve (ctx).Compilation;
				return compilation;
			}
		}
		#endregion
		
		protected CSharpCompletionEngineBase (IProjectContent content, CSharpTypeResolveContext ctx, CompilationUnit unit, CSharpParsedFile parsedFile)
		{
			if (content == null)
				throw new ArgumentNullException ("content");
			if (ctx == null)
				throw new ArgumentNullException ("ctx");
			if (unit == null)
				throw new ArgumentNullException ("unit");
			if (parsedFile == null)
				throw new ArgumentNullException ("parsedFile");
			
			this.ProjectContent = content;
			this.ctx = ctx;
			this.Unit = unit;
			this.CSharpParsedFile = parsedFile;
		}
		
		
		public IMemberProvider MemberProvider {
			get;
			set;
		}
		
		protected void SetOffset (int offset)
		{
			Reset ();
			
			this.offset = offset;
			this.location = document.GetLocation (offset);
			var provider = MemberProvider ?? new DefaultMemberProvider (this);
			provider.GetCurrentMembers (offset, out currentType, out currentMember);
		}

		protected bool GetParameterCompletionCommandOffset(out int cpos)
		{
			// Start calculating the parameter offset from the beginning of the
			// current member, instead of the beginning of the file. 
			cpos = offset - 1;
			var mem = currentMember;
			if (mem == null || (mem is IType)) {
				return false;
			}
			int startPos = document.GetOffset(mem.Region.BeginLine, mem.Region.BeginColumn);
			int parenDepth = 0;
			int chevronDepth = 0;
			Stack<int> indexStack = new Stack<int>();
			while (cpos > startPos) {
				char c = document.GetCharAt(cpos);
				if (c == ')') {
					parenDepth++;
				}
				if (c == '>') {
					chevronDepth++;
				}
				if (c == '}') {
					if (indexStack.Count > 0) {
						parenDepth = indexStack.Pop();
					} else {
						parenDepth = 0;
					}
					chevronDepth = 0;
				}
				if (indexStack.Count == 0 && (parenDepth == 0 && c == '(' || chevronDepth == 0 && c == '<')) {
					int p = GetCurrentParameterIndex (cpos + 1, startPos);
					if (p != -1) {
						cpos++;
						return true;
					} else {
						return false;
					}
				}
				if (c == '(') {
					parenDepth--;
				}
				if (c == '<') {
					chevronDepth--;
				}
				if (c == '{') {
					indexStack.Push (parenDepth);
					chevronDepth = 0;
				}
				cpos--;
			}
			return false;
		}
		
		protected int GetCurrentParameterIndex (int offset, int memberStart)
		{
			int cursor = this.offset;
			int i = offset;
			
			if (i > cursor) {
				return -1;
			}
			if (i == cursor) { 
				return 1;
			}
			// parameters are 1 based
			int index = memberStart + 1;
			int parentheses = 0;
			int bracket = 0;
			bool insideQuote = false, insideString = false, insideSingleLineComment = false, insideMultiLineComment = false;
			Stack<int> indexStack = new Stack<int> ();
			do {
				char c = document.GetCharAt (i - 1);
				switch (c) {
				case '\\':
					if (insideString || insideQuote) {
						i++;
					}
					break;
				case '\'':
					if (!insideString && !insideSingleLineComment && !insideMultiLineComment) {
						insideQuote = !insideQuote;
					}
					break;
				case '"':
					if (!insideQuote && !insideSingleLineComment && !insideMultiLineComment) {
						insideString = !insideString;
					}
					break;
				case '/':
					if (!insideQuote && !insideString && !insideMultiLineComment) {
						if (document.GetCharAt (i) == '/') {
							insideSingleLineComment = true;
						}
						if (document.GetCharAt (i) == '*') {
							insideMultiLineComment = true;
						}
					}
					break;
				case '*':
					if (insideMultiLineComment && document.GetCharAt (i) == '/') {
						insideMultiLineComment = false;
					}
					break;
				case '\n':
				case '\r':
					insideSingleLineComment = false;
					break;
				case '{':
					if (!insideQuote && !insideString && !insideSingleLineComment && !insideMultiLineComment) {
						bracket++;
						indexStack.Push (index);
					}
					break;
				case '}':
					if (!insideQuote && !insideString && !insideSingleLineComment && !insideMultiLineComment) {
						bracket--;
						if (indexStack.Count > 0)
							index = indexStack.Pop ();
					}
					break;
				case '(':
					if (!insideQuote && !insideString && !insideSingleLineComment && !insideMultiLineComment) {
						parentheses++;
					}
					break;
				case ')':
					if (!insideQuote && !insideString && !insideSingleLineComment && !insideMultiLineComment) {
						parentheses--;
					}
					break;
				case ',':
					if (!insideQuote && !insideString && !insideSingleLineComment && !insideMultiLineComment && parentheses == 1 && bracket == 0) {
						index++;
					}
					break;

				}
				i++;
			} while (i <= cursor && parentheses >= 0);
			Console.WriteLine (indexStack.Count >= 0 || parentheses != 1 || bracket > 0 ? -1 : index);
			return indexStack.Count >= 0 || parentheses != 1 || bracket > 0 ? -1 : index;
		}

		#region Context helper methods
		public class MiniLexer
		{
			readonly string text;

			public bool IsFistNonWs = true;
			public bool IsInSingleComment = false;
			public bool IsInString = false;
			public bool IsInVerbatimString = false;
			public bool IsInChar = false;
			public bool IsInMultiLineComment = false;
			public bool IsInPreprocessorDirective = false;

			public MiniLexer(string text)
			{
				this.text = text;
			}

			public void Parse(Action<char> act = null)
			{
				Parse(0, text.Length, act);
			}

			public void Parse(int start, int length, Action<char> act = null)
			{
				for (int i = start; i < length; i++) {
					char ch = text [i];
					char nextCh = i + 1 < text.Length ? text [i + 1] : '\0';
					switch (ch) {
						case '#':
							if (IsFistNonWs)
								IsInPreprocessorDirective = true;
							break;
						case '/':
							if (IsInString || IsInChar || IsInVerbatimString)
								break;
							if (nextCh == '/') {
								i++;
								IsInSingleComment = true;
							}
							if (nextCh == '*')
								IsInMultiLineComment = true;
							break;
						case '*':
							if (IsInString || IsInChar || IsInVerbatimString || IsInSingleComment)
								break;
							if (nextCh == '/') {
								i++;
								IsInMultiLineComment = false;
							}
							break;
						case '@':
							if (IsInString || IsInChar || IsInVerbatimString || IsInSingleComment || IsInMultiLineComment)
								break;
							if (nextCh == '"') {
								i++;
								IsInVerbatimString = true;
							}
							break;
						case '\n':
						case '\r':
							IsInSingleComment = false;
							IsInString = false;
							IsInChar = false;
							IsFistNonWs = true;
							break;
						case '\\':
							if (IsInString || IsInChar)
								i++;
							break;
						case '"':
							if (IsInSingleComment || IsInMultiLineComment || IsInChar)
								break;
							if (IsInVerbatimString) {
								if (nextCh == '"') {
									i++;
									break;
								}
								IsInVerbatimString = false;
								break;
							}
							IsInString = !IsInString;
							break;
						case '\'':
							if (IsInSingleComment || IsInMultiLineComment || IsInString || IsInVerbatimString)
								break;
							IsInChar = !IsInChar;
							break;
					}
					if (act != null)
						act(ch);
					IsFistNonWs &= ch == ' ' || ch == '\t' || ch == '\n' || ch == '\r';
				}
			}
		}

		protected bool IsInsideCommentStringOrDirective()
		{
			var text = GetMemberTextToCaret();
			var lexer = new MiniLexer(text.Item1);
			lexer.Parse();
			return
				lexer.IsInSingleComment || 
				lexer.IsInString ||
				lexer.IsInVerbatimString ||
				lexer.IsInChar ||
				lexer.IsInMultiLineComment || 
				lexer.IsInPreprocessorDirective;
		}

		protected bool IsInsideDocComment ()
		{
			var text = GetMemberTextToCaret ();
			bool inSingleComment = false, inString = false, inVerbatimString = false, inChar = false, inMultiLineComment = false;
			bool singleLineIsDoc = false;
			
			for (int i = 0; i < text.Item1.Length - 1; i++) {
				char ch = text.Item1 [i];
				char nextCh = text.Item1 [i + 1];
				
				switch (ch) {
				case '/':
					if (inString || inChar || inVerbatimString)
						break;
					if (nextCh == '/') {
						i++;
						inSingleComment = true;
						singleLineIsDoc = i + 1 < text.Item1.Length && text.Item1 [i + 1] == '/';
						if (singleLineIsDoc) {
							i++;
						}
					}
					if (nextCh == '*')
						inMultiLineComment = true;
					break;
				case '*':
					if (inString || inChar || inVerbatimString || inSingleComment)
						break;
					if (nextCh == '/') {
						i++;
						inMultiLineComment = false;
					}
					break;
				case '@':
					if (inString || inChar || inVerbatimString || inSingleComment || inMultiLineComment)
						break;
					if (nextCh == '"') {
						i++;
						inVerbatimString = true;
					}
					break;
				case '\n':
				case '\r':
					inSingleComment = false;
					inString = false;
					inChar = false;
					break;
				case '\\':
					if (inString || inChar)
						i++;
					break;
				case '"':
					if (inSingleComment || inMultiLineComment || inChar)
						break;
					if (inVerbatimString) {
						if (nextCh == '"') {
							i++;
							break;
						}
						inVerbatimString = false;
						break;
					}
					inString = !inString;
					break;
				case '\'':
					if (inSingleComment || inMultiLineComment || inString || inVerbatimString)
						break;
					inChar = !inChar;
					break;
				}
			}
			
			return inSingleComment && singleLineIsDoc;
		}

		protected CSharpResolver GetState ()
		{
			return new CSharpResolver (ctx);
			/*var state = new CSharpResolver (ctx);
			
			state.CurrentMember = currentMember;
			state.CurrentTypeDefinition = currentType;
			state.CurrentUsingScope = CSharpParsedFile.GetUsingScope (location);
			if (state.CurrentMember != null) {
				var node = Unit.GetNodeAt (location);
				if (node == null)
					return state;
				var navigator = new NodeListResolveVisitorNavigator (new[] { node });
				var visitor = new ResolveVisitor (state, CSharpParsedFile, navigator);
				Unit.AcceptVisitor (visitor, null);
				try {
					var newState = visitor.GetResolverStateBefore (node);
					if (newState != null)
						state = newState;
				} catch (Exception) {
				}
			}
			
			return state;*/
		}
				#endregion
		
		#region Basic parsing/resolving functions
		static Stack<Tuple<char, int>> GetBracketStack (string memberText)
		{
			var bracketStack = new Stack<Tuple<char, int>> ();
			
			bool inSingleComment = false, inString = false, inVerbatimString = false, inChar = false, inMultiLineComment = false;
			
			for (int i = 0; i < memberText.Length; i++) {
				char ch = memberText [i];
				char nextCh = i + 1 < memberText.Length ? memberText [i + 1] : '\0';
				switch (ch) {
				case '(':
				case '[':
				case '{':
					if (inString || inChar || inVerbatimString || inSingleComment || inMultiLineComment)
						break;
					bracketStack.Push (Tuple.Create (ch, i));
					break;
				case ')':
				case ']':
				case '}':
					if (inString || inChar || inVerbatimString || inSingleComment || inMultiLineComment)
						break;
					if (bracketStack.Count > 0)
						bracketStack.Pop ();
					break;
				case '/':
					if (inString || inChar || inVerbatimString)
						break;
					if (nextCh == '/') {
						i++;
						inSingleComment = true;
					}
					if (nextCh == '*')
						inMultiLineComment = true;
					break;
				case '*':
					if (inString || inChar || inVerbatimString || inSingleComment)
						break;
					if (nextCh == '/') {
						i++;
						inMultiLineComment = false;
					}
					break;
				case '@':
					if (inString || inChar || inVerbatimString || inSingleComment || inMultiLineComment)
						break;
					if (nextCh == '"') {
						i++;
						inVerbatimString = true;
					}
					break;
				case '\n':
				case '\r':
					inSingleComment = false;
					inString = false;
					inChar = false;
					break;
				case '\\':
					if (inString || inChar)
						i++;
					break;
				case '"':
					if (inSingleComment || inMultiLineComment || inChar)
						break;
					if (inVerbatimString) {
						if (nextCh == '"') {
							i++;
							break;
						}
						inVerbatimString = false;
						break;
					}
					inString = !inString;
					break;
				case '\'':
					if (inSingleComment || inMultiLineComment || inString || inVerbatimString)
						break;
					inChar = !inChar;
					break;
				default :
					break;
				}
			}
			return bracketStack;
		}
		
		public static void AppendMissingClosingBrackets (StringBuilder wrapper, string memberText, bool appendSemicolon)
		{
			var bracketStack = GetBracketStack (memberText);
			bool didAppendSemicolon = !appendSemicolon;
			//char lastBracket = '\0';
			while (bracketStack.Count > 0) {
				var t = bracketStack.Pop ();
				switch (t.Item1) {
				case '(':
					wrapper.Append (')');
					if (appendSemicolon)
						didAppendSemicolon = false;
					//lastBracket = ')';
					break;
				case '[':
					wrapper.Append (']');
					if (appendSemicolon)
						didAppendSemicolon = false;
					//lastBracket = ']';
					break;
				case '<':
					wrapper.Append ('>');
					if (appendSemicolon)
						didAppendSemicolon = false;
					//lastBracket = '>';
					break;
				case '{':
					int o = t.Item2 - 1;
					if (!didAppendSemicolon) {
						didAppendSemicolon = true;
						wrapper.Append (';');
					}
						
					bool didAppendCatch = false;
					while (o >= "try".Length) {
						char ch = memberText [o];
						if (!char.IsWhiteSpace (ch)) {
							if (ch == 'y' && memberText [o - 1] == 'r' && memberText [o - 2] == 't') {
								wrapper.Append ("} catch {}");
								didAppendCatch = true;
							}
							break;
						}
						o--;
					}
					if (!didAppendCatch)
						wrapper.Append ('}');
					break;
				}
			}
			if (!didAppendSemicolon)
				wrapper.Append (';');
		}

		protected CompilationUnit ParseStub(string continuation, bool appendSemicolon = true, string afterContinuation = null)
		{
			var mt = GetMemberTextToCaret();
			if (mt == null) {
				return null;
			}

			string memberText = mt.Item1;
			var memberLocation = mt.Item2;
			int closingBrackets = 1;
			int generatedLines = 0;
			var wrapper = new StringBuilder();
			bool wrapInClass = memberLocation != new TextLocation(1, 1);
			if (wrapInClass) {
				wrapper.Append("class Stub {");
				wrapper.AppendLine();
				closingBrackets++;
				generatedLines++;
			}
			wrapper.Append(memberText);
			wrapper.Append(continuation);
			AppendMissingClosingBrackets(wrapper, memberText, appendSemicolon);
			wrapper.Append(afterContinuation);
			if (closingBrackets > 0) { 
				wrapper.Append(new string('}', closingBrackets));
			}
			using (var stream = new System.IO.StringReader (wrapper.ToString ())) {
				try {
					var parser = new CSharpParser ();
					var result = parser.Parse(stream, "stub.cs", memberLocation.Line - 1 - generatedLines);
					return result;
				} catch (Exception) {
					Console.WriteLine("------");
					Console.WriteLine(wrapper);
					throw;
				}
			}
		}
		
		string cachedText = null;
		
		protected virtual void Reset ()
		{
			cachedText = null;
		}
		
		protected Tuple<string, TextLocation> GetMemberTextToCaret()
		{
			int startOffset;
			if (currentMember != null && currentType != null && currentType.Kind != TypeKind.Enum) {
				startOffset = document.GetOffset(currentMember.Region.Begin);
			} else if (currentType != null) {
				startOffset = document.GetOffset(currentType.Region.Begin);
			} else {
				startOffset = 0;
			}
			while (startOffset > 0) {
				char ch = document.GetCharAt(startOffset - 1);
				if (ch != ' ' && ch != '\t') {
					break;
				}
				--startOffset;
			}
			if (cachedText == null)
				cachedText = document.GetText (startOffset, offset - startOffset);
			
			return Tuple.Create (cachedText, document.GetLocation (startOffset));
		}
		
		protected ExpressionResult GetInvocationBeforeCursor(bool afterBracket)
		{
			CompilationUnit baseUnit;
			baseUnit = ParseStub("a", false);
			
			var section = baseUnit.GetNodeAt<AttributeSection>(location.Line, location.Column - 2);
			var attr = section != null ? section.Attributes.LastOrDefault() : null;
			if (attr != null) {
				return new ExpressionResult((AstNode)attr, baseUnit);
			}

			//var memberLocation = currentMember != null ? currentMember.Region.Begin : currentType.Region.Begin;
			var mref = baseUnit.GetNodeAt(location.Line, location.Column - 1, n => n is InvocationExpression || n is ObjectCreateExpression); 
			AstNode expr = null;
			if (mref is InvocationExpression) {
				expr = ((InvocationExpression)mref).Target;
			} else if (mref is ObjectCreateExpression) {
				expr = mref;
			} else {
				baseUnit = ParseStub(")};", false);
				mref = baseUnit.GetNodeAt(location.Line, location.Column - 1, n => n is InvocationExpression || n is ObjectCreateExpression); 
				if (mref is InvocationExpression) {
					expr = ((InvocationExpression)mref).Target;
				} else if (mref is ObjectCreateExpression) {
					expr = mref;
				}
			}

			if (expr == null) {
				// work around for missing ';' bug in mcs:
				baseUnit = ParseStub("a", true);
			
				section = baseUnit.GetNodeAt<AttributeSection>(location.Line, location.Column - 2);
				attr = section != null ? section.Attributes.LastOrDefault() : null;
				if (attr != null) {
					return new ExpressionResult((AstNode)attr, baseUnit);
				}
	
				//var memberLocation = currentMember != null ? currentMember.Region.Begin : currentType.Region.Begin;
				mref = baseUnit.GetNodeAt(location.Line, location.Column - 1, n => n is InvocationExpression || n is ObjectCreateExpression); 
				expr = null;
				if (mref is InvocationExpression) {
					expr = ((InvocationExpression)mref).Target;
				} else if (mref is ObjectCreateExpression) {
					expr = mref;
				}
			}

			if (expr == null) {
				return null;
			}
			return new ExpressionResult ((AstNode)expr, baseUnit);
		}
		
		public class ExpressionResult
		{
			public AstNode Node { get; private set; }
			public CompilationUnit Unit  { get; private set; }
			
			
			public ExpressionResult (AstNode item2, CompilationUnit item3)
			{
				this.Node = item2;
				this.Unit = item3;
			}
			
			public override string ToString ()
			{
				return string.Format ("[ExpressionResult: Node={0}, Unit={1}]", Node, Unit);
			}
		}
		
		protected Tuple<ResolveResult, CSharpResolver> ResolveExpression (ExpressionResult tuple)
		{
			return ResolveExpression (tuple.Node, tuple.Unit);
		}

		protected Tuple<ResolveResult, CSharpResolver> ResolveExpression(AstNode expr, CompilationUnit unit)
		{
			if (expr == null) {
				return null;
			}
			AstNode resolveNode;
			if (expr is Expression || expr is AstType) {
				resolveNode = expr;
			} else if (expr is VariableDeclarationStatement) {
				resolveNode = ((VariableDeclarationStatement)expr).Type;
			} else {
				resolveNode = expr;
			}
			try {
				var ctx = CSharpParsedFile.GetResolver(Compilation, location);
				var root = expr.AncestorsAndSelf.FirstOrDefault(n => n is EntityDeclaration || n is CompilationUnit);
				if (root == null) {
					return null;
				}
				var csResolver = new CSharpAstResolver (ctx, root, CSharpParsedFile);
				var result = csResolver.Resolve(resolveNode);
				var state = csResolver.GetResolverStateBefore(resolveNode);
				return Tuple.Create(result, state);
			} catch (Exception e) {
				Console.WriteLine(e);
				return null;
			}
		}
		
		#endregion
		
		class DefaultMemberProvider : IMemberProvider
	{
		CSharpCompletionEngineBase engine;
		
		
		public DefaultMemberProvider (CSharpCompletionEngineBase engine)
		{
			this.engine = engine;
		}
		
		public void GetCurrentMembers (int offset, out IUnresolvedTypeDefinition currentType, out IUnresolvedMember currentMember)
		{
			//var document = engine.document;
			var location = engine.location;
			
			currentType = null;
			
			foreach (var type in engine.CSharpParsedFile.TopLevelTypeDefinitions) {
				if (type.Region.Begin < location)
					currentType = type;
			}
			currentType = FindInnerType (currentType, location);
			
			// location is beyond last reported end region, now we need to check, if the end region changed
			if (currentType != null && currentType.Region.End < location) {
				if (!IsInsideType (currentType, location))
					currentType = null;
			}
			currentMember = null;
			if (currentType != null) {
				foreach (var member in currentType.Members) {
					if (member.Region.Begin < location && (currentMember == null || currentMember.Region.Begin < member.Region.Begin))
						currentMember = member;
				}
			}
			
			// location is beyond last reported end region, now we need to check, if the end region changed
			// NOTE: Enums are a special case, there the "last" field needs to be treated as current member
			if (currentMember != null && currentMember.Region.End < location && currentType.Kind != TypeKind.Enum) {
				if (!IsInsideType (currentMember, location))
					currentMember = null;
			}
			var stack = GetBracketStack (engine.GetMemberTextToCaret ().Item1);
			if (stack.Count == 0)
				currentMember = null;
		}

		IUnresolvedTypeDefinition FindInnerType (IUnresolvedTypeDefinition parent, TextLocation location)
		{
			if (parent == null)
				return null;
			var currentType = parent;
			foreach (var type in parent.NestedTypes) {
				if (type.Region.Begin < location  && location < type.Region.End)
					currentType = FindInnerType (type, location);
			}
			
			return currentType;
		}
		
		bool IsInsideType (IUnresolvedEntity currentType, TextLocation location)
		{
			var document = engine.document;
			
			int startOffset = document.GetOffset (currentType.Region.Begin);
			int endOffset = document.GetOffset (location);
			//bool foundEndBracket = false;
		
			var bracketStack = new Stack<char> ();
		
			bool isInString = false, isInChar = false;
			bool isInLineComment = false, isInBlockComment = false;
			
			for (int i = startOffset; i < endOffset; i++) {
				char ch = document.GetCharAt (i);
				switch (ch) {
					case '(':
					case '[':
					case '{':
						if (!isInString && !isInChar && !isInLineComment && !isInBlockComment)
							bracketStack.Push (ch);
						break;
					case ')':
					case ']':
					case '}':
						if (!isInString && !isInChar && !isInLineComment && !isInBlockComment)
						if (bracketStack.Count > 0)
							bracketStack.Pop ();
						break;
					case '\r':
					case '\n':
						isInLineComment = false;
						break;
					case '/':
						if (isInBlockComment) {
							if (i > 0 && document.GetCharAt (i - 1) == '*') 
								isInBlockComment = false;
						} else if (!isInString && !isInChar && i + 1 < document.TextLength) {
							char nextChar = document.GetCharAt (i + 1);
							if (nextChar == '/')
								isInLineComment = true;
							if (!isInLineComment && nextChar == '*')
								isInBlockComment = true;
						}
						break;
					case '"':
						if (!(isInChar || isInLineComment || isInBlockComment)) 
							isInString = !isInString;
						break;
					case '\'':
						if (!(isInString || isInLineComment || isInBlockComment)) 
							isInChar = !isInChar;
						break;
					default :
						break;
					}
				}
			return bracketStack.Any (t => t == '{');
		}		
	}

	
	}
}