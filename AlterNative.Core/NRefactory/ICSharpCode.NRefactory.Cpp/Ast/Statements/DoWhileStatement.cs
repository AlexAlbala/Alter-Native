// 
// DoWhileStatement.cs
//  
// Author:
//       Mike Krüger <mkrueger@novell.com>
// 
// Copyright (c) 2011 Novell, Inc (http://www.novell.com)
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
// THE SOFTWARE.using System;

using ICSharpCode.NRefactory.Cpp.Ast;
namespace ICSharpCode.NRefactory.Cpp
{
	/// <summary>
	/// "do EmbeddedStatement while(Condition);"
	/// </summary>
	public class DoWhileStatement : Statement
	{
		public static readonly Role<CppTokenNode> DoKeywordRole = new Role<CppTokenNode>("DoKeyword", CppTokenNode.Null);
		public static readonly Role<CppTokenNode> WhileKeywordRole = new Role<CppTokenNode>("WhileKeyword", CppTokenNode.Null);
		
		public CppTokenNode DoToken {
			get { return GetChildByRole (DoKeywordRole); }
		}
		
		public Statement EmbeddedStatement {
			get { return GetChildByRole (Roles.EmbeddedStatement); }
			set { SetChildByRole (Roles.EmbeddedStatement, value); }
		}
		
		public CppTokenNode WhileToken {
			get { return GetChildByRole (WhileKeywordRole); }
		}
		
		public CppTokenNode LParToken {
			get { return GetChildByRole (Roles.LPar); }
		}
		
		public Expression Condition {
			get { return GetChildByRole (Roles.Condition); }
			set { SetChildByRole (Roles.Condition, value); }
		}
		
		public CppTokenNode RParToken {
			get { return GetChildByRole (Roles.RPar); }
		}
		
		public CppTokenNode SemicolonToken {
			get { return GetChildByRole (Roles.Semicolon); }
		}
		
		public override S AcceptVisitor<T, S> (IAstVisitor<T, S> visitor, T data = default(T))
		{
			return visitor.VisitDoWhileStatement (this, data);
		}
		
		protected internal override bool DoMatch(AstNode other, PatternMatching.Match match)
		{
			DoWhileStatement o = other as DoWhileStatement;
			return o != null && this.EmbeddedStatement.DoMatch(o.EmbeddedStatement, match) && this.Condition.DoMatch(o.Condition, match);
		}
	}
}

