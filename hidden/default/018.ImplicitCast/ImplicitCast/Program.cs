using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImplicitCast
{
    class A
    {
        public virtual void f()
        {
            Console.WriteLine("I'm A");
        }
    }
    class B : A
    {
        public override void f()
        {
            Console.WriteLine("I'm B");
        }
    }
    class C : A
    {
        public override void f()
        {
            Console.WriteLine("I'm C");
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            //IMPLICIT CASTING
            A ea = new A();
            B eb = new B();
            C ec = new C();


            ea.f();
            ea = eb;
            ea.f();
            ea = ec;
            ea.f();
        }
    }
}
