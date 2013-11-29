using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ICSharpCode.NRefactory.Cpp.Ast
{
    public class GlobalNamespaceReferenceExpression : Expression
    {
        public Expression Target
        {
            get { return GetChildByRole(Roles.TargetExpression); }
            set { SetChildByRole(Roles.TargetExpression, value); }
        }

        public override S AcceptVisitor<T, S>(IAstVisitor<T, S> visitor, T data = default(T))
        {
            return visitor.VisitGlobalNamespaceReferenceReferenceExpression(this, data);
        }

        public GlobalNamespaceReferenceExpression(Expression target)
        {
            AddChild(target, Roles.TargetExpression);
        }

        protected internal override bool DoMatch(AstNode other, PatternMatching.Match match)
        {
            TypeReferenceExpression o = other as TypeReferenceExpression;
            return o != null && this.Target.DoMatch(o.Type, match);
        }

    }
}
