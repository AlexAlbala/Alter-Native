using System.Collections.Generic;
using ICSharpCode.NRefactory.Cpp.Ast;
using System.Text;

namespace ICSharpCode.NRefactory.Cpp
{
    /// <summary>
    /// Target(Arguments)
    /// </summary>
    public class HeaderMacroExpression : AttributedNode
    {
        public Expression Target
        {
            get { return GetChildByRole(Roles.TargetExpression); }
            set { SetChildByRole(Roles.TargetExpression, value); }
        }

        public CppTokenNode LParToken
        {
            get { return GetChildByRole(Roles.LPar); }
        }

        public AstNodeCollection<Expression> Arguments
        {
            get { return GetChildrenByRole<Expression>(Roles.Argument); }
        }

        public CppTokenNode RParToken
        {
            get { return GetChildByRole(Roles.RPar); }
        }

        public override S AcceptVisitor<T, S>(IAstVisitor<T, S> visitor, T data = default(T))
        {
            return visitor.VisitHeaderMacroExpression(this, data);
        }

        public HeaderMacroExpression()
        {
        }

        public HeaderMacroExpression(Expression target, IEnumerable<Expression> arguments)
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

        public HeaderMacroExpression(Expression target, params Expression[] arguments)
            : this(target, (IEnumerable<Expression>)arguments)
        {
        }

        protected internal override bool DoMatch(AstNode other, PatternMatching.Match match)
        {
            InvocationExpression o = other as InvocationExpression;
            return o != null && this.Target.DoMatch(o.Target, match) && this.Arguments.DoMatch(o.Arguments, match);
        }

        public override string ToString()
        {
            StringBuilder b = new StringBuilder();
            b.Append(Target);
            b.Append("(");
            bool isfirst = true;
            foreach (Expression arg in Arguments)
            {
                if (!isfirst)
                    b.Append(",");
                else
                    isfirst = false;
                b.Append(arg.ToString());
            }
            b.Append(")");
            return b.ToString();
        }
    }
}
