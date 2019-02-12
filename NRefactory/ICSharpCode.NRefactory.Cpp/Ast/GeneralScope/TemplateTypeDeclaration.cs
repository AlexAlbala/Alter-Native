using System.Collections.Generic;
using System.Linq;
using ICSharpCode.NRefactory.TypeSystem;
using ICSharpCode.NRefactory.Cpp.Ast;

namespace ICSharpCode.NRefactory.Cpp.Ast
{

    /// <summary>
    /// class Name&lt;TypeParameters&gt; : BaseTypes where Constraints;
    /// </summary>
    public class TemplateTypeDeclaration : TypeDeclaration
    {
        public readonly static Role<AstType> ArgumentTypeRole = new Role<AstType>("ArgumentType", AstType.Null);

        public new AstNodeCollection<AstType> TypeParameters
        {
            get { return GetChildrenByRole(ArgumentTypeRole); }
        }

        public override S AcceptVisitor<T, S>(IAstVisitor<T, S> visitor, T data = default(T))
        {
            return visitor.VisitTemplateTypeDeclaration(this, data);
        }

        protected internal override bool DoMatch(AstNode other, PatternMatching.Match match)
        {
            TemplateTypeDeclaration o = other as TemplateTypeDeclaration;
            return o != null && this.ClassType == o.ClassType && this.MatchAttributesAndModifiers(o, match)
                && MatchString(this.Name, o.Name) && this.TypeParameters.DoMatch(o.TypeParameters, match)
                && this.BaseTypes.DoMatch(o.BaseTypes, match) /*&& this.Constraints.DoMatch(o.Constraints, match)*/
                && this.Members.DoMatch(o.Members, match);
        }
    }
}
