using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ForwardDeclaration
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            p.Run();
        }

        public void Run()
        {
            ClassA a = new ClassA();
            string hello = a.SayHello();
            Console.WriteLine(hello);
        }
    }
}
