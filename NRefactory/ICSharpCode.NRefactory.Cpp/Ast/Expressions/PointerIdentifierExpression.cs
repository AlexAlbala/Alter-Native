using System.Collections.Generic;
using ICSharpCode.NRefactory.Cpp.Ast;

namespace ICSharpCode.NRefactory.Cpp
{
    public class PointerIdentifierExpression : Expression
    {
        public PointerIdentifierExpression()
        {
            //base.generateNewId();
        }

        public PointerIdentifierExpression(string identifier)
        {
            this.Identifier = identifier;
        }

        public PointerIdentifierExpression(string identifier, TextLocation location)
        {
            SetChildByRole(Roles.Identifier, Ast.Identifier.Create(identifier, location));
        }

        public Identifier IdentifierToken
        {
            get { return GetChildByRole(Roles.Identifier); }
            set { SetChildByRole(Roles.Identifier, value); }
        }

        public string Identifier
        {
            get
            {
                return GetChildByRole(Roles.Identifier).Name;
            }
            set
            {
                SetChildByRole(Roles.Identifier, Ast.Identifier.Create(value, TextLocation.Empty));
            }
        }

        public override S AcceptVisitor<T, S>(IAstVisitor<T, S> visitor, T data = default(T))
        {
            return visitor.VisitPointerIdentifierExpression(this, data);
        }

        protected internal override bool DoMatch(AstNode other, PatternMatching.Match match)
        {
            IdentifierExpression o = other as IdentifierExpression;
            return o != null && MatchString(this.Identifier, o.Identifier);
        }
    }
}
