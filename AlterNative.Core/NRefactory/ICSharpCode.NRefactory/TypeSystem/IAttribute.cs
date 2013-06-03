﻿// Copyright (c) AlphaSierraPapa for the SharpDevelop Team
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
using System.Diagnostics.Contracts;

using ICSharpCode.NRefactory.Semantics;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Represents an unresolved attribute.
	/// </summary>
	#if WITH_CONTRACTS
	[ContractClass(typeof(IUnresolvedAttributeContract))]
	#endif
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
	public interface IUnresolvedAttribute
	{
		/// <summary>
		/// Gets the code region of this attribute.
		/// </summary>
		DomRegion Region { get; }
		
		//ITypeReference AttributeType { get; }
		
		/// <summary>
		/// Resolves the attribute.
		/// </summary>
		IAttribute CreateResolvedAttribute(ITypeResolveContext context);
	}
	
	/// <summary>
	/// Represents an attribute.
	/// </summary>
	#if WITH_CONTRACTS
	[ContractClass(typeof(IAttributeContract))]
	#endif
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
	public interface IAttribute
	{
		/// <summary>
		/// Gets the code region of this attribute.
		/// </summary>
		DomRegion Region { get; }
		
		/// <summary>
		/// Gets the type of the attribute.
		/// </summary>
		IType AttributeType { get; }
		
		/// <summary>
		/// Gets the constructor being used.
		/// This property may return null if no matching constructor was found.
		/// </summary>
		IMethod Constructor { get; }
		
		/// <summary>
		/// Gets the positional arguments.
		/// </summary>
		IList<ResolveResult> PositionalArguments { get; }
		
		/// <summary>
		/// Gets the named arguments passed to the attribute.
		/// </summary>
		IList<KeyValuePair<IMember, ResolveResult>> NamedArguments { get; }
	}
	
	#if WITH_CONTRACTS
	[ContractClassFor(typeof(IAttribute))]
	abstract class IAttributeContract : IFreezableContract, IAttribute
	{
		DomRegion IAttribute.Region {
			get { return DomRegion.Empty; }
		}
		
		ITypeReference IAttribute.AttributeType {
			get {
				Contract.Ensures(Contract.Result<ITypeReference>() != null);
				return null;
			}
		}
		
		IList<IConstantValue> IAttribute.GetPositionalArguments(ITypeResolveContext context)
		{
			Contract.Requires(context != null);
			Contract.Ensures(Contract.Result<IList<IConstantValue>>() != null);
			return null;
		}
		
		IList<KeyValuePair<string, IConstantValue>> IAttribute.GetNamedArguments(ITypeResolveContext context)
		{
			Contract.Requires(context != null);
			Contract.Ensures(Contract.Result<IList<KeyValuePair<string, IConstantValue>>>() != null);
			return null;
		}
		
		IMethod IAttribute.ResolveConstructor(ITypeResolveContext context)
		{
			Contract.Requires(context != null);
			return null;
		}
	}
	#endif
}
