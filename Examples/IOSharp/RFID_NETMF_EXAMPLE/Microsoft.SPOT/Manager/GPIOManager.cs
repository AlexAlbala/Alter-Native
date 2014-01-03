using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SPOT.Hardware;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Globalization;

namespace Microsoft.SPOT.Manager
{
    public enum PortType
    {
        NONE,
        INPUT,
        OUTPUT,
        TRISTATE,
        INTERRUPT,
        RESERVED
    };

    class GPIOManager
    {
        private static GPIOManager instance;



        private static Dictionary<Cpu.Pin, PortType> _activePins = new Dictionary<Cpu.Pin, PortType>();
        //private static List<Cpu.Pin> _reservedPins = new List<Cpu.Pin>();

        protected static String GPIO_PATH = "/sys/class/gpio/";

        private GPIOManager() { }

        public static GPIOManager GetInstance()
        {
            if (instance == null)
            {
                instance = new GPIOManager();
            }
            return instance;
        }

        public void Export(Cpu.Pin pin)
        {
            if (!_activePins.ContainsKey(pin))
            {
                File.WriteAllText(GPIO_PATH + "export", ((int)pin).ToString());
                _activePins.Add(pin, PortType.NONE);
            }
        }

        //public bool IsPinExported(Cpu.Pin pin)
        //{
        //    if (_activePins.ContainsKey(pin))
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        List<String> l = Directory.GetDirectories(GPIO_PATH).ToList();
        //        if (l.Contains(GPIO_PATH + pin.ToString().Split('_')[1]))
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //}

        public void Unexport(Cpu.Pin pin)
        {
            if (_activePins.ContainsKey(pin))
            {
                File.WriteAllText(GPIO_PATH + "unexport", ((int)pin).ToString());
                _activePins.Remove(pin);
            }
        }

        public bool Read(Cpu.Pin pin)
        {
            if (_activePins.ContainsKey(pin))
            {
                //if ((_activePins[pin] == PortType.INPUT) || (_activePins[pin] == PortType.OUTPUT))
                //{
                String value = File.ReadAllText(GPIO_PATH + "gpio" + ((int)pin) + "/value");
                return value == "1" ? true : false;
                //}
                //else if ((_activePins[pin] == PortType.INTERRUPT) || (_activePins[pin] == PortType.TRISTATE))
                //{
                //    throw new NotImplementedException();
                //}
                //else
                //{
                //    throw new Exception();
                //}
            }
            else
            {
                throw new Exception();
            }
        }

        public void Write(Cpu.Pin pin, bool state)
        {
            if (_activePins.ContainsKey(pin))
            {
                if ((_activePins[pin] == PortType.OUTPUT))
                {
                    int value = state == true ? 1 : 0;                    
                    File.WriteAllText(GPIO_PATH + "gpio" + ((int)pin) + "/value", value.ToString().ToLower());
                }
                else if ((_activePins[pin] == PortType.TRISTATE))
                {
                    throw new NotImplementedException();
                }
                else
                {
                    throw new Exception();
                }
            }
            else { throw new Exception(); }
        }

        public void SetPortType(Cpu.Pin pin, PortType type)
        {
            if (_activePins.ContainsKey(pin))
            {
                string direction = "";
                switch (type)
                {
                    case PortType.NONE:
                        _activePins[pin] = PortType.NONE;
                        throw new NotImplementedException();
                    case PortType.INPUT:
                        direction = "in";
                        _activePins[pin] = PortType.INPUT;
                        break;
                    case PortType.OUTPUT:
                        direction = "out";
                        _activePins[pin] = PortType.OUTPUT;
                        break;
                    case PortType.TRISTATE:
                        _activePins[pin] = PortType.TRISTATE;
                        throw new NotImplementedException();
                    case PortType.INTERRUPT:
                        direction = "in";
                        _activePins[pin] = PortType.INTERRUPT;
                        break;
                }
                File.WriteAllText(GPIO_PATH + "gpio" + ((int)pin) + "/direction", direction.ToLower());
            }
            else
            {
                throw new Exception();
            }
        }

        public void SetEdge(Cpu.Pin pin, Port.InterruptMode interruptMode)
        {
            String interrupt = "none";
            Console.WriteLine(interruptMode);
            switch (interruptMode)
            {
                case Port.InterruptMode.InterruptNone:
                    break;
                case Port.InterruptMode.InterruptEdgeLow:
                    interrupt = "falling";
                    break;
                case Port.InterruptMode.InterruptEdgeLevelLow:
                    throw new Exception();
                case Port.InterruptMode.InterruptEdgeLevelHigh:
                    throw new Exception();
                case Port.InterruptMode.InterruptEdgeHigh:
                    interrupt = "rising";
                    break;
                case Port.InterruptMode.InterruptEdgeBoth:
                    interrupt = "both";
                    break;
            }
            File.WriteAllText(GPIO_PATH + "gpio" + ((int)pin) + "/edge", interrupt.ToLower());
        }

        public bool ReservePin(Cpu.Pin pin, bool fReserve)
        {

            if (_activePins.ContainsKey(pin) && fReserve)
            {
                return false;
            }
            else if (_activePins.ContainsKey(pin) && !fReserve)
            {
                _activePins.Remove(pin);
                return true;
            }
            else if (!_activePins.ContainsKey(pin) && fReserve)
            {
                _activePins.Add(pin, PortType.RESERVED);
                return true;
            }
            else
            {
                return false;
            }

        }

        public void Listen_events(Cpu.Pin pin, NativeEventHandler callback)
        {
            Thread t = new Thread(this.Listen);
            ThreadHelper th = new ThreadHelper();
            th.Pin = pin;
            th.Callback = callback;
            t.Start(th);
        }

        private void Listen(object obj)
        {
            ThreadHelper th = (ThreadHelper)obj;
            while (true)
            {
                callback_p cback = GPIOManager.start_polling((int)th.Pin);
                th.Callback((uint)cback.pin, (uint)0, DateTime.Now);
            }
        }

        [DllImport("libIOSharp-c.so", CallingConvention = CallingConvention.StdCall)]
        private static extern callback_p start_polling(int gpio);

        private class ThreadHelper
        {
            public Cpu.Pin Pin { get; set; }
            public NativeEventHandler Callback { get; set; }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct callback_p
        {
            public int pin;
            public string state;
            public long poll_time;
        }
    }
}