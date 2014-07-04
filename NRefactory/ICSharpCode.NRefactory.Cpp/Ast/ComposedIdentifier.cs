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
        public static readonly Role<CppTokenNode> PointerRole = new Role<CppTokenNode>("PointerRole");

        public ComposedIdentifier(string name, TextLocation location) : base(name, location) { }

        public AstNodeCollection<ArraySpecifier> ArraySpecifiers
        {
            get { return GetChildrenByRole(ArraySpecifierRole); }
        }

        public int PointerRank
        {
            get
            {
                return GetChildrenByRole(PointerRole).Count;
            }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException();
                int d = this.PointerRank;
                while (d > value)
                {
                    GetChildByRole(PointerRole).Remove();
                    d--;
                }
                while (d < value)
                {
                    InsertChildBefore(GetChildByRole(PointerRole), new CppTokenNode(TextLocation.Empty, 1), PointerRole);
                    d++;
                }
            }
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
