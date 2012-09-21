// 
// TypeDeclaration.cs
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
using ICSharpCode.NRefactory.TypeSystem;
using ICSharpCode.NRefactory.Cpp.Ast;

namespace ICSharpCode.NRefactory.Cpp.Ast
{
	public enum ClassType
	{
		Class,
		Struct,
		Interface,
		Enum
	}
	
	/// <summary>
	/// class Name&lt;TypeParameters&gt; : BaseTypes where Constraints;
	/// </summary>
	public class TypeDeclaration : AttributedNode
	{
        public static readonly new TypeDeclaration Null = new NullTypeDeclaration();
		public readonly static Role<CppTokenNode> ColonRole = Roles.Colon;
		public readonly static Role<AstType> BaseTypeRole = new Role<AstType>("BaseType", AstType.Null);        
		public readonly static Role<AttributedNode> MemberRole = new Role<AttributedNode>("Member");

        sealed class NullTypeDeclaration : TypeDeclaration
        {
            public override bool IsNull
            {
                get
                {
                    return true;
                }
            }

            public override S AcceptVisitor<T, S>(IAstVisitor<T, S> visitor, T data = default(T))
            {
                return default(S);
            }

            protected internal override bool DoMatch(AstNode other, PatternMatching.Match match)
            {
                return other == null || other.IsNull;
            }
        }
		
        //public override NodeType NodeType {
        //    get {
        //        return NodeType.TypeDeclaration;
        //    }
        //}
		
		public ClassType ClassType {
			get;
			set;
		}
		
		public string Name {
			get {
				return GetChildByRole (Roles.Identifier).Name;
			}
			set {
				SetChildByRole (Roles.Identifier, Identifier.Create (value, TextLocation.Empty));
			}
		}
		
		public Identifier NameToken {
			get {
				return GetChildByRole (Roles.Identifier);
			}
			set {
				SetChildByRole (Roles.Identifier, value);
			}
		}
		
		public AstNodeCollection<TypeParameterDeclaration> TypeParameters {
			get { return GetChildrenByRole (Roles.TypeParameter); }
		}
		
		public AstNodeCollection<AstType> BaseTypes {
			get { return GetChildrenByRole (BaseTypeRole); }
		}
		
        //public AstNodeCollection<Constraint> Constraints {
        //    get { return GetChildrenByRole (Roles.Constraint); }
        //}
		
		public CppTokenNode LBraceToken {
			get { return GetChildByRole (Roles.LBrace); }
		}
		
		public AstNodeCollection<AttributedNode> Members {
			get { return GetChildrenByRole (MemberRole); }
		}
		
		public CppTokenNode RBraceToken {
			get { return GetChildByRole (Roles.RBrace); }
		}
		
		public override S AcceptVisitor<T, S> (IAstVisitor<T, S> visitor, T data = default(T))
		{
			return visitor.VisitTypeDeclaration (this, data);
		}
		
		protected internal override bool DoMatch(AstNode other, PatternMatching.Match match)
		{
			TypeDeclaration o = other as TypeDeclaration;
			return o != null && this.ClassType == o.ClassType && this.MatchAttributesAndModifiers(o, match)
				&& MatchString(this.Name, o.Name) && this.TypeParameters.DoMatch(o.TypeParameters, match)
				&& this.BaseTypes.DoMatch(o.BaseTypes, match) /*&& this.Constraints.DoMatch(o.Constraints, match)*/
				&& this.Members.DoMatch(o.Members, match);
		}
	}
}
