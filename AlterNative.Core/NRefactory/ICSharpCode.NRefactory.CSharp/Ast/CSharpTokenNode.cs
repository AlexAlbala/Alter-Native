﻿// 
// TokenNode.cs
//  
// Author:
//       Mike Krüger <mkrueger@novell.com>
// 
// Copyright (c) 2010 Novell, Inc (http://www.novell.com)
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

namespace ICSharpCode.NRefactory.CSharp
{
	/// <summary>
	/// Represents a token in C#. Note that the type of the token is defined through the TokenRole.
	/// </summary>
	/// <remarks>
	/// In all non null c# token nodes the Role of a CSharpToken must be a TokenRole.
	/// </remarks>
	public class CSharpTokenNode : AstNode
	{
		public static new readonly CSharpTokenNode Null = new NullCSharpTokenNode ();
		class NullCSharpTokenNode : CSharpTokenNode
		{
			public override bool IsNull {
				get {
					return true;
				}
			}
			
			public NullCSharpTokenNode () : base (TextLocation.Empty)
			{
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
		}
		
		public override NodeType NodeType {
			get {
				return NodeType.Token;
			}
		}
		
		TextLocation startLocation;
		public override TextLocation StartLocation {
			get {
				return startLocation;
			}
		}
		
		protected virtual int TokenLength {
			get {
				if (!(Role is TokenRole))
					return 0;
				return ((TokenRole)Role).Length;
			}
		}
		
		public override TextLocation EndLocation {
			get {
				return new TextLocation (StartLocation.Line, StartLocation.Column + TokenLength);
			}
		}
		
		public CSharpTokenNode (TextLocation location)
		{
			this.startLocation = location;
		}
		
		public override string GetText (CSharpFormattingOptions formattingOptions = null)
		{
			if (!(Role is TokenRole))
				return null;
			return ((TokenRole)Role).Token;
		}

		public override void AcceptVisitor (IAstVisitor visitor)
		{
			visitor.VisitCSharpTokenNode (this);
		}
			
		public override T AcceptVisitor<T> (IAstVisitor<T> visitor)
		{
			return visitor.VisitCSharpTokenNode (this);
		}
		
		public override S AcceptVisitor<T, S> (IAstVisitor<T, S> visitor, T data)
		{
			return visitor.VisitCSharpTokenNode (this, data);
		}
		
		protected internal override bool DoMatch(AstNode other, PatternMatching.Match match)
		{
			CSharpTokenNode o = other as CSharpTokenNode;
			return o != null && !o.IsNull && !(o is CSharpModifierToken);
		}
		
		public override string ToString ()
		{
			return string.Format ("[CSharpTokenNode: StartLocation={0}, EndLocation={1}, Role={2}]", StartLocation, EndLocation, Role);
		}
	}
}

