// Copyright (c) AlphaSierraPapa for the SharpDevelop Team
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using ICSharpCode.NRefactory.CSharp.Resolver;
using ICSharpCode.NRefactory.TypeSystem;

namespace ICSharpCode.NRefactory.CSharp
{
	/// <summary>
	/// A type reference in the C# AST.
	/// </summary>
	public abstract class AstType : AstNode
	{
		#region Null
		public new static readonly AstType Null = new NullAstType ();
		
		sealed class NullAstType : AstType
		{
			public override bool IsNull {
				get {
					return true;
				}
			}
			
			public override void AcceptVisitor (IAstVisitor visitor)
			{
			}
			
			public override T AcceptVisitor<T> (IAstVisitor<T> visitor)
			{
				return default (T);
			}
			
			public override S AcceptVisitor<T, S> (IAstVisitor<T, S> visitor, T data)
			{
				return default (S);
			}
			
			protected internal override bool DoMatch(AstNode other, PatternMatching.Match match)
			{
				return other == null || other.IsNull;
			}
			
			public override ITypeReference ToTypeReference(SimpleNameLookupMode lookupMode)
			{
				return SpecialType.UnknownType;
			}
		}
		#endregion
		
		#region PatternPlaceholder
		public static implicit operator AstType(PatternMatching.Pattern pattern)
		{
			return pattern != null ? new PatternPlaceholder(pattern) : null;
		}
		
		sealed class PatternPlaceholder : AstType, PatternMatching.INode
		{
			readonly PatternMatching.Pattern child;
			
			public PatternPlaceholder(PatternMatching.Pattern child)
			{
				this.child = child;
			}
			
			public override NodeType NodeType {
				get { return NodeType.Pattern; }
			}
			
			public override void AcceptVisitor (IAstVisitor visitor)
			{
				visitor.VisitPatternPlaceholder (this, child);
			}
			
			public override T AcceptVisitor<T> (IAstVisitor<T> visitor)
			{
				return visitor.VisitPatternPlaceholder (this, child);
			}
			
			public override S AcceptVisitor<T, S>(IAstVisitor<T, S> visitor, T data)
			{
				return visitor.VisitPatternPlaceholder (this, child, data);
			}
			
			public override ITypeReference ToTypeReference(SimpleNameLookupMode lookupMode)
			{
				throw new NotSupportedException();
			}
			
			protected internal override bool DoMatch(AstNode other, PatternMatching.Match match)
			{
				return child.DoMatch(other, match);
			}
			
			bool PatternMatching.INode.DoMatchCollection(Role role, PatternMatching.INode pos, PatternMatching.Match match, PatternMatching.BacktrackingInfo backtrackingInfo)
			{
				return child.DoMatchCollection(role, pos, match, backtrackingInfo);
			}
		}
		#endregion
		
		public override NodeType NodeType {
			get { return NodeType.TypeReference; }
		}
		
		public new AstType Clone()
		{
			return (AstType)base.Clone();
		}
		
		/// <summary>
		/// Create an ITypeReference for this AstType.
		/// </summary>
		/// <remarks>
		/// The resulting type reference will read the context information from the
		/// <see cref="ITypeResolveContext"/>:
		/// For resolving type parameters, the CurrentTypeDefinition/CurrentMember is used.
		/// For resolving simple names, the current namespace and usings from the CurrentUsingScope
		/// (on CSharpTypeResolveContext only) is used.
		/// </remarks>
		public abstract ITypeReference ToTypeReference(SimpleNameLookupMode lookupMode = SimpleNameLookupMode.Type);
		
		/// <summary>
		/// Creates a pointer type from this type by nesting it in a <see cref="ComposedType"/>.
		/// If this type already is a pointer type, this method just increases the PointerRank of the existing pointer type.
		/// </summary>
		public virtual AstType MakePointerType()
		{
			return new ComposedType { BaseType = this }.MakePointerType();
		}
		
		/// <summary>
		/// Creates an array type from this type by nesting it in a <see cref="ComposedType"/>.
		/// If this type already is an array type, the additional rank is prepended to the existing array specifier list.
		/// Thus, <c>new SimpleType("T").MakeArrayType(1).MakeArrayType(2)</c> will result in "T[,][]".
		/// </summary>
		public virtual AstType MakeArrayType(int rank = 1)
		{
			return new ComposedType { BaseType = this }.MakeArrayType(rank);
		}
		
		/// <summary>
		/// Creates a nullable type from this type by nesting it in a <see cref="ComposedType"/>.
		/// </summary>
		public AstType MakeNullableType()
		{
			return new ComposedType { BaseType = this, HasNullableSpecifier = true };
		}
		
		/// <summary>
		/// Builds an expression that can be used to access a static member on this type.
		/// </summary>
		public MemberReferenceExpression Member(string memberName)
		{
			return new TypeReferenceExpression { Type = this }.Member(memberName);
		}
		
		/// <summary>
		/// Builds an expression that can be used to access a static member on this type.
		/// </summary>
		public MemberType MemberType(string memberName, params AstType[] typeArguments)
		{
			var memberType = new MemberType(this, memberName);
			memberType.TypeArguments.AddRange(typeArguments);
			return memberType;
		}
		
		/// <summary>
		/// Builds an expression that can be used to access a static member on this type.
		/// </summary>
		public MemberType MemberType(string memberName, IEnumerable<AstType> typeArguments)
		{
			var memberType = new MemberType(this, memberName);
			memberType.TypeArguments.AddRange(typeArguments);
			return memberType;
		}
		
		/// <summary>
		/// Builds an invocation expression using this type as target.
		/// </summary>
		public InvocationExpression Invoke(string methodName, IEnumerable<Expression> arguments)
		{
			return new TypeReferenceExpression { Type = this }.Invoke(methodName, arguments);
		}
		
		/// <summary>
		/// Builds an invocation expression using this type as target.
		/// </summary>
		public InvocationExpression Invoke(string methodName, params Expression[] arguments)
		{
			return new TypeReferenceExpression { Type = this }.Invoke(methodName, arguments);
		}
		
		/// <summary>
		/// Builds an invocation expression using this type as target.
		/// </summary>
		public InvocationExpression Invoke(string methodName, IEnumerable<AstType> typeArguments, IEnumerable<Expression> arguments)
		{
			return new TypeReferenceExpression { Type = this }.Invoke(methodName, typeArguments, arguments);
		}
	}
}
