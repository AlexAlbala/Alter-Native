#include "TreeNode.h"
namespace BinaryTree {
	TreeNode::TreeNode(int item){
		this->item = item;
		this->next = null;
	}
	TreeNode* TreeNode::bottomUpTree(int item, int depth)
	{
		TreeNode* result;
		if (depth > 0) {
			result = new TreeNode(TreeNode::bottomUpTree(2 * item - 1, depth - 1), TreeNode::bottomUpTree(2 * item, depth - 1), item);
		}
		else {
			result = new TreeNode(item);
		}
		return result;
	}
	TreeNode::TreeNode(TreeNode* left, TreeNode* right, int item)
	{
		this->next = new Next();
		this->next->left = left;
		this->next->right = right;
		this->item = item;
	}
	int TreeNode::itemCheck()
	{
		int result;
		if (this->next == null) {
			result = this->item;
		}
		else {
			result = this->item + this->next->left->itemCheck() - this->next->right->itemCheck();
		}
		return result;
	}

}
