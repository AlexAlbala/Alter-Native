using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefaultArguments
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            p.Run();
            p.Run(10);
            p.Run(20, 30);

            Program p2 = new Program(10);
            Program p3 = new Program(20, 30);
        }

        public void Run(int defaultParameter1 = 1, int defaultParameter2 = 2)
        {
            Console.WriteLine("Function");
            Console.WriteLine("Default parameter 1 is:");
            Console.WriteLine(defaultParameter1);

            Console.WriteLine("Default parameter 2 is:");
            Console.WriteLine(defaultParameter2);
        }

        public Program(int defaultParameter1 = 1, int defaultParameter2 = 2)
        {
            Console.WriteLine("Constructor");
            Console.WriteLine("Default parameter 1 is:");
            Console.WriteLine(defaultParameter1);

            Console.WriteLine("Default parameter 2 is:");
            Console.WriteLine(defaultParameter2);
        }
    }
}
