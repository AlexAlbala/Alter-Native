using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ICSharpCode.NRefactory.Cpp.Ast
{
    public class PointerExpression : Expression
    {
        public readonly static Role<CppTokenNode> AsteriskRole = new Role<CppTokenNode>("Asterisk", CppTokenNode.Null);

        public PointerExpression()
        {
        }

        public PointerExpression(Expression Target)
        {
            SetChildByRole(Roles.TargetExpression, Target);
        }
        public Expression Target
        {
            get { return GetChildByRole(Roles.TargetExpression); }
            set { SetChildByRole(Roles.TargetExpression, value); }
        }

        public override S AcceptVisitor<T, S>(IAstVisitor<T, S> visitor, T data)
        {
            return visitor.VisitPointerExpression(this, data);
        }

        protected internal override bool DoMatch(AstNode other, PatternMatching.Match match)
        {
            throw new NotImplementedException();
        }
    }
}
