using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AsIs
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            p.AsIsTest();
        }

        public void AsIsTest()
        {
            Person p = new Person();
            Console.Write("Person: ");
            Console.WriteLine(p.getName());

            Object j = new John();
            Object a = new Anne();
            Object n = new Person();

            Console.WriteLine("Cast to super class");
            if (j is Person)
            {
                Person _j = j as Person;
                Console.WriteLine(_j.getName());
            }
            if (a is Person)
            {
                Person _a = a as Person;
                Console.WriteLine(_a.getName());
            }
            Console.WriteLine("Cast to specific class");
            if (j is John)
            {
                John _j = j as John;
                Console.WriteLine(_j.getName());
            }
            if (a is Anne)
            {
                Anne _a = a as Anne;
                Console.WriteLine(_a.getName());
            }
            if (n is John)
            {
                Console.WriteLine("error");
            }
        }
    }

    class Person
    {
        protected string name = "Unassigned";

        public string getName()
        {
            return name;
        }
    }

    class John : Person
    {
        public John()
        {
            name = "John";
        }
    }

    class Anne : Person
    {
        public Anne()
        {
            name = "Anne";
        }
    }
}
