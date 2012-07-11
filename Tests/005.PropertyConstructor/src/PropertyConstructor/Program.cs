using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PropertyConstructor
{
    class A
    {
        private int p;

        public int P
        {
            set { this.p = value; }
            get { return p; }
        }

        static void Main(String[] args) {
            Main1();
            Main2();
        }
        
        static void Main1()
        {
            
            A a = new A();
            a.P = 3;
            Console.WriteLine(a.P);
        }

        static void Main2()
        {
            A a = new A { P = 3 };
            Console.WriteLine(a.P);
        }

    }
}