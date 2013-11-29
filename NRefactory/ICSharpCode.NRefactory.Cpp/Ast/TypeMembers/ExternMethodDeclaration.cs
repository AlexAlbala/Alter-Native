using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.NRefactory.Cpp.Ast;

namespace ICSharpCode.NRefactory.Cpp
{
    public class ExternMethodDeclaration : MethodDeclaration
    {
        public static readonly Role<Identifier> EntryPointRole = new Role<Identifier>("EntryPoint", Ast.Identifier.Null);
        public static readonly Role<Identifier> LibraryRole = new Role<Identifier>("Library", Ast.Identifier.Null);

        public string EntryPoint
        {
            get
            {
                return GetChildByRole(EntryPointRole).Name;
            }
            set
            {
                SetChildByRole(EntryPointRole, Identifier.Create(value, TextLocation.Empty));
            }
        }

        public string Library
        {
            get
            {
                return GetChildByRole(LibraryRole).Name;
            }
            set
            {
                SetChildByRole(LibraryRole, Identifier.Create(value, TextLocation.Empty));
            }
        }

        public override S AcceptVisitor<T, S>(IAstVisitor<T, S> visitor, T data = default(T))
        {
            return visitor.VisitExternMethodDeclaration(this, data);
        }

        protected internal override bool DoMatch(AstNode other, PatternMatching.Match match)
        {
            ExternMethodDeclaration o = other as ExternMethodDeclaration;
            return o != null && this.MatchMember(o, match) && this.TypeParameters.DoMatch(o.TypeParameters, match)
                && this.Parameters.DoMatch(o.Parameters, match) /*&& this.Constraints.DoMatch(o.Constraints, match)*/
                && this.Body.DoMatch(o.Body, match);
        }
    }
}
