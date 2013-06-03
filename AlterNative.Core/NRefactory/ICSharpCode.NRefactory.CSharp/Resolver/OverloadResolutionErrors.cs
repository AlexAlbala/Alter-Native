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

namespace ICSharpCode.NRefactory.CSharp.Resolver
{
	[Flags]
	public enum OverloadResolutionErrors
	{
		None = 0,
		/// <summary>
		/// Too many positional arguments (some could not be mapped to any parameter).
		/// </summary>
		TooManyPositionalArguments = 0x0001,
		/// <summary>
		/// A named argument could not be mapped to any parameter
		/// </summary>
		NoParameterFoundForNamedArgument = 0x0002,
		/// <summary>
		/// Type inference failed for a generic method.
		/// </summary>
		TypeInferenceFailed = 0x0004,
		/// <summary>
		/// Type arguments were explicitly specified, but did not match the number of type parameters.
		/// </summary>
		WrongNumberOfTypeArguments = 0x0008,
		/// <summary>
		/// After substituting type parameters with the inferred types; a constructed type within the formal parameters
		/// does not satisfy its constraint.
		/// </summary>
		ConstructedTypeDoesNotSatisfyConstraint = 0x0010,
		/// <summary>
		/// No argument was mapped to a non-optional parameter
		/// </summary>
		MissingArgumentForRequiredParameter = 0x0020,
		/// <summary>
		/// Several arguments were mapped to a single (non-params-array) parameter
		/// </summary>
		MultipleArgumentsForSingleParameter = 0x0040,
		/// <summary>
		/// 'ref'/'out' passing mode doesn't match for at least 1 parameter
		/// </summary>
		ParameterPassingModeMismatch = 0x0080,
		/// <summary>
		/// Argument type cannot be converted to parameter type
		/// </summary>
		ArgumentTypeMismatch = 0x0100,
		/// <summary>
		/// There is no unique best overload.
		/// This error does not apply to any single candidate, but only to the overall result of overload resolution.
		/// </summary>
		AmbiguousMatch = 0x0200,
		/// <summary>
		/// The member is not accessible.
		/// </summary>
		Inaccessible = 0x0400
	}
}
