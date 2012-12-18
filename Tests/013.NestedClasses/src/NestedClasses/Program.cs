using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NestedClasses
{
    class Program
    {
        static void Main(string[] args)
        {
            ParentClass p = new ParentClass();
            ParentClass.NestedClass1 n1 = new ParentClass.NestedClass1();
            ParentClass.NestedClass2 n2 = new ParentClass.NestedClass2();
            ParentClass.NestedClass2.NestedClass3 n3 = new ParentClass.NestedClass2.NestedClass3();

            p.f();
            n1.f();
            n2.f();
            n3.f();            
        }
    }

    public class ParentClass
    {
        public void f()
        {
            Console.WriteLine("ParentClass");
        }

        public class NestedClass1
        {
            public void f()
            {
                Console.WriteLine("NestedClass1");
            }
        }

        public class NestedClass2
        {
            public void f()
            {
                Console.WriteLine("NestedClass2");
            }

            public class NestedClass3
            {
                public void f()
                {
                    Console.WriteLine("NestedClass3");
                }
            }
        }
    }
}
