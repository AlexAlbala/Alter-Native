#pragma once
#include "System/System.h"
#include "TreeNode.h"

using namespace System;
namespace BinaryTree {
	//Forward Declaration
	class TreeNode;

	class Next : public virtual Object{
		public:
			TreeNode* left;
		public:
			TreeNode* right;
	};
}
