using ICSharpCode.NRefactory.Cpp.Ast;
namespace ICSharpCode.NRefactory.Cpp
{
    public class HeaderDestructorDeclaration : AttributedNode
    {
        public static readonly Role<CppTokenNode> TildeRole = new Role<CppTokenNode>("Tilde", CppTokenNode.Null);

        public CppTokenNode TildeToken
        {
            get { return GetChildByRole(TildeRole); }
        }

        /// <summary>
        /// Gets/Sets the name of the class containing the destructor.
        /// This property can be used to inform the output visitor about the class name when writing a destructor declaration
        /// without writing the complete type declaration. It is ignored when the destructor has a type declaration as parent.
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

        public CppTokenNode RParToken
        {
            get { return GetChildByRole(Roles.RPar); }
        }

        public override S AcceptVisitor<T, S>(IAstVisitor<T, S> visitor, T data = default(T))
        {
            return visitor.VisitHeaderDestructorDeclaration(this, data);
        }

        protected internal override bool DoMatch(AstNode other, PatternMatching.Match match)
        {
            DestructorDeclaration o = other as DestructorDeclaration;
            return o != null && this.MatchAttributesAndModifiers(o, match);
        }
    }
}
