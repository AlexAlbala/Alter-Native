using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ICSharpCode.NRefactory.Cpp.Ast
{
    public class DelegateInvokeExpression : InvocationExpression
    {
        public override S AcceptVisitor<T, S>(IAstVisitor<T, S> visitor, T data = default(T))
        {
            return visitor.VisitDelegateInvokeExpression(this, data);
        }

        public DelegateInvokeExpression()
        {
        }

        public DelegateInvokeExpression(Expression target, IEnumerable<Expression> arguments)
        {
            AddChild(target, Roles.TargetExpression);
            if (arguments != null)
            {
                foreach (var arg in arguments)
                {
                    AddChild(arg, Roles.Argument);
                }
            }
        }
    }
}
