using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Constraints
{
    public class Person { }
    public class A : Person { }
    public class B { public B() { } }
    public class C { private C() { } }

    public class Example<T> where T : Person { }
    public class Example2<T> where T : new() { }

    class Program
    {
        
        static void Main(string[] args)
        {
            Example<A> a;
            //Example<B> b;

            Example2<B> b2;
            //Example2<C> c;
        }
    }
}
