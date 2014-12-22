using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace List
{
    public class Node
    {
        public Node next;
        public int value;

        public Node()
        {
            double f = Utils.random.NextDouble();
            this.value = (int)(f * 1000.0);
        }
    }
}
