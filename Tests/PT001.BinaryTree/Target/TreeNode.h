#pragma once
#include "System/System.h"
#include "Next.h"

using namespace System;
namespace BinaryTree {
	//Forward Declaration
	class Next;

	class TreeNode : public virtual Object{
		private:
			Next* next;
		private:
			int item;
		private:
			TreeNode(int item);
		public:
		static TreeNode* bottomUpTree(int item, int depth);
		private:
			TreeNode(TreeNode* left, TreeNode* right, int item);
		public:
			int itemCheck();
	};
}
