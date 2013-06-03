// 
// ForeachStatement.cs
//
// Author:
//       Mike Krüger <mkrueger@novell.com>
// 
// Copyright (c) 2009 Novell, Inc (http://www.novell.com)
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using ICSharpCode.NRefactory.Cpp.Ast;
namespace ICSharpCode.NRefactory.Cpp
{
    /// <summary>
    /// foreach (Type VariableName in InExpression) EmbeddedStatement
    /// </summary>
    public class ForeachStatement : Statement
    {
        public readonly static Role<Identifier> VariableRole = new Role<Identifier>("VarId", Identifier.Null);
        public readonly static Role<Expression> CollectionRole = new Role<Expression>("ColEx", Expression.Null);

        public Statement ForEachStatement
        {
            get { return GetChildByRole(Roles.EmbeddedStatement); }
            set { SetChildByRole(Roles.EmbeddedStatement, value); }
        }

        public Identifier VariableIdentifier
        {
            get { return GetChildByRole(VariableRole); }
            set { SetChildByRole(VariableRole, value); }
        }

        public Expression CollectionExpression
        {
            get { return GetChildByRole(CollectionRole); }
            set { SetChildByRole(CollectionRole, value); }
        }

        public override S AcceptVisitor<T, S>(IAstVisitor<T, S> visitor, T data = default(T))
        {
            return visitor.VisitForeachStatement(this, data);
        }

        protected internal override bool DoMatch(AstNode other, PatternMatching.Match match)
        {
            ForeachStatement o = other as ForeachStatement;
            return o != null && this.ForEachStatement.DoMatch(o.ForEachStatement, match);
        }
    }
}
