using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Threads
{
    class Program
    {
        
        static void Main(string[] args)
        {
            Program p = new Program();
            p.Run();
        }

        public void Run()
        {
            Thread th1;
            Thread th2;

            th1 = new Thread(new ThreadStart(target1));
            th2 = new Thread(new ThreadStart(target2));

            Console.WriteLine("Start threads");
            th1.Start();
            th2.Start();

            Thread.Sleep(5000);
            Console.WriteLine("Abort Target 2");
            th2.Abort();            
            Console.WriteLine("Wait for Target 1 to finish (15 iterations)");
            th1.Join();
            Console.WriteLine("Finished");
        }

        public void target1()
        {
            int i = 0;
            while (i < 15)
            {
                Console.Write("Target 1 - ");
                Console.WriteLine(i++);
                Thread.Sleep(1000);
            }
        }

        public void target2()
        {
            int i = 0;
            while (i < 15)
            {
                Console.Write("Target 2 - ");
                Console.WriteLine(i++);
                Thread.Sleep(1000);
            }
        }
    }
}
