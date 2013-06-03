
using ICSharpCode.NRefactory.Cpp.Ast;
namespace ICSharpCode.NRefactory.Cpp
{
    /// <summary>
    /// (CastTo)Expression
    /// </summary>
    public class DynamicCastExpression : Expression
    {
        public AstType Type
        {
            get { return GetChildByRole(Roles.Type); }
            set { SetChildByRole(Roles.Type, value); }
        }

        public CppTokenNode RParToken
        {
            get { return GetChildByRole(Roles.RPar); }
        }

        public Expression Expression
        {
            get { return GetChildByRole(Roles.Expression); }
            set { SetChildByRole(Roles.Expression, value); }
        }

        public DynamicCastExpression()
        {
        }

        public DynamicCastExpression(AstType castToType, Expression expression)
        {
            AddChild(castToType, Roles.Type);
            AddChild(expression, Roles.Expression);
        }

        public override S AcceptVisitor<T, S>(IAstVisitor<T, S> visitor, T data = default(T))
        {
            return visitor.VisitDynamicCastExpression(this, data);
        }

        protected internal override bool DoMatch(AstNode other, PatternMatching.Match match)
        {
            CastExpression o = other as CastExpression;
            return o != null && this.Type.DoMatch(o.Type, match) && this.Expression.DoMatch(o.Expression, match);
        }
    }
}

