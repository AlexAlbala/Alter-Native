namespace ICSharpCode.NRefactory.CSharp
{
    /// <summary>
    /// (UnBox)Expression
    /// </summary>
    public class UnBoxExpression : Expression
    {
        Role<AstType> unBoxTypeRole = new Role<AstType>("UnBoxType", AstType.Null);
        public Expression Expression
        {
            get { return GetChildByRole(Roles.Expression); }
            set { SetChildByRole(Roles.Expression, value); }
        }

        public UnBoxExpression()
        {
        }

        public UnBoxExpression(Expression expression)
        {
            AddChild(expression, Roles.Expression);
        }

        public AstType type
        {
            get { return GetChildByRole(unBoxTypeRole); }
            set { SetChildByRole(unBoxTypeRole, value); }
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitUnBoxExpression(this);
        }

        public override T AcceptVisitor<T>(IAstVisitor<T> visitor)
        {
            return visitor.VisitUnBoxExpression(this);
        }

        public override S AcceptVisitor<T, S>(IAstVisitor<T, S> visitor, T data = default(T))
        {
            return visitor.VisitUnBoxExpression(this, data);
        }

        protected internal override bool DoMatch(AstNode other, PatternMatching.Match match)
        {
            CastExpression o = other as CastExpression;
            return o != null && this.Expression.DoMatch(o.Expression, match);
        }
    }
}
