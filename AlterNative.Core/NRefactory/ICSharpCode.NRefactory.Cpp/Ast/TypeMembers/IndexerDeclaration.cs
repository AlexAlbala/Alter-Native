﻿// 
// IndexerDeclaration.cs
//
// Author:
//       Mike Krüger <mkrueger@novell.com>
// 
// Copyright (c) 2009 Novell, Inc (http://www.novell.com)
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

using System.Collections.Generic;
using System.Linq;

namespace ICSharpCode.NRefactory.Cpp
{
	public class IndexerDeclaration : MemberDeclaration
	{
        public static readonly Role<MethodDeclaration> GetterRole = PropertyDeclaration.GetterRole;
        public static readonly Role<MethodDeclaration> SetterRole = PropertyDeclaration.SetterRole;
		
		public CppTokenNode LBracketToken {
			get { return GetChildByRole (Roles.LBracket); }
		}
		
		public AstNodeCollection<ParameterDeclaration> Parameters {
			get { return GetChildrenByRole (Roles.Parameter); }
		}
		
		public CppTokenNode RBracketToken {
			get { return GetChildByRole (Roles.RBracket); }
		}
		
		public CppTokenNode LBraceToken {
			get { return GetChildByRole (Roles.LBrace); }
		}

        public MethodDeclaration Getter
        {
			get { return GetChildByRole(GetterRole); }
			set { SetChildByRole(GetterRole, value); }
		}

        public MethodDeclaration Setter
        {
			get { return GetChildByRole(SetterRole); }
			set { SetChildByRole(SetterRole, value); }
		}
		
		public CppTokenNode RBraceToken {
			get { return GetChildByRole (Roles.RBrace); }
		}
		
		public override S AcceptVisitor<T, S> (IAstVisitor<T, S> visitor, T data = default(T))
		{
			return visitor.VisitIndexerDeclaration (this, data);
		}
		
		protected internal override bool DoMatch(AstNode other, PatternMatching.Match match)
		{
			IndexerDeclaration o = other as IndexerDeclaration;
			return o != null && this.MatchMember(o, match) && this.Parameters.DoMatch(o.Parameters, match)
				&& this.Getter.DoMatch(o.Getter, match) && this.Setter.DoMatch(o.Setter, match);
		}
	}
}
