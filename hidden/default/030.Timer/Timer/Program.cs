using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TimerTest
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
            System.Threading.Timer t = new System.Threading.Timer(new TimerCallback(timer_elapsed));
            Console.WriteLine("Starting timer at: ");
            Console.WriteLine(DateTime.Now);
            t.Change(1000, 1000);

            Thread.Sleep(5000);
            t.Dispose();
        }

        public void timer_elapsed(Object state)
        {
            Console.Write("Timer elapsed: ");
            Console.WriteLine(DateTime.Now);
        }
    }
}
