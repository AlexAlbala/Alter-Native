using System;
using System.Collections.Generic;
using System.Linq;
using ICSharpCode.NRefactory.Cpp.Ast;

namespace ICSharpCode.NRefactory.Cpp
{
    public class VariadicParameterDeclaration : ParameterDeclaration
    {
        public static readonly Role<CppTokenNode> VariadicRole = new Role<CppTokenNode>("Variadic", CppTokenNode.Null);

        public override S AcceptVisitor<T, S>(IAstVisitor<T, S> visitor, T data = default(T))
        {
            return visitor.VisitVariadicParameterDeclaration(this, data);
        }

        protected internal override bool DoMatch(AstNode other, PatternMatching.Match match)
        {
            VariadicParameterDeclaration o = other as VariadicParameterDeclaration;
            return o != null && this.Attributes.DoMatch(o.Attributes, match) && this.ParameterModifier == o.ParameterModifier
                && this.Type.DoMatch(o.Type, match) && MatchString(this.Name, o.Name)
                && this.DefaultExpression.DoMatch(o.DefaultExpression, match);
        }

        public VariadicParameterDeclaration()
        {
        }
    }
}

