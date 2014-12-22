using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleEvent
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
            SimpleEventClass se = new SimpleEventClass();
            Console.WriteLine("Created event");
            se.simpleEvent += se_simpleEvent;
            Console.WriteLine("Subscribed");
            Console.WriteLine("I will fire 10 events just now !");
            se.Start();
        }

        void se_simpleEvent(int a, float b)
        {
            Console.WriteLine("Event fired");
            Console.WriteLine(a);
            Console.WriteLine(b);
        }
    }
}
