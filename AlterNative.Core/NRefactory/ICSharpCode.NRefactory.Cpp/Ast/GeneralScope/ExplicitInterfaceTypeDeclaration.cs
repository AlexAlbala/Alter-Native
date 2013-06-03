using System;
using ICSharpCode.NRefactory.Cpp.Ast;

namespace ICSharpCode.NRefactory.Cpp.Ast
{
    /// <summary>
    /// Type&lt;[EMPTY]&gt;
    /// </summary>
    public class ExplicitInterfaceTypeDeclaration : AttributedNode
    {
        public static readonly new ExplicitInterfaceTypeDeclaration Null = new NullExplicitInterfaceTypeDeclaration();
        public readonly static Role<TypeDeclaration> TypeRole = new Role<TypeDeclaration>("Type", TypeDeclaration.Null);
        public readonly static Role<AttributedNode> OutMemberRole = new Role<AttributedNode>("OutMember");

        sealed class NullExplicitInterfaceTypeDeclaration : ExplicitInterfaceTypeDeclaration
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

        public ExplicitInterfaceTypeDeclaration() { }

        public ExplicitInterfaceTypeDeclaration(TypeDeclaration type)
        {
            this.Type = type;
        }

        public TypeDeclaration Type
        {
            get { return GetChildByRole(TypeRole); }
            set { SetChildByRole(TypeRole, value); }
        }

        /// <summary>
        /// Members used for accessing the nested type. These members are declared in the parent type
        /// </summary>
        public AstNodeCollection<AttributedNode> OutMembers
        {
            get { return GetChildrenByRole(OutMemberRole); }
        }

        public override S AcceptVisitor<T, S>(IAstVisitor<T, S> visitor, T data = default(T))
        {
            return visitor.VisitExplicitInterfaceTypeDeclaration(this, data);
        }

        protected internal override bool DoMatch(AstNode other, PatternMatching.Match match)
        {
            var o = other as EmptyExpression;
            return o != null;
        }
    }
}
