using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dictionary
{
    class A 
    {
        void f()
        {
            Console.WriteLine("Hello I'm A");
        }
    }
    class B
    {
        public void f()
        {
            Console.WriteLine("Hello I'm B");
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            p.Run();
        }

        void Run()
        {
            Dictionary<int, int> myDict = new Dictionary<int, int>();
            myDict.Add(0, 1);
            myDict.Add(1, 2);
            myDict.Add(2, 3);
            myDict.Add(3, 4);

            Console.WriteLine("Now it should print false");
            Console.WriteLine(myDict.ContainsKey(4));

            Console.WriteLine("Now it should print true");
            Console.WriteLine(myDict.ContainsKey(3));
            myDict.Remove(3);

            Console.WriteLine("Now it should print false");
            Console.WriteLine(myDict.ContainsKey(3));

            Console.WriteLine("Now it should print 2");
            Console.WriteLine(myDict[1]);

            Dictionary<A, B> myDict2 = new Dictionary<A, B>();
            A a1 = new A();
            A a2 = new A();

            myDict2.Add(a1, new B());
            myDict2.Add(a2, new B());           
            

            Console.WriteLine("Now it should print false");
            Console.WriteLine(myDict2.ContainsKey(new A()));

            Console.WriteLine("Now it should print true");
            Console.WriteLine(myDict2.ContainsKey(a1));
            myDict2.Remove(a1);

            Console.WriteLine("Now it should print false");
            Console.WriteLine(myDict2.ContainsKey(a1));

            Console.WriteLine("Now it should print \"Hello I'm B\"");
            myDict2[a2].f();
        }
    }
}
