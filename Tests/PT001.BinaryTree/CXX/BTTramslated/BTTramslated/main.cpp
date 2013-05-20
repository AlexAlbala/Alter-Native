#include <iostream>

int max(int n1, int n2)
{
	if(n1 > n2) return n1;
	return n2;
}

class TreeNode
{
    class Next
   	{
	public: TreeNode* left;
			TreeNode* right;

			~Next()
			{
				delete right;
				delete left;
			}
    };
   	
private: Next* next;
private: int item;

public: TreeNode()
		 {
			 this->next = 0;
		 }
    TreeNode(int item){
        this->item = item;
        this->next = 0;
    }

public:
    static TreeNode* bottomUpTree(int item, int depth){
        if (depth>0){
        return new TreeNode(bottomUpTree(2*item-1, depth-1), bottomUpTree(2*item, depth-1), item);
        }
        else {
        return new TreeNode(item);
        }
    }

    TreeNode(TreeNode* left, TreeNode* right, int item) {
		/*if(this->next != (Next*)0){
			delete this->next->right;
			delete this->next->left;
			delete this->next;
		}*/
	   	this->next = new Next ();
        this->next->left = left;
        this->next->right = right;
        this->item = item;
    }

    int itemCheck(){
        // if necessary deallocate here
        if (next==0) return item;
        else return item + next->left->itemCheck() - next->right->itemCheck();
    }

	~TreeNode()
	{
		delete next;
	}
};


int main()
{
	const int minDepth = 4;
    int n = 14;
    //if (args.Length > 0) n = Int32.Parse(args[0]);

    int maxDepth = max(minDepth + 2, n);
    int stretchDepth = maxDepth + 1;

	TreeNode tn = *(TreeNode::bottomUpTree(0,stretchDepth));
    int check = tn.itemCheck();
	
	std::cout << "stretch tree of depth ";
	std::cout << stretchDepth;
	std::cout << "\t check: ";
	std::cout << check << std::endl;

    TreeNode* longLivedTree = TreeNode::bottomUpTree(0,maxDepth);

    for (int depth=minDepth; depth<=maxDepth; depth+=2){
        int iterations = 1 << (maxDepth - depth + minDepth);

        check = 0;
        for (int i=1; i<=iterations; i++)
        {
			TreeNode tni = *(TreeNode::bottomUpTree(i,depth));
			TreeNode tni_ = *(TreeNode::bottomUpTree(-i,depth));
			check += (tni).itemCheck();
			check += (tni_).itemCheck();
        }

		std::cout << iterations*2;
		std::cout << "\t trees of depth ";
		std::cout << depth;
		std::cout << "\t check: ";
		std::cout << check << std::endl;
    }
	//delete longLivedTree;

	std::cout << "long lived tree of depth ";
	std::cout << maxDepth;
	std::cout << "\t check: ";
	std::cout << longLivedTree->itemCheck() << std::endl;   
	delete longLivedTree;
}