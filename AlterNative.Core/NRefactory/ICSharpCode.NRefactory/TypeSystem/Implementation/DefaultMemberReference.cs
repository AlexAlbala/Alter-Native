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
using System.Linq;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// References an entity by its type and name.
	/// This class can be used to refer to fields, events, and parameterless properties.
	/// </summary>
	[Serializable]
	public sealed class DefaultMemberReference : IMemberReference, ISupportsInterning
	{
		EntityType entityType;
		ITypeReference typeReference;
		string name;
		int typeParameterCount;
		IList<ITypeReference> parameterTypes;
		
		public DefaultMemberReference(EntityType entityType, ITypeReference typeReference, string name, int typeParameterCount = 0, IList<ITypeReference> parameterTypes = null)
		{
			if (typeReference == null)
				throw new ArgumentNullException("typeReference");
			if (name == null)
				throw new ArgumentNullException("name");
			if (typeParameterCount != 0 && entityType != EntityType.Method)
				throw new ArgumentException("Type parameter count > 0 is only supported for methods.");
			this.entityType = entityType;
			this.typeReference = typeReference;
			this.name = name;
			this.typeParameterCount = typeParameterCount;
			this.parameterTypes = parameterTypes ?? EmptyList<ITypeReference>.Instance;
		}
		
		public IMember Resolve(ITypeResolveContext context)
		{
			IType type = typeReference.Resolve(context);
			IEnumerable<IMember> members;
			if (entityType == EntityType.Method) {
				members = type.GetMethods(
					m => m.Name == name && m.EntityType == EntityType.Method && m.TypeParameters.Count == typeParameterCount,
					GetMemberOptions.IgnoreInheritedMembers);
			} else {
				members = type.GetMembers(
					m => m.Name == name && m.EntityType == entityType,
					GetMemberOptions.IgnoreInheritedMembers);
			}
			var resolvedParameterTypes = parameterTypes.Resolve(context);
			foreach (IMember member in members) {
				IParameterizedMember parameterizedMember = member as IParameterizedMember;
				if (parameterTypes.Count == 0) {
					if (parameterizedMember == null || parameterizedMember.Parameters.Count == 0)
						return member;
				} else if (parameterTypes.Count == parameterizedMember.Parameters.Count) {
					bool signatureMatches = true;
					for (int i = 0; i < parameterTypes.Count; i++) {
						IType type1 = ParameterListComparer.Instance.NormalizeMethodTypeParameters(resolvedParameterTypes[i]);
						IType type2 = ParameterListComparer.Instance.NormalizeMethodTypeParameters(parameterizedMember.Parameters[i].Type);
						if (!type1.Equals(type2)) {
							signatureMatches = false;
							break;
						}
					}
					if (signatureMatches)
						return member;
				}
			}
			return null;
		}
		
		void ISupportsInterning.PrepareForInterning(IInterningProvider provider)
		{
			typeReference = provider.Intern(typeReference);
			name = provider.Intern(name);
			parameterTypes = provider.InternList(parameterTypes);
		}
		
		int ISupportsInterning.GetHashCodeForInterning()
		{
			return (int)entityType ^ typeReference.GetHashCode() ^ name.GetHashCode() ^ parameterTypes.GetHashCode();
		}
		
		bool ISupportsInterning.EqualsForInterning(ISupportsInterning other)
		{
			DefaultMemberReference o = other as DefaultMemberReference;
			return o != null && entityType == o.entityType && typeReference == o.typeReference && name == o.name && parameterTypes == o.parameterTypes;
		}
	}
}
