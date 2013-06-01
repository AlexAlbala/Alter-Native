using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.NRefactory.Cpp.Ast;

namespace ICSharpCode.NRefactory.Cpp
{
    public class ComposedIdentifier : Identifier
    {
        public static readonly Role<ArraySpecifier> ArraySpecifierRole = new Role<ArraySpecifier>("ArraySpecifier");

        public ComposedIdentifier(string name, TextLocation location) : base(name, location) { }

        public AstNodeCollection<ArraySpecifier> ArraySpecifiers
        {
            get { return GetChildrenByRole(ArraySpecifierRole); }
        }

        public Identifier BaseIdentifier
        {
            get { return GetChildByRole(Roles.Identifier); }
            set { SetChildByRole(Roles.Identifier, value); }
        }

        public override S AcceptVisitor<T, S>(IAstVisitor<T, S> visitor, T data)
        {
            return visitor.VisitComposedIdentifier(this, data);
        }

        protected internal override bool DoMatch(AstNode other, PatternMatching.Match match)
        {
            ComposedType o = other as ComposedType;
            return o != null;
        }
    }
}
