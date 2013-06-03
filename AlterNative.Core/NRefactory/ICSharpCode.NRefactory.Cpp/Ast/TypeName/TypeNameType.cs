// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under MIT X11 license (for details please see \doc\license.txt)

using System;
using System.Linq;
using System.Text;

namespace ICSharpCode.NRefactory.Cpp.Ast
{
    /// <summary>
    /// Description of TypeNameType.
    /// </summary>
    public class TypeNameType : AstType
    {
        public static readonly Role<AstType> TargetRole = new Role<AstType>("Target", AstType.Null);

        public AstType Target
        {
            get { return GetChildByRole(TargetRole); }
            set { SetChildByRole(TargetRole, value); }
        }

        public TypeNameType(AstType target)
        {
            Target = target;
        }

        public AstNodeCollection<AstType> TypeArguments
        {
            get { return GetChildrenByRole(Roles.TypeArgument); }
        }

        public override S AcceptVisitor<T, S>(IAstVisitor<T, S> visitor, T data)
        {
            return visitor.VisitTypeNameType(this, data);
        }

        protected internal override bool DoMatch(AstNode other, PatternMatching.Match match)
        {
            var o = other as QualifiedType;
            return o != null && this.Target.DoMatch(o.Target, match);
        }

        //public override string ToString()
        //{
        //    StringBuilder b = new StringBuilder();
        //    b.Append(this.Target);
        //    b.Append('.');
        //    b.Append(this.Name);
        //    if (this.TypeArguments.Any())
        //    {
        //        b.Append('(');
        //        b.Append(string.Join(", ", this.TypeArguments));
        //        b.Append(')');
        //    }
        //    return b.ToString();
        //}
    }
}
