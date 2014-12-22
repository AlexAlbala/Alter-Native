using System;
using System.Linq;
using System.Text;

namespace Covariance
{
    class Program
    {
        static void Main(string[] args)
        {
            A<C> a = new A<C>();
            CovIEnumerator<C> b = a.Get();

            A<int> a2 = new A<int>();
            CovIEnumerator<int> b2 = a2.Get();
        }
    }

    class C
    {
    }

    interface CovIEnumerable<T>
    {
        CovIEnumerator<T> Get();
    }

    interface CovIEnumerator<T>
    {
    }

    class B<T> : CovIEnumerator<T>
    {
        public B()
        {
            Console.WriteLine("Building B...");
        }
    }

    class A<T> : CovIEnumerable<T>
    {
        public CovIEnumerator<T> Get()
        {
            Console.WriteLine("A<T>.Get()");
            return new B<T>();
        }
    }

}
