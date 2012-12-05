#include "BinaryTrees.h"
namespace BinaryTree {
	void BinaryTrees::Main(String* args[]){
		int i = 12;
		int maxDepth = Math::Max(6, i);
		int stretchDepth = maxDepth + 1;
		int check = TreeNode::bottomUpTree(0, stretchDepth)->itemCheck();
		Console::Write(new String("stretch tree of depth "));
		Console::Write(stretchDepth);
		Console::Write(new String("\t check: "));
		Console::WriteLine(check);
		TreeNode* longLivedTree = TreeNode::bottomUpTree(0, maxDepth);
		for (int depth = 4; depth <= maxDepth; depth += 2){
			int iterations = 1 << maxDepth - depth + 4;
			check = 0;
			for (int j = 1; j <= iterations; j += 1){
				check += TreeNode::bottomUpTree(j, depth)->itemCheck();
				check += TreeNode::bottomUpTree(-j, depth)->itemCheck();
			}
			Console::Write(iterations * 2);
			Console::Write(new String("\t trees of depth "));
			Console::Write(depth);
			Console::Write(new String("\t check: "));
			Console::WriteLine(check);
		}
		Console::Write(new String("long lived tree of depth "));
		Console::Write(maxDepth);
		Console::Write(new String("\t check: "));
		Console::WriteLine(longLivedTree->itemCheck());
	}
	BinaryTrees::BinaryTrees()
	{
		minDepth = 4;
	}

}
