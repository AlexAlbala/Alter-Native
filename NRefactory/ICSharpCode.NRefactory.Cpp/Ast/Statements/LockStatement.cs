using ICSharpCode.NRefactory.Cpp.Ast;
namespace ICSharpCode.NRefactory.Cpp
{
	/// <summary>
	/// lock (Expression) EmbeddedStatement;
	/// </summary>
	public class LockStatement : Statement
	{
        public static readonly Role<CppTokenNode> LockKeywordRole = new Role<CppTokenNode>("lock");
		
		public CppTokenNode LockToken {
			get { return GetChildByRole (LockKeywordRole); }
		}

        public CppTokenNode LParToken
        {
			get { return GetChildByRole (Roles.LPar); }
		}
		
		public Expression Expression {
			get { return GetChildByRole (Roles.Expression); }
			set { SetChildByRole (Roles.Expression, value); }
		}

        public CppTokenNode RParToken
        {
			get { return GetChildByRole (Roles.RPar); }
		}
		
		public Statement EmbeddedStatement {
			get { return GetChildByRole (Roles.EmbeddedStatement); }
			set { SetChildByRole (Roles.EmbeddedStatement, value); }
		}

		public override S AcceptVisitor<T, S> (IAstVisitor<T, S> visitor, T data)
		{
			return visitor.VisitLockStatement (this, data);
		}
		
		protected internal override bool DoMatch(AstNode other, PatternMatching.Match match)
		{
			LockStatement o = other as LockStatement;
			return o != null && this.Expression.DoMatch(o.Expression, match) && this.EmbeddedStatement.DoMatch(o.EmbeddedStatement, match);
		}
	}
}
