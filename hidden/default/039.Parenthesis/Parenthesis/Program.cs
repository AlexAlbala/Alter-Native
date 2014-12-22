using System;

namespace Parenthesis
{
    class Program
    {
        static int Calc(int a, int b)
        {
            return (a + 10) * b;
        }

        static void Main(string[] args)
        {
            var result = Calc(5, 2);
            Console.WriteLine("The result should be 30");
            Console.WriteLine(result); // (5+10)*2 = 30
        }
    }
}