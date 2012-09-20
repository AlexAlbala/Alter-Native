
using System.Collections.Generic;
using System.Linq;
using ICSharpCode.NRefactory.Cpp.Ast;
using System;

namespace ICSharpCode.NRefactory.Cpp
{
    public class HeaderMethodDeclaration : MemberDeclaration
    {
        public static readonly new HeaderMethodDeclaration Null = new HeaderNullMethodDeclaration();
        public static readonly Role<Identifier> TypeRole = new Role<Identifier>("type", Identifier.Null);

        sealed class HeaderNullMethodDeclaration : HeaderMethodDeclaration
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

        public AstNodeCollection<TypeParameterDeclaration> TypeParameters
        {
            get { return GetChildrenByRole(Roles.TypeParameter); }
        }

        public CppTokenNode LParToken
        {
            get { return GetChildByRole(Roles.LPar); }
        }

        public Identifier TypeMember
        {
            get { return GetChildByRole(TypeRole); }
            set { SetChildByRole(TypeRole, value); }
        }

        public string Namespace
        {
            get;
            set;
        }

        public AstNodeCollection<ParameterDeclaration> Parameters
        {
            get { return GetChildrenByRole(Roles.Parameter); }
        }

        public CppTokenNode RParToken
        {
            get { return GetChildByRole(Roles.RPar); }
        }

        public bool IsExtensionMethod
        {
            get
            {
                ParameterDeclaration pd = (ParameterDeclaration)GetChildByRole(Roles.Parameter);
                return pd != null && pd.ParameterModifier == ParameterModifier.This;
            }
        }

        public override S AcceptVisitor<T, S>(IAstVisitor<T, S> visitor, T data = default(T))
        {
            return visitor.VisitHeaderMethodDeclaration(this, data);
        }

        protected internal override bool DoMatch(AstNode other, PatternMatching.Match match)
        {
            MethodDeclaration o = other as MethodDeclaration;
            return o != null && this.MatchMember(o, match) && this.TypeParameters.DoMatch(o.TypeParameters, match)
                && this.Parameters.DoMatch(o.Parameters, match);
        }
    }
}
