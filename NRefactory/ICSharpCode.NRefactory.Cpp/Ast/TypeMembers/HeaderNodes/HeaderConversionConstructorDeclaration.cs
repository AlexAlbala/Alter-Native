using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.NRefactory.Cpp.Ast;

namespace ICSharpCode.NRefactory.Cpp
{
    public class HeaderConversionConstructorDeclaration : AttributedNode
    {
        public AstType ReturnType
        {
            get { return GetChildByRole(Roles.Type); }
            set { SetChildByRole(Roles.Type, value); }
        }

        public CppTokenNode LParToken
        {
            get { return GetChildByRole(Roles.LPar); }
        }

        public CppTokenNode RParToken
        {
            get { return GetChildByRole(Roles.RPar); }
        }

        /// <summary>
        /// Gets the method name for the operator type. ("op_Addition", "op_Implicit", etc.)
        /// </summary>
        public static string GetName(OperatorType type)
        {
            return Mono.CSharp.Operator.GetMetadataName((Mono.CSharp.Operator.OpType)type);
        }

        /// <summary>
        /// Gets the token for the operator type ("+", "implicit", etc.)
        /// </summary>
        public static string GetToken(OperatorType type)
        {
            return Mono.CSharp.Operator.GetName((Mono.CSharp.Operator.OpType)type);
        }

        //public override NodeType NodeType {
        //    get { return NodeType.Member; }
        //}

        public override S AcceptVisitor<T, S>(IAstVisitor<T, S> visitor, T data = default(T))
        {
            return visitor.VisitHeaderConversionConstructorDeclaration(this, data);
        }

        protected internal override bool DoMatch(AstNode other, PatternMatching.Match match)
        {
            OperatorDeclaration o = other as OperatorDeclaration;
            return o != null && this.MatchAttributesAndModifiers(o, match)
                && this.ReturnType.DoMatch(o.ReturnType, match);
        }
    }
}
