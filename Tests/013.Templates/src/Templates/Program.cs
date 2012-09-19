﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Templates
{
    class Program
    {
        static void Main(string[] args)
        {
            MyClass<A> myA = new MyClass<A>();
            myA.set(new A());

            MyClass<B> myB = new MyClass<B>();
            myB.set(new B());

            MyClass<string> myString = new MyClass<string>();
            myString.set("Hello");

            Console.WriteLine(myA.get().getText());
            Console.WriteLine(myB.get().getText());
            Console.WriteLine(myString.get());
        }
    }

    public class MyClass<T>
    {
        T data;

        public void set(T data)
        {
            this.data = data;
        }

        public T get()
        {
            return data;
        }
    }

    public class A
    {
        public string getText()
        {
            return "I'm class A!!";
        }
    }

    public class B
    {
        public string getText()
        {
            return "I'm class B!!";
        }
    }
}
