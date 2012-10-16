using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NestedClasses
{
    public interface IA {
        void f();
    }

    public interface IB
    {
        void f();
    }

    public interface IC
    {
        int f();
    }

    class C : IA, IB, IC
    {
         void IA.f()
        {
            Console.WriteLine("a");
        }

        void IB.f()
        {
            Console.WriteLine("b");
        }

        public int f()
        {
            Console.WriteLine("c");
            return 0;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            C c = new C();

            IA a = c;
            a.f();

            IB b = c;
            b.f();

            c.f();
        }
    }
}
