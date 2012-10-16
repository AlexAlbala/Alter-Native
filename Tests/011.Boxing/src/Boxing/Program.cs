using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Boxing
{
    class A
    {
        public void f(Object arg)
        {
            Console.WriteLine("I'm A");
            Console.WriteLine(arg);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Box();
            UnBox();
        }

        static void Box()
        {
            int i = 123;
            object o = i;  // implicit boxing

            i = 456;  // change the contents of i

            f(i);
            A a = new A();
            a.f(i);

            System.Console.WriteLine(i);
            System.Console.WriteLine(o);
        }

        static void UnBox()
        {
            object o = 123;
            int i = (int)o;

            o = 456;  // change the contents of i

            

            System.Console.WriteLine(i);
            System.Console.WriteLine(o);
        }

        static void f(Object arg)
        {
            Console.WriteLine("I'm P");
            Console.WriteLine(arg);
        }
    }
}
