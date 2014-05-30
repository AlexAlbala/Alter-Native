// 
// EmptyStatement.cs
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

using System;
using ICSharpCode.NRefactory.Cpp.Ast;
namespace ICSharpCode.NRefactory.Cpp
{
    /// <summary>
    /// ;
    /// </summary>
    public class TranslationException : Expression
    {
        public static readonly Role<Exception> ExceptionRole = new Role<Exception>("Exception", new Exception());
        public static readonly Role<CSharp.AstNode> InnerNodeRole = new Role<CSharp.AstNode>("Node", CSharp.AstNode.Null);

        private Exception innerException;
        private CSharp.AstNode innerNode;
        public Exception exception
        {
            get
            {
                return innerException;
            }
            set
            {
                innerException = value;
            }
        }

        public CSharp.AstNode node
        {
            get
            {
                return innerNode;
            }
            set
            {
                innerNode = value;
            }
        }

        /*public static implicit operator Statement(TranslationException _this)
        {
            Statement st = new ExpressionStatement(new TranslationException() { exception = _this.exception, node = _this.node });
            return st;
        }*/

        public static implicit operator Statement(TranslationException _this)
        {
            Statement st = new ExpressionStatement(new TranslationException() { exception = _this.exception, node = _this.node });
            return st;
        }

        public static implicit operator AttributedNode(TranslationException _this)
        {
            return _this;
        }

        public override S AcceptVisitor<T, S>(IAstVisitor<T, S> visitor, T data = default(T))
        {
            return visitor.VisitTranslationException(this, data);
        }

        protected internal override bool DoMatch(AstNode other, PatternMatching.Match match)
        {
            EmptyStatement o = other as EmptyStatement;
            return o != null;
        }
    }
}
