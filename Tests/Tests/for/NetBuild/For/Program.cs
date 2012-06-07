using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace For
{
    class Program
    {
        static void Main(string[] args)
        {
            int size = 50;
            int[] value = new int[size];

            for (int i = 0; i < size; i++)
            {
                value[i] = i;
            }
        }
    }
}
