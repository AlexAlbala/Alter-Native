using System;
using ICSharpCode.NRefactory.Cpp.Ast;

namespace ICSharpCode.NRefactory.Cpp.Ast
{
    /// <summary>
    /// Type&lt;[EMPTY]&gt;
    /// </summary>
    public class InterfaceTypeDeclaration : TypeDeclaration
    {
        public static readonly new InterfaceTypeDeclaration Null = new NullInterfaceTypeDeclaration();

        sealed class NullInterfaceTypeDeclaration : InterfaceTypeDeclaration
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

        public InterfaceTypeDeclaration() { }

        public InterfaceTypeDeclaration(TypeDeclaration type)
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
        }

        public override S AcceptVisitor<T, S>(IAstVisitor<T, S> visitor, T data = default(T))
        {
            return visitor.VisitInterfaceTypeDeclaration(this, data);
        }

        protected internal override bool DoMatch(AstNode other, PatternMatching.Match match)
        {
            var o = other as EmptyExpression;
            return o != null;
        }
    }
}
