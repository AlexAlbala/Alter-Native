using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arrays
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();

            int[] arr = new int[15];

            for (int i = 0; i < arr.Length; i++)
                arr[i] = i;

            p.Test1(arr);
        }

        public void Test1(int[] myArray)
        {
            long[] arr = new long[myArray.Length];
            int pos = 0;
            foreach (int i in myArray)
                arr[pos++] = i + 50;

            this.Test2(myArray, arr);
        }

        public void Test2(int[] myArray1, long[] myArray2)
        {
            string s = "This is a char array message";
            char[] message = s.ToCharArray();

            this.Finish(myArray1, myArray2, message);
        }

        public void Finish(int[] myArray1, long[] myArray2, char[] myArray3)
        {
            for (int i = 0; i < myArray1.Length; i++)
            {
                Console.WriteLine(myArray1[i]);
                Console.WriteLine(myArray2[i]);
            }

            Console.WriteLine(myArray3);
        }
    }
}
