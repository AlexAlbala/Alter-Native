using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Depth1;

namespace RecursiveAssemblies
{
    class Program
    {
        static void Main(string[] args)
        {
            MyDepth1Class c = new MyDepth1Class();
            String message = c.Ping("Hello World");

            Console.WriteLine("Sending Hello World...");
            Console.WriteLine(message);
        }
    }
}
