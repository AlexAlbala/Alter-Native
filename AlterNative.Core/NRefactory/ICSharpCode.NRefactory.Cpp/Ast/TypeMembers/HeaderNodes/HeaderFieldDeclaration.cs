using System.Collections.Generic;
using System.Linq;
using ICSharpCode.NRefactory.Cpp.Ast;

namespace ICSharpCode.NRefactory.Cpp
{
    public class HeaderFieldDeclaration : AttributedNode
    {

        public AstType ReturnType
        {
            get { return GetChildByRole(Roles.Type); }
            set { SetChildByRole(Roles.Type, value); }
        }

        public AstNodeCollection<VariableInitializer> Variables
        {
            get { return GetChildrenByRole(Roles.Variable); }
        }

        public override S AcceptVisitor<T, S>(IAstVisitor<T, S> visitor, T data = default(T))
        {
            return visitor.VisitHeaderFieldDeclaration(this, data);
        }

        protected internal override bool DoMatch(AstNode other, PatternMatching.Match match)
        {
            FieldDeclaration o = other as FieldDeclaration;
            return o != null && this.MatchAttributesAndModifiers(o, match)
                && this.ReturnType.DoMatch(o.ReturnType, match) && this.Variables.DoMatch(o.Variables, match);
        }
    }


}
