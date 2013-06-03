using System.Collections.Generic;
using System.Linq;
using ICSharpCode.NRefactory.Cpp.Ast;

namespace ICSharpCode.NRefactory.Cpp
{
    public class HeaderConstructorDeclaration : AttributedNode
    {
        public static readonly Role<ConstructorInitializer> InitializerRole = new Role<ConstructorInitializer>("Initializer", ConstructorInitializer.Null);

        /// <summary>
        /// Gets/Sets the name of the class containing the constructor.
        /// This property can be used to inform the output visitor about the class name when writing a constructor declaration
        /// without writing the complete type declaration. It is ignored when the constructor has a type declaration as parent.
        /// </summary>
        public string Name { get; set; }

        public Identifier IdentifierToken
        {
            get { return GetChildByRole(Roles.Identifier); }
            set { SetChildByRole(Roles.Identifier, value); }
        }

        public CppTokenNode LParToken
        {
            get { return GetChildByRole(Roles.LPar); }
        }

        public AstNodeCollection<ParameterDeclaration> Parameters
        {
            get { return GetChildrenByRole(Roles.Parameter); }
        }

        public CppTokenNode RParToken
        {
            get { return GetChildByRole(Roles.RPar); }
        }

        public CppTokenNode ColonToken
        {
            get { return GetChildByRole(Roles.Colon); }
        }

        public ConstructorInitializer Initializer
        {
            get { return GetChildByRole(InitializerRole); }
            set { SetChildByRole(InitializerRole, value); }
        }

        public override S AcceptVisitor<T, S>(IAstVisitor<T, S> visitor, T data = default(T))
        {
            return visitor.VisitHeaderConstructorDeclaration(this, data);
        }

        protected internal override bool DoMatch(AstNode other, PatternMatching.Match match)
        {
            ConstructorDeclaration o = other as ConstructorDeclaration;
            return o != null && this.MatchAttributesAndModifiers(o, match) && this.Parameters.DoMatch(o.Parameters, match)
                && this.Initializer.DoMatch(o.Initializer, match);
        }
    }
}
