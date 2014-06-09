using System;
using System.Collections;
using System.Threading;
using System.IO;
using System.Runtime.CompilerServices;
using Microsoft.SPOT.Exceptions;

namespace Microsoft.SPOT.Hardware
{
    public class HardwareProvider
    {
        private static HardwareProvider s_hwProvider = null;

        //--//

        public static void Register(HardwareProvider provider)
        {
            s_hwProvider = provider;

        }

        //--//

        public static HardwareProvider HwProvider
        {
            get
            {
                if (s_hwProvider == null)
                {
                    //s_hwProvider = new HardwareProvider();
                    throw new HardwareProvidedNotRegistered();
                }

                return s_hwProvider;
            }
        }

        //--//

        public virtual void GetSerialPins(string comPort, out Cpu.Pin rxPin, out Cpu.Pin txPin, out Cpu.Pin ctsPin, out Cpu.Pin rtsPin)
        {
            throw new HardwareProvidedNotRegistered();
        }

        public virtual int GetSerialPortsCount()
        {

            throw new HardwareProvidedNotRegistered();
        }

        public virtual bool SupportsNonStandardBaudRate(int com)
        {
            throw new HardwareProvidedNotRegistered();
        }

        public virtual void GetBaudRateBoundary(int com, out uint MaxBaudRate, out uint MinBaudRate)
        {
            throw new HardwareProvidedNotRegistered();
        }

        public virtual bool IsSupportedBaudRate(int com, ref uint baudrateHz)
        {
            throw new HardwareProvidedNotRegistered();
        }

        //public virtual void GetSupportBaudRates(int com, out System.IO.Ports.BaudRate[] StdBaudRate, out int size)
        //{
        //    throw new NotImplementedException();
        //}

        //--//
        public virtual void GetSpiPins(SPI.SPI_module spi_mod, out Cpu.Pin msk, out Cpu.Pin miso, out Cpu.Pin mosi)
        {
            throw new HardwareProvidedNotRegistered();
        }

        public virtual int GetSpiPortsCount()
        {
            throw new HardwareProvidedNotRegistered();
        }

        //--//
        public virtual void GetI2CPins(out Cpu.Pin scl, out Cpu.Pin sda)
        {
            throw new HardwareProvidedNotRegistered();
        }


        public virtual int GetPWMChannelsCount()
        {
            throw new HardwareProvidedNotRegistered();
        }

        public virtual Cpu.Pin GetPwmPinForChannel(Cpu.PWMChannel channel)
        {
            throw new HardwareProvidedNotRegistered();
        }

        //--//

        public virtual int GetAnalogChannelsCount()
        {
            throw new HardwareProvidedNotRegistered();
        }

        public virtual Cpu.Pin GetAnalogPinForChannel(Cpu.AnalogChannel channel)
        {
            throw new HardwareProvidedNotRegistered();
        }

        public virtual int[] GetAvailablePrecisionInBitsForChannel(Cpu.AnalogChannel channel)
        {
            throw new HardwareProvidedNotRegistered();
        }

        //--//

        public virtual int GetAnalogOutputChannelsCount()
        {
            throw new HardwareProvidedNotRegistered();
        }

        public virtual Cpu.Pin GetAnalogOutputPinForChannel(Cpu.AnalogOutputChannel channel)
        {
            throw new HardwareProvidedNotRegistered();
        }

        public virtual int[] GetAvailableAnalogOutputPrecisionInBitsForChannel(Cpu.AnalogOutputChannel channel)
        {
            throw new HardwareProvidedNotRegistered();
        }

        //--//

        public virtual int GetPinsCount()
        {
            throw new HardwareProvidedNotRegistered();
        }

        public virtual void GetPinsMap(out Cpu.PinUsage[] pins, out int PinCount)
        {
            throw new HardwareProvidedNotRegistered();
        }

        public virtual Cpu.PinUsage GetPinsUsage(Cpu.Pin pin)
        {
            throw new HardwareProvidedNotRegistered();
        }

        public virtual Cpu.PinValidResistorMode GetSupportedResistorModes(Cpu.Pin pin)
        {
            throw new HardwareProvidedNotRegistered();
        }

        public virtual Cpu.PinValidInterruptMode GetSupportedInterruptModes(Cpu.Pin pin)
        {
            throw new HardwareProvidedNotRegistered();
        }

        //--//

        //public virtual Cpu.Pin GetButtonPins(Button iButton)
        //{
        //    throw new NotImplementedException();
        //}

        //--//
        public virtual void GetLCDMetrics(out int width, out int height, out int bitsPerPixel, out int orientationDeg)
        {
            throw new HardwareProvidedNotRegistered();
        }
    }
}


