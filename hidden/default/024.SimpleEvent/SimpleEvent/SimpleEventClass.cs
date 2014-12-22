using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleEvent
{
    class SimpleEventClass
    {
        public delegate void SimpleEventHandler(int a, float b);
        public event SimpleEventHandler simpleEvent;

        public void Start()
        {
            int a = 0;

            while (a < 10)
            {
                if (simpleEvent != null)
                    simpleEvent(a, a + a);

                a++;
            }
        }
    }
}
