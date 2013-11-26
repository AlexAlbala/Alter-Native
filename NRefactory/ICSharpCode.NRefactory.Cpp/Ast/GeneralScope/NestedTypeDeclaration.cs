using System;
using ICSharpCode.NRefactory.Cpp.Ast;

namespace ICSharpCode.NRefactory.Cpp.Ast
{
    /// <summary>
    /// Type&lt;[EMPTY]&gt;
    /// </summary>
    public class NestedTypeDeclaration : TypeDeclaration
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
            this.Name = type.Name;

            foreach (var member in type.Members)
                this.Members.Add((AttributedNode)member.Clone());

            foreach (var header in type.HeaderNodes)
                this.HeaderNodes.Add(header.Clone());

            foreach (var baseType in type.BaseTypes)
                this.BaseTypes.Add((AstType)baseType.Clone());

            foreach (var mod in type.ModifierTokens)
                this.ModifierTokens.Add((CppModifierToken)mod.Clone());

            foreach (var typePar in type.TypeParameters)
            {
                this.TypeParameters.Add((TypeParameterDeclaration)typePar.Clone());
            }

            this.ClassType = type.ClassType;
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
