using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            String a = "This text will be removed";
            String b = "Hello";
            String c = " ";
            String d = "World!";

            string[] array = new string[3];
            array[0] = b;
            array[1] = c;
            array[2] = d;
            a = String.Concat(array);
            Console.WriteLine(a);
        }
    }
}
