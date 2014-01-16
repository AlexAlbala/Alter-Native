using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SPOT.Hardware;
using IOSharp.NETMF.RaspberryPi.Hardware;
using System.Threading;


namespace raspberrypi
{
    class Program
    {
        public static void Main()
        {

            // Specify the GPIO pin we want to use as an interrupt 
            // source, specify the edges the interrupt should trigger on 
            InterruptPort button = new InterruptPort(Pins.V2_GPIO2, false,
              Port.ResistorMode.Disabled, Port.InterruptMode.InterruptEdgeBoth);

            // Hook up an event handler (delegate) to the OnInterrupt event 
            button.OnInterrupt += new NativeEventHandler(button_OnInterrupt);
            Debug.Print("Started"); 
            Thread.Sleep(-1);
        }

        static void button_OnInterrupt(uint port, uint state, DateTime time)
        {
            // This method is called whenever an interrupt occurrs
            Debug.Print("Interruption");
            OutputPort bar = new OutputPort(Pins.V2_GPIO17, false);
            bar.Write(false);
            bool foo = false;
            OutputPort o = new OutputPort(Pins.V2_GPIO11, false);

            bar.Write(true);
            for (int i = 0; i < 200; i++)
            {
                foo = !foo;
                o.Write(foo);
            }
            bar.Write(false);
            Debug.Print("END");
        }
    }
}
