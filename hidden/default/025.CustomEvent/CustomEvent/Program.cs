using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomEvent
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            p.Run();
        }

        void Run()
        {
            CustomEventClass cec = new CustomEventClass();
            cec.customEvent += cec_customEvent;
            cec.customEvent += cec_customEvent2;  

            cec.RaiseEvent(3, (float)5.0);

            cec.customEvent -= cec_customEvent;

            cec.RaiseEvent(2, (float)8.0);
        }

        void cec_customEvent(int a, float b)
        {
            Console.WriteLine("1: Event raised");
            Console.WriteLine(a);
            Console.WriteLine(b);
        }

        void cec_customEvent2(int a, float b)
        {
            Console.WriteLine("2: Event raised");
            Console.WriteLine(a);
            Console.WriteLine(b);
        }
    }
}
