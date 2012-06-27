using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace List
{
    class MyList
    {
        Node first;
        int length;

        public MyList()
        {
            first = null;
            length = 0;
        }

        public int Length()
        {
            return length;
        }

        public void Add(Node n)
        {
            n.next = first;
            first = n;
            length++;
        }

        public Node getElementAt(int index)
        {
            if (index >= length)
                return null;
            
                Node n = first;
                for (int i = 0; i < index; i++)
                    n = n.next;

                return n;            
        }

        public void BubbleSort()
        {
            bool sorted = false;

            while (!sorted)
            {
                sorted = true;
                for (int i = 0; i < length - 1; i++)
                {
                    Node n1 = getElementAt(i);
                    Node n2 = getElementAt(i + 1);

                    if (n1.value > n2.value)
                    {
                        Swap(i, i + 1);
                        sorted = false;                        
                    }
                }
            }
        }

        private void Swap(int pos1, int pos2)
        {
            Node n1 = getElementAt(pos1);
            Node n2 = getElementAt(pos2);

            n1.next = n2.next;
            n2.next = n1;

           if(pos1 > 0)
           {
               Node nant = getElementAt(pos1 - 1);
               nant.next = n2;
           }

            if (pos2 == 1 && pos1 == 0)
                first = n2;
        }


    }
}
