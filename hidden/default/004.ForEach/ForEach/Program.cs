using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace ForEach
{
    class A
    {
        public void f()
        {
            Console.WriteLine("I'm an object");
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            List<float> myList = new List<float>();
            myList.Add(5.6f);
            myList.Add(5.7f);
            myList.Add(5.2f);
            myList.Add(5.9f);
            myList.Add(3.6f);
            myList.Add(52.6f);
            myList.Add(523.6f);

            foreach (float f in myList)
            {
                Console.WriteLine(f);
            }

            List<A> myAList = new List<A>();
            myAList.Add(new A());
            myAList.Add(new A());

            foreach (A a in myAList)
            {
                a.f();
            }
        }
    }
}
