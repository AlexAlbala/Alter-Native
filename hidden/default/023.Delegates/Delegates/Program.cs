using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delegates
{
    class Program
    {
        static void Main(string[] args)
        {
            DelegatesTest d = new DelegatesTest();
            d.Test();
        }
    }

    class DelegatesTest
    {
        delegate void Target1Del();
        delegate int TargetIntInt(int i);

        public void Test()
        {
            Console.WriteLine("One delegate");
            Target1Del targ = new Target1Del(Target1);
            targ.Invoke();

            Console.WriteLine("Other delegate");
            targ = new Target1Del(Target2);
            targ.Invoke();

            Console.WriteLine("Now multicast");
            targ = new Target1Del(Target1);
            targ += new Target1Del(Target2);
            targ.Invoke();

            Console.WriteLine("Now with parameters");
            TargetIntInt targInt = new TargetIntInt(Target3);

            int a = targInt.Invoke(10);
            Console.WriteLine(a);
        }

        public void Target1()
        {
            Console.WriteLine("Hello");
        }

        public void Target2()
        {
            Console.WriteLine("World");
        }

        public int Target3(int i)
        {
            return i;
        }

    }

}
