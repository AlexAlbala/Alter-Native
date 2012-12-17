using ICSharpCode.NRefactory.Cpp.Ast;
namespace ICSharpCode.NRefactory.Cpp
{
    public class ExitScopeStatement : Statement
    {
        public static readonly Role<CppTokenNode> BoostExitScopeKeywordRole = new Role<CppTokenNode>("BoostEndScope", CppTokenNode.Null);
        public static readonly Role<CppTokenNode> BoostExitScopeEndRoleKeyword = new Role<CppTokenNode>("BoostEndScopeEnd", CppTokenNode.Null);
        public static readonly Role<BlockStatement> FinallyBlockRole = new Role<BlockStatement>("FinallyBlock", BlockStatement.Null);
        public static readonly Role<Expression> FinallyVarRole = new Role<Expression>("FinallyVar", Expression.Null);

        public new static readonly ExitScopeStatement Null = new NullExitScopeStatement();
        sealed class NullExitScopeStatement : ExitScopeStatement
        {
            public override bool IsNull
            {
                get
                {
                    return true;
                }
            }

            public override S AcceptVisitor<T, S>(IAstVisitor<T, S> visitor, T data = default(T))
            {
                return default(S);
            }

            protected internal override bool DoMatch(AstNode other, PatternMatching.Match match)
            {
                return other == null || other.IsNull;
            }
        }

        public CppTokenNode SemicolonToken
        {
            get { return GetChildByRole(Roles.Semicolon); }
        }

        public BlockStatement Block
        {
            get { return GetChildByRole(FinallyBlockRole); }
            set { SetChildByRole(FinallyBlockRole, value); }
        }

        public AstNodeCollection<Expression> Variables
        {
            get { return GetChildrenByRole(FinallyVarRole); }
        }

        public override S AcceptVisitor<T, S>(IAstVisitor<T, S> visitor, T data = default(T))
        {
            return visitor.VisitEndScopeStatement(this, data);
        }

        public ExitScopeStatement()
        {
        }

        protected internal override bool DoMatch(AstNode other, PatternMatching.Match match)
        {
            ExpressionStatement o = other as ExpressionStatement;
            return o != null;
        }
    }
}
