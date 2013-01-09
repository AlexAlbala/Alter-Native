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
using ICSharpCode.NRefactory.TypeSystem;

namespace ICSharpCode.NRefactory.Semantics
{
	/// <summary>
	/// Resolve result for a C# 'is' expression.
	/// "Input is TargetType".
	/// </summary>
	public class TypeIsResolveResult : ResolveResult
	{
		public readonly ResolveResult Input;
		/// <summary>
		/// Type that is being compared with.
		/// </summary>
		public readonly IType TargetType;
		
		public TypeIsResolveResult(ResolveResult input, IType targetType, IType booleanType)
			: base(booleanType)
		{
			if (input == null)
				throw new ArgumentNullException("input");
			if (targetType == null)
				throw new ArgumentNullException("targetType");
			this.Input = input;
			this.TargetType = targetType;
		}
	}
}
