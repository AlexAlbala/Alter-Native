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
using System.Diagnostics;

namespace ICSharpCode.NRefactory.PatternMatching
{
	public class OptionalNode : Pattern
	{
		readonly INode childNode;
		
		public INode ChildNode {
			get { return childNode; }
		}
		
		public OptionalNode(INode childNode)
		{
			if (childNode == null)
				throw new ArgumentNullException("childNode");
			this.childNode = childNode;
		}
		
		public OptionalNode(string groupName, INode childNode) : this(new NamedNode(groupName, childNode))
		{
		}
		
		public override bool DoMatchCollection(Role role, INode pos, Match match, BacktrackingInfo backtrackingInfo)
		{
			backtrackingInfo.backtrackingStack.Push(new PossibleMatch(pos, match.CheckPoint()));
			return childNode.DoMatch(pos, match);
		}
		
		public override bool DoMatch(INode other, Match match)
		{
			if (other == null || other.IsNull)
				return true;
			else
				return childNode.DoMatch(other, match);
		}
	}
}
