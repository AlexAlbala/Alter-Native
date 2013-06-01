using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ICSharpCode.NRefactory.Cpp.Ast
{
    public class AddressOfExpression : Expression
    {
        public readonly static Role<CppTokenNode> AmpersandRole = new Role<CppTokenNode>("Ampersand", CppTokenNode.Null);

        public Expression Target
        {
            get { return GetChildByRole(Roles.TargetExpression); }
            set { SetChildByRole(Roles.TargetExpression, value); }
        }

        public AddressOfExpression ()
		{
		}

        public AddressOfExpression(Expression targetExpression)
		{
            AddChild(targetExpression, Roles.TargetExpression);
		}

        public override S AcceptVisitor<T, S>(IAstVisitor<T, S> visitor, T data)
        {
            return visitor.VisitAddressOfExpression(this, data);
        }

        protected internal override bool DoMatch(AstNode other, PatternMatching.Match match)
        {
            throw new NotImplementedException();
        }
    }
}
