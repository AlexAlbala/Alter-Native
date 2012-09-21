using System;
using ICSharpCode.NRefactory.Cpp.Ast;

namespace ICSharpCode.NRefactory.Cpp.Ast
{
    /// <summary>
    /// Type&lt;[EMPTY]&gt;
    /// </summary>
    public class NestedTypeDeclaration : AttributedNode
    {
        public static readonly new NestedTypeDeclaration Null = new NullNestedTypeDeclaration();
        public readonly static Role<TypeDeclaration> TypeRole = new Role<TypeDeclaration>("Type", TypeDeclaration.Null);

        sealed class NullNestedTypeDeclaration : NestedTypeDeclaration
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

        public NestedTypeDeclaration() { }

        public NestedTypeDeclaration(TypeDeclaration type)
        {
            this.Type = type;
        }

        public TypeDeclaration Type
        {
            get { return GetChildByRole(TypeRole); }
            set { SetChildByRole(TypeRole, value); }
        }

        public override S AcceptVisitor<T, S>(IAstVisitor<T, S> visitor, T data = default(T))
        {
            return visitor.VisitNestedTypeDeclaration(this, data);
        }

        protected internal override bool DoMatch(AstNode other, PatternMatching.Match match)
        {
            var o = other as EmptyExpression;
            return o != null;
        }
    }
}
