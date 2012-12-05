#pragma once
#include "System/System.h"
#include "System/Math.h"
#include "TreeNode.h"
#include "System/Console.h"

using namespace System;
namespace BinaryTree {
	class BinaryTrees : public virtual Object
	{
		private:
			int minDepth;
		public:
			static void Main(String* args[]);
		public:
			BinaryTrees();
	};
}
