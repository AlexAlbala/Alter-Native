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
using System.Collections.ObjectModel;
using ICSharpCode.NRefactory.CSharp.Resolver;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;
using ICSharpCode.NRefactory.Utils;

namespace ICSharpCode.NRefactory.CSharp.TypeSystem
{
	/// <summary>
	/// Reference to a qualified type or namespace name.
	/// </summary>
	[Serializable]
	public sealed class MemberTypeOrNamespaceReference : TypeOrNamespaceReference, ISupportsInterning
	{
		TypeOrNamespaceReference target;
		string identifier;
		IList<ITypeReference> typeArguments;
		
		public MemberTypeOrNamespaceReference(TypeOrNamespaceReference target, string identifier, IList<ITypeReference> typeArguments)
		{
			if (target == null)
				throw new ArgumentNullException("target");
			if (identifier == null)
				throw new ArgumentNullException("identifier");
			this.target = target;
			this.identifier = identifier;
			this.typeArguments = typeArguments ?? EmptyList<ITypeReference>.Instance;
		}
		
		public string Identifier {
			get { return identifier; }
		}
		
		public TypeOrNamespaceReference Target {
			get { return target; }
		}
		
		public IList<ITypeReference> TypeArguments {
			get { return new ReadOnlyCollection<ITypeReference>(typeArguments); }
		}
		
		/// <summary>
		/// Adds a suffix to the identifier.
		/// Does not modify the existing type reference, but returns a new one.
		/// </summary>
		public MemberTypeOrNamespaceReference AddSuffix(string suffix)
		{
			return new MemberTypeOrNamespaceReference(target, identifier + suffix, typeArguments);
		}
		
		public override ResolveResult Resolve(CSharpResolver resolver)
		{
			ResolveResult targetRR = target.Resolve(resolver);
			if (targetRR.IsError)
				return targetRR;
			IList<IType> typeArgs = typeArguments.Resolve(resolver.CurrentTypeResolveContext);
			using (var busyLock = BusyManager.Enter(this)) {
				if (busyLock.Success) {
					return resolver.ResolveMemberType(targetRR, identifier, typeArgs);
				} else {
					// This can happen for "class Test : $Test.Base$ { public class Base {} }":
					return ErrorResolveResult.UnknownError; // don't cache this error
				}
			}
		}
		
		public override string ToString()
		{
			if (typeArguments.Count == 0)
				return target.ToString() + "." + identifier;
			else
				return target.ToString() + "." + identifier + "<" + string.Join(",", typeArguments) + ">";
		}
		
		void ISupportsInterning.PrepareForInterning(IInterningProvider provider)
		{
			target = provider.Intern(target);
			identifier = provider.Intern(identifier);
			typeArguments = provider.InternList(typeArguments);
		}
		
		int ISupportsInterning.GetHashCodeForInterning()
		{
			int hashCode = 0;
			unchecked {
				hashCode += 1000000007 * target.GetHashCode();
				hashCode += 1000000033 * identifier.GetHashCode();
				hashCode += 1000000087 * typeArguments.GetHashCode();
			}
			return hashCode;
		}
		
		bool ISupportsInterning.EqualsForInterning(ISupportsInterning other)
		{
			MemberTypeOrNamespaceReference o = other as MemberTypeOrNamespaceReference;
			return o != null && this.target == o.target
				&& this.identifier == o.identifier && this.typeArguments == o.typeArguments;
		}
	}
}
