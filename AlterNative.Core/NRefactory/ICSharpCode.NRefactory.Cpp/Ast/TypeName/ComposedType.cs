// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under MIT X11 license (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ICSharpCode.NRefactory.Cpp.Ast
{
    public class ComposedType : AstType
    {
        public static readonly Role<CppTokenNode> NullableRole = new Role<CppTokenNode>("Nullable", CppTokenNode.Null);
        public static readonly Role<CppTokenNode> PointerRole = new Role<CppTokenNode>("Pointer", CppTokenNode.Null);

        public AstType BaseType
        {
            get { return GetChildByRole(Roles.Type); }
            set { SetChildByRole(Roles.Type, value); }
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

        public bool HasNullableSpecifier
        {
            get
            {
                return !GetChildByRole(NullableRole).IsNull;
            }
            set
            {
                SetChildByRole(NullableRole, value ? new CppTokenNode(TextLocation.Empty, 1) : null);
            }
        }       

        public override S AcceptVisitor<T, S>(IAstVisitor<T, S> visitor, T data)
        {
            return visitor.VisitComposedType(this, data);
        }

        protected internal override bool DoMatch(AstNode other, PatternMatching.Match match)
        {
            ComposedType o = other as ComposedType;
            return o != null && this.HasNullableSpecifier == o.HasNullableSpecifier;
        }

        public override string ToString()
        {
            StringBuilder b = new StringBuilder();
            b.Append(this.BaseType.ToString());
            if (this.HasNullableSpecifier)
                b.Append('?');
            //foreach (var arraySpecifier in this.ArraySpecifiers)
            //{
            //    b.Append('(');
            //    b.Append(',', arraySpecifier.Dimensions - 1);
            //    b.Append(')');
            //}
            return b.ToString();
        }        
    }

    /// <summary>
    /// [,,,]
    /// </summary>
    public class ArraySpecifier : AstNode
    {
        public ArraySpecifier()
        {
        }

        public ArraySpecifier(int dimensions)
        {
            this.Dimensions = dimensions;
        }

        public CppTokenNode LParToken
        {
            get { return GetChildByRole(Roles.LPar); }
        }

        public int Dimensions
        {
            get { return 1 + GetChildrenByRole(Roles.Comma).Count(); }
            set
            {
                int d = this.Dimensions;
                while (d > value)
                {
                    GetChildByRole(Roles.Comma).Remove();
                    d--;
                }
                while (d < value)
                {
                    InsertChildBefore(GetChildByRole(Roles.Comma), new CppTokenNode(TextLocation.Empty, 1), Roles.Comma);
                    d++;
                }
            }
        }

        public CppTokenNode RParToken
        {
            get { return GetChildByRole(Roles.LPar); }
        }

        public override S AcceptVisitor<T, S>(IAstVisitor<T, S> visitor, T data)
        {
            return visitor.VisitArraySpecifier(this, data);
        }

        protected internal override bool DoMatch(AstNode other, PatternMatching.Match match)
        {
            ArraySpecifier o = other as ArraySpecifier;
            return o != null && this.Dimensions == o.Dimensions;
        }

        public override string ToString()
        {
            return "(" + new string(',', this.Dimensions - 1) + ")";
        }

        //public override NodeType NodeType
        //{
        //    get { return this.NodeType; }
        //}
    }
}

