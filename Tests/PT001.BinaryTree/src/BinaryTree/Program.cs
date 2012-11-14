/* The Computer Language Benchmarks Game
   http://shootout.alioth.debian.org/ 

   contributed by Marek Safar  
*/

using System;

namespace BinaryTree
{
    public class BinaryTrees
    {
        const int minDepth = 4;

        public static void Main(String[] args)
        {
            int n = 12;//DEPTH HARDCPDED !
            // if (args.Length > 0) n = Int32.Parse(args[0]);

            int maxDepth = Math.Max(minDepth + 2, n);
            int stretchDepth = maxDepth + 1;

            int check = (TreeNode.bottomUpTree(0, stretchDepth)).itemCheck();
            Console.Write("stretch tree of depth ");
            Console.Write(stretchDepth);
            Console.Write("\t check: ");
            Console.WriteLine(check);

            TreeNode longLivedTree = TreeNode.bottomUpTree(0, maxDepth);

            for (int depth = minDepth; depth <= maxDepth; depth += 2)
            {
                int iterations = 1 << (maxDepth - depth + minDepth);

                check = 0;
                for (int i = 1; i <= iterations; i++)
                {
                    check += (TreeNode.bottomUpTree(i, depth)).itemCheck();
                    check += (TreeNode.bottomUpTree(-i, depth)).itemCheck();
                }


                Console.Write(iterations * 2);
                Console.Write("\t trees of depth ");
                Console.Write(depth);
                Console.Write("\t check: ");
                Console.WriteLine(check);
            }

            Console.Write("long lived tree of depth ");
            Console.Write(maxDepth);
            Console.Write("\t check: ");
            Console.WriteLine(longLivedTree.itemCheck());
        }
    }

    public class Next
    {
        public TreeNode left, right;
    }

    public class TreeNode
    {

        private Next next;
        private int item;

        TreeNode(int item)
        {
            this.item = item;
            this.next = null;
        }

        public static TreeNode bottomUpTree(int item, int depth)
        {
            if (depth > 0)
            {
                return new TreeNode(
                     bottomUpTree(2 * item - 1, depth - 1)
                   , bottomUpTree(2 * item, depth - 1)
                   , item
                   );
            }
            else
            {
                return new TreeNode(item);
            }
        }

        TreeNode(TreeNode left, TreeNode right, int item)
        {
            this.next = new Next();
            this.next.left = left;
            this.next.right = right;
            this.item = item;
        }

        public int itemCheck()
        {
            // if necessary deallocate here
            if (next == null) return item;
            else return item + next.left.itemCheck() - next.right.itemCheck();
        }

    }
}