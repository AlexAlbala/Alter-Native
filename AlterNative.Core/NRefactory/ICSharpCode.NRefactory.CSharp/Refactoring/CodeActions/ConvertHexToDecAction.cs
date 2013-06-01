﻿// 
// ConvertHexToDec.cs
//  
// Author:
//       Mike Krüger <mkrueger@novell.com>
// 
// Copyright (c) 2011 Mike Krüger <mkrueger@novell.com>
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
using System.Threading;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory.CSharp.Refactoring
{
	/// <summary>
	/// Convert a hex numer to dec. For example: 0x10 -> 16
	/// </summary>
	[ContextAction("Convert hex to dec.", Description = "Convert hex to dec.")]
	public class ConvertHexToDecAction : ICodeActionProvider
	{
		public IEnumerable<CodeAction> GetActions(RefactoringContext context)
		{
			var pexpr = context.GetNode<PrimitiveExpression>();
			if (pexpr == null || !pexpr.LiteralValue.StartsWith("0X", System.StringComparison.OrdinalIgnoreCase)) {
				yield break;
			}
			if (!((pexpr.Value is int) || (pexpr.Value is long) || (pexpr.Value is short) || (pexpr.Value is sbyte) ||
				(pexpr.Value is uint) || (pexpr.Value is ulong) || (pexpr.Value is ushort) || (pexpr.Value is byte))) {
				yield break;
			}
			
			yield return new CodeAction(context.TranslateString("Convert hex to dec"), script => {
				script.Replace(pexpr, new PrimitiveExpression (pexpr.Value));
			});
		}
	}
}
