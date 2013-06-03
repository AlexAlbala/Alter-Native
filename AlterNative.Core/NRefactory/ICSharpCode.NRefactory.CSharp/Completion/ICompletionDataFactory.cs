// 
// ICompletionDataFactory.cs
//  
// Author:
//       Mike Krüger <mkrueger@xamarin.com>
// 
// Copyright (c) 2011 Xamarin Inc. (http://xamarin.com)
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using System.Collections.Generic;
using ICSharpCode.NRefactory.TypeSystem;
using ICSharpCode.NRefactory.Completion;

namespace ICSharpCode.NRefactory.CSharp.Completion
{
	public interface ICompletionDataFactory
	{
		ICompletionData CreateEntityCompletionData (IUnresolvedEntity entity);
		ICompletionData CreateEntityCompletionData (IUnresolvedEntity entity, string text);
		ICompletionData CreateEntityCompletionData (IEntity entity);
		ICompletionData CreateEntityCompletionData (IEntity entity, string text);
		
		ICompletionData CreateTypeCompletionData (IType type, string shortType);
		ICompletionData CreateTypeCompletionData (IUnresolvedTypeDefinition type, string shortType);
		
		/// <summary>
		/// Creates a generic completion data.
		/// </summary>
		/// <returns>
		/// The title of the completion data
		/// </param>
		/// <param name='description'>
		/// The description of the literal.
		/// </param>
		/// <param name='insertText'>
		/// The insert text. If null, title is taken.
		/// </param>
		ICompletionData CreateLiteralCompletionData (string title, string description = null, string insertText = null);
		
		ICompletionData CreateNamespaceCompletionData (string name);
		
		ICompletionData CreateVariableCompletionData (IVariable variable);

		ICompletionData CreateVariableCompletionData (IUnresolvedTypeParameter parameter);
		
		ICompletionData CreateEventCreationCompletionData (string varName, IType delegateType, IEvent evt, string parameterDefinition, IUnresolvedMember currentMember, IUnresolvedTypeDefinition currentType);

		ICompletionData CreateNewOverrideCompletionData (int declarationBegin, IUnresolvedTypeDefinition type, IMember m);
		ICompletionData CreateNewPartialCompletionData (int declarationBegin, IUnresolvedTypeDefinition type, IUnresolvedMember m);
		
		IEnumerable<ICompletionData> CreateCodeTemplateCompletionData ();
		
		IEnumerable<ICompletionData> CreatePreProcessorDefinesCompletionData ();
	}
}
