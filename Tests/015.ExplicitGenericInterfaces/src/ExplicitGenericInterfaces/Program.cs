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

    class C<T> : IA, IB<T>, IC<T> where T : new()
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

    class A
    {
    }

    class Program
    {
        static void Main(string[] args)
        {
            C<A> c = new C<A>();

            IA a = c;
            a.f();

            IB<A> b = c;
            b.f();

            c.f();
        }
    }
}
