using System;
using System.Runtime.CompilerServices;
using Microsoft.SPOT.Manager;

namespace Microsoft.SPOT.Hardware
{

    public delegate void NativeEventHandler(uint data1, uint data2, DateTime time);

    //--//

    public class NativeEventDispatcher : IDisposable
    {
        protected NativeEventHandler m_threadSpawn = null;
        protected NativeEventHandler m_callbacks = null;
        protected bool m_disposed = false;
        private object m_NativeEventDispatcher;

        //--//
        public NativeEventDispatcher() { }

        public NativeEventDispatcher(string strDriverName, ulong drvData)
        {
        }

        public virtual void EnableInterrupt() { }

        public virtual void DisableInterrupt() { }

        protected virtual void Dispose(bool disposing) { }

        //--//

        ~NativeEventDispatcher()
        {
            Dispose(false);
        }

        [MethodImplAttribute(MethodImplOptions.Synchronized)]
        public virtual void Dispose()
        {
            if (!m_disposed)
            {
                Dispose(true);

                GC.SuppressFinalize(this);

                m_disposed = true;
            }
        }

        public event NativeEventHandler OnInterrupt
        {
            [MethodImplAttribute(MethodImplOptions.Synchronized)]
            add
            {
                if (m_disposed)
                {
                    throw new ObjectDisposedException("");
                }
                NativeEventHandler callbacksOld = m_callbacks;
                NativeEventHandler callbacksNew = (NativeEventHandler)Delegate.Combine(callbacksOld, value);

                try
                {
                    m_callbacks = callbacksNew;

                    if (callbacksNew != null)
                    {
                        if (callbacksOld == null)
                        {
                            EnableInterrupt();
                        }

                        if (callbacksNew.Equals(value) == false)
                        {
                            callbacksNew = new NativeEventHandler(this.MultiCastCase);
                        }
                    }

                    m_threadSpawn = callbacksNew;
                }
                catch
                {
                    m_callbacks = callbacksOld;

                    if (callbacksOld == null)
                    {
                        DisableInterrupt();
                    }

                    throw;
                }
            }

            [MethodImplAttribute(MethodImplOptions.Synchronized)]
            remove
            {
                if (m_disposed)
                {
                    throw new ObjectDisposedException("");
                }

                NativeEventHandler callbacksOld = m_callbacks;
                NativeEventHandler callbacksNew = (NativeEventHandler)Delegate.Remove(callbacksOld, value);

                try
                {
                    m_callbacks = (NativeEventHandler)callbacksNew;

                    if (callbacksNew == null && callbacksOld != null)
                    {
                        DisableInterrupt();
                    }
                }
                catch
                {
                    m_callbacks = callbacksOld;

                    throw;
                }
            }
        }

        private void MultiCastCase(uint port, uint state, DateTime time)
        {
            NativeEventHandler callbacks = m_callbacks;

            if (callbacks != null)
            {
                callbacks(port, state, time);
            }
        }
    }

    //--//

    public class Port : NativeEventDispatcher
    {
        public enum ResistorMode
        {
            Disabled = 0,
            PullDown = 1,
            PullUp = 2,
        }

        public enum InterruptMode
        {
            InterruptNone = 0,
            InterruptEdgeLow = 1,
            InterruptEdgeHigh = 2,
            InterruptEdgeBoth = 3,
            InterruptEdgeLevelHigh = 4,
            InterruptEdgeLevelLow = 5,
        }

        //--//

        private InterruptMode m_interruptMode;
        private ResistorMode m_resistorMode;
        private uint m_portId;
        private uint m_flags;
        private bool m_glitchFilterEnable;
        private bool m_initialState;
        //--//

        protected Port(Cpu.Pin portId, bool glitchFilter, ResistorMode resistor, InterruptMode interruptMode)
        {
            this.Id = portId;
            GPIOManager.GetInstance().Export(portId);
        }

        protected Port(Cpu.Pin portId, bool initialState)
        {
            this.Id = portId;
            GPIOManager.GetInstance().Export(portId);
        }

        protected Port(Cpu.Pin portId, bool initialState, bool glitchFilter, ResistorMode resistor)
        {
            this.Id = portId;
            throw new NotImplementedException();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Console.Write("Disposing(");
                Console.Write(this.Id);
                Console.WriteLine(")");
                GPIOManager.GetInstance().Unexport(this.Id);
            }
        }

        public bool Read()
        {
            Console.Write("Reading(");
            Console.Write(this.Id);
            Console.WriteLine(")");
            return GPIOManager.GetInstance().Read(this.Id);
        }

        public Cpu.Pin Id { get; set; }

        static public bool ReservePin(Cpu.Pin pin, bool fReserve)
        {
            return GPIOManager.GetInstance().ReservePin(pin, fReserve);
        }
    }

    //--//

    public class InputPort : Port
    {
        public InputPort(Cpu.Pin portId, bool glitchFilter, ResistorMode resistor)
            : base(portId, glitchFilter, resistor, InterruptMode.InterruptNone)
        {
            GPIOManager.GetInstance().SetPortType(portId, PortType.INPUT);
        }

        protected InputPort(Cpu.Pin portId, bool glitchFilter, ResistorMode resistor, InterruptMode interruptMode)
            : base(portId, glitchFilter, resistor, interruptMode)
        {
            GPIOManager.GetInstance().SetPortType(portId, PortType.INPUT);
        }

        protected InputPort(Cpu.Pin portId, bool initialState, bool glitchFilter, ResistorMode resistor)
            : base(portId, initialState, glitchFilter, resistor)
        {
            throw new NotImplementedException();
        }

        public ResistorMode Resistor { get; set; }

        public bool GlitchFilter { get; set; }

    }

    //--//

    public class OutputPort : Port
    {
        public OutputPort(Cpu.Pin portId, bool initialState)
            : base(portId, initialState)
        {
            GPIOManager.GetInstance().SetPortType(portId, PortType.OUTPUT);
        }

        protected OutputPort(Cpu.Pin portId, bool initialState, bool glitchFilter, ResistorMode resistor)
            : base(portId, initialState, glitchFilter, resistor)
        {
            throw new NotImplementedException();
        }

        public void Write(bool state)
        {
            Console.Write("Write(");
            Console.Write(this.Id);
            Console.WriteLine(")");
            GPIOManager.GetInstance().Write(this.Id, state);
        }

        public bool InitialState { get; set; }

    }

    //--//

    public sealed class TristatePort : OutputPort
    {
        private bool active = false;
        private bool firstTimeOutput = false;
        public TristatePort(Cpu.Pin portId, bool initialState, bool glitchFilter, ResistorMode resistor)
            : base(portId, initialState, glitchFilter, resistor)
        {
            GPIOManager.GetInstance().SetPortType(portId, PortType.INPUT);
            active = false;
            this.InitialState = initialState;
            this.firstTimeOutput = true;
        }

        public bool Active
        {
            get
            {
                return active;
            }
            set
            {
                if (Active)
                {
                    GPIOManager.GetInstance().SetPortType(this.Id, PortType.OUTPUT);
                    if (firstTimeOutput)
                    {
                        this.Write(InitialState);
                        firstTimeOutput = false;
                    }
                }
                else
                {
                    GPIOManager.GetInstance().SetPortType(this.Id, PortType.INPUT);
                }
                this.active = Active;
            }
        }

        public ResistorMode Resistor { get; set; }

        public bool GlitchFilter { get; set; }
    }

    //--//

    public sealed class InterruptPort : InputPort
    {
        //--//

        public InterruptPort(Cpu.Pin portId, bool glitchFilter, ResistorMode resistor, InterruptMode interrupt)
            : base(portId, glitchFilter, resistor, interrupt)
        {
            //m_threadSpawn = null;
            //m_callbacks = null;
            GPIOManager.GetInstance().SetPortType(portId, PortType.INTERRUPT);
            GPIOManager.GetInstance().SetEdge(portId, interrupt);
        }

        public void ClearInterrupt() { }

        public InterruptMode Interrupt { get; set; }

        public event NativeEventHandler OnInterrupt
        {
            [MethodImplAttribute(MethodImplOptions.Synchronized)]
            add
            {
                //GPIOManager.start_polling(17, value);
                GPIOManager.GetInstance().Listen_events(this.Id, value);
            }

            [MethodImplAttribute(MethodImplOptions.Synchronized)]
            remove
            {
                Console.WriteLine("Adeu");
            }
        }


        public override void EnableInterrupt() { }

        public override void DisableInterrupt() { }

    }
}


