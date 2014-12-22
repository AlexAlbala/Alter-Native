using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polymorphism
{
    class Base
    {
        public virtual void Virtual()
        {
            Console.WriteLine("Base::Virtual()");
        }

        public void NonVirtual()
        {
            Console.WriteLine("Base::NonVirtual()");
        }
    }

    class Child1 : Base
    {
        public override void Virtual()
        {
            Console.WriteLine("Child1::Virtual()");
        }

        public new void NonVirtual()
        {
            Console.WriteLine("Child1::NonVirtual()");
        }
    }

    class Child2 : Base
    {

    }

    public class Program
    {
        static void Main(string[] args)
        {
            Base b1 = new Base();
            Child1 c1 = new Child1();
            Child2 c2 = new Child2();

            Base b2 = c1;
            Base b3 = c2;

            b1.Virtual();
            b1.NonVirtual();

            b2.Virtual();
            b2.NonVirtual();

            b3.Virtual();
            b3.NonVirtual();

            c1.Virtual();
            c1.NonVirtual();

            c2.Virtual();
            c2.NonVirtual();

        }
    }
}