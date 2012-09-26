using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExplicitGenericInterfaces
{
    public interface IA
    {
        void f();
    }

    public interface IB<T>
    {
        void f();
    }

    public interface IC<T>
    {
        T f();
    }

    class C<T> : IA, IB<T>, IC<T>
    {
        T value;
        void IA.f()
        {
            Console.WriteLine("a");
        }

        void IB<T>.f()
        {
            Console.WriteLine("b");
        }

        public T f()
        {
            Console.WriteLine("c");
            return value;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            C<int> c = new C<int>();

            IA a = c;
            a.f();

            IB<int> b = c;
            b.f();

            c.f();
        }
    }
}
