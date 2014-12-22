using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomEvent
{
    class CustomEventClass
    {
        public delegate void CustomEventHandler(int a, float b);

        private CustomEventHandler myPrivateEvent;
        public event CustomEventHandler customEvent
        {
            add
            {
                Console.WriteLine("I'm adding a delegate ");
                myPrivateEvent += value;
            }

            remove
            {
                Console.WriteLine("I'm removing a delegate");
                myPrivateEvent -= value;
            }
        }

        public void RaiseEvent(int a, float b)
        {
            if (myPrivateEvent != null)
                myPrivateEvent(a, b);            
        }


    }
}
