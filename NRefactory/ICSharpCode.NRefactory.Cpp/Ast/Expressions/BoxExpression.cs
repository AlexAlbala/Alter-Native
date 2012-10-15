using ICSharpCode.NRefactory.Cpp.Ast;
namespace ICSharpCode.NRefactory.Cpp
{
    /// <summary>
    /// (Box)Expression
    /// </summary>
    public class BoxExpression : Expression
    {
        Role<AstType> boxTypeRole = new Role<AstType>("BoxType", AstType.Null);
        public Expression Expression
        {
            get { return GetChildByRole(Roles.Expression); }
            set { SetChildByRole(Roles.Expression, value); }
        }

        public BoxExpression()
        {
        }

        public BoxExpression(Expression expression)
        {            
            AddChild(expression, Roles.Expression);
        }

        public AstType type
        {
            get { return GetChildByRole(boxTypeRole); }
            set { SetChildByRole(boxTypeRole, value); }
        }

        public override S AcceptVisitor<T, S>(IAstVisitor<T, S> visitor, T data = default(T))
        {
            return visitor.VisitBoxExpression(this, data);
        }

        protected internal override bool DoMatch(AstNode other, PatternMatching.Match match)
        {
            CastExpression o = other as CastExpression;
            return o != null && this.Expression.DoMatch(o.Expression, match);
        }
    }
}

