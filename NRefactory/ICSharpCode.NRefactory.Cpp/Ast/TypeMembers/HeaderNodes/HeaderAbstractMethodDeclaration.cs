
using System.Collections.Generic;
using System.Linq;
using ICSharpCode.NRefactory.Cpp.Ast;
using System;

namespace ICSharpCode.NRefactory.Cpp
{
    public class HeaderAbstractMethodDeclaration : MethodDeclaration
    {
        public static readonly Role<CppTokenNode> EqualToken = new Role<CppTokenNode>("Equal", CppTokenNode.Null);
        public static readonly Role<CppTokenNode> ZeroToken = new Role<CppTokenNode>("Zero", CppTokenNode.Null);

        public new AstNodeCollection<TypeParameterDeclaration> TypeParameters
        {
            get { return GetChildrenByRole(Roles.TypeParameter); }
        }

        public new CppTokenNode LParToken
        {
            get { return GetChildByRole(Roles.LPar); }
        }

        public string Namespace
        {
            get;
            set;
        }

        public new AstNodeCollection<ParameterDeclaration> Parameters
        {
            get { return GetChildrenByRole(Roles.Parameter); }
        }

        public new CppTokenNode RParToken
        {
            get { return GetChildByRole(Roles.RPar); }
        }

        public new bool IsExtensionMethod
        {
            get
            {
                ParameterDeclaration pd = (ParameterDeclaration)GetChildByRole(Roles.Parameter);
                return pd != null && pd.ParameterModifier == ParameterModifier.This;
            }
        }

        public override S AcceptVisitor<T, S>(IAstVisitor<T, S> visitor, T data = default(T))
        {
            return visitor.VisitHeaderAbstractMethodDeclaration(this, data);
        }

        protected internal override bool DoMatch(AstNode other, PatternMatching.Match match)
        {
            MethodDeclaration o = other as MethodDeclaration;
            return o != null && this.MatchMember(o, match) && this.TypeParameters.DoMatch(o.TypeParameters, match)
                && this.Parameters.DoMatch(o.Parameters, match);
        }
    }
}
