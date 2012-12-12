using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InterClassBoxing
{
    class Container<T>
    {
        private T data;

        public void Set(T t)
        {
            data = t;
        }

        public T Get()
        {
            return data;
        }
    }

    class Program
    {
        public static void Case1()
        {
            Container<int> a = new Container<int>();

            int i = 5;
            a.Set(i);
            int j = a.Get();
            Console.WriteLine(j);
        }

        public static void Case2()
        {
            Container<int> a = new Container<int>();

            Object k = 5;
            a.Set((int)k);
            Object l = a.Get();
            Console.WriteLine(l);
        }

        public static void Case3()
        {
            Container<Object> a = new Container<Object>();

            int i = 5;
            a.Set(i);
            int j = (int)a.Get();
            Console.WriteLine(j);
        }

        public static void Case4()
        {
            Container<Object> a = new Container<Object>();

            Object k = 5;
            a.Set(k);
            Object l = a.Get();
            Console.WriteLine(l);
        }



        static void Main(string[] args)
        {
            Case1();
            Case2();
            Case3();
            Case4();
        }
    }
}
