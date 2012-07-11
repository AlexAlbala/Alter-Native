using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Boxing
{
    class Program
    {
        static void Main(string[] args)
        {
            Box();
            UnBox();
        }

        static void Box()
        {
            int i = 123;
            object o = i;  // implicit boxing

            i = 456;  // change the contents of i

            System.Console.WriteLine("The value-type value = {0}", i);
            System.Console.WriteLine("The object-type value = {0}", o);
        }

        static void UnBox()
        {
            object o = 123;
            int i = (int)o;

            o = 456;  // change the contents of i

            System.Console.WriteLine("The value-type value = {0}", i);
            System.Console.WriteLine("The object-type value = {0}", o);
        }
    }
}
