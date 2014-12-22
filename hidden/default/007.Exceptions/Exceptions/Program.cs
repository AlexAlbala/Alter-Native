using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExceptionExample
{
    class Program
    {
        public static void g(int i)
        {
            try
            {
                i = f(i);
                Console.Write("Result: ");
                Console.WriteLine(i);
            }
            catch (Exception e)
            {
                Console.Write("CATCH 2 => ");
                Console.WriteLine(e.Message);
            }
        }

        public static int f(int i)
        {
            int j = 0;
            Console.Write("f(");
            Console.Write(i);
            Console.WriteLine(")");
            

            if (i == 0) return j;

            if (i == 1) throw new Exception("one");

            try
            {
                if (i == 2)
                {
                    j = 2;
                    return j;
                }
                if (i == 3 || i == 4)
                {
                    j = 3;
                    throw new Exception();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("CATCH");

                if (i == 4)
                {
                    j = 4;
                    throw new Exception("four");
                }
            }
            finally
            {
                Console.WriteLine("FINALLY");
                if (i == 2)
                {
                    j = 22;
                    Console.Write("RETURN SHOULD BE ");
                    Console.WriteLine(j);                    
                }
                if (i == 5)
                {
                    throw new Exception("five");
                }
            }

            if (i == 6) throw new Exception("six");

            Console.WriteLine("RETURN");

            return j;
        }

        static void Main(string[] args)
        {
            g(0);
            g(1);
            g(2);
            g(3);
            g(4);
            g(5);
            g(6);
            g(100);
        }
    }
}