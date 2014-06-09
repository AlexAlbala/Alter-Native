using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SPOT.Hardware;

namespace IOSharp.NETMF.RaspberryPi.Hardware
{
    public class RaspberryPiHWProvider : HardwareProvider
    {
        override public int GetAnalogChannelsCount()
        {
            throw new NotImplementedException();
        }

        override public int GetAnalogOutputChannelsCount()
        {
            throw new NotImplementedException();
        }

        override public Cpu.Pin GetAnalogOutputPinForChannel(Cpu.AnalogOutputChannel channel)
        {
            throw new NotImplementedException();
        }

        override public Cpu.Pin GetAnalogPinForChannel(Cpu.AnalogChannel channel)
        {
            throw new NotImplementedException();
        }

        override public int[] GetAvailableAnalogOutputPrecisionInBitsForChannel(Cpu.AnalogOutputChannel channel)
        {
            throw new NotImplementedException();
        }

        override public int[] GetAvailablePrecisionInBitsForChannel(Cpu.AnalogChannel channel)
        {
            throw new NotImplementedException();
        }

        override public void GetBaudRateBoundary(int com, out uint MaxBaudRate, out uint MinBaudRate)
        {
            throw new NotImplementedException();
        }

        override public void GetI2CPins(out Cpu.Pin scl, out Cpu.Pin sda)
        {
            throw new NotImplementedException();
        }

        override public void GetLCDMetrics(out int width, out int height, out int bitsPerPixel, out int orientationDeg)
        {
            throw new NotImplementedException();
        }

        override public int GetPinsCount()
        {
            throw new NotImplementedException();
        }

        override public void GetPinsMap(out Cpu.PinUsage[] pins, out int PinCount)
        {
            throw new NotImplementedException();
        }

        override public Cpu.PinUsage GetPinsUsage(Cpu.Pin pin)
        {
            throw new NotImplementedException();
        }

        override public int GetPWMChannelsCount()
        {
            throw new NotImplementedException();
        }

        override public Cpu.Pin GetPwmPinForChannel(Cpu.PWMChannel channel)
        {
            throw new NotImplementedException();
        }

        override public void GetSerialPins(string comPort, out Cpu.Pin rxPin, out Cpu.Pin txPin, out Cpu.Pin ctsPin, out Cpu.Pin rtsPin)
        {
            throw new NotImplementedException();
        }

        override public int GetSerialPortsCount()
        {
            throw new NotImplementedException();
        }

        override public void GetSpiPins(SPI.SPI_module spi_mod, out Cpu.Pin msk, out Cpu.Pin miso, out Cpu.Pin mosi)
        {
            if ((int)spi_mod == 0)
            {
                msk = Pins.SPI0_SCLK;
                miso = Pins.SPI0_MISO;
                mosi = Pins.SPI0_MOSI;
            }
            else{
                throw new NotImplementedException();
            }
        }

        override public int GetSpiPortsCount()
        {
            throw new NotImplementedException();
        }

        override public Cpu.PinValidInterruptMode GetSupportedInterruptModes(Cpu.Pin pin)
        {
            throw new NotImplementedException();
        }

        override public Cpu.PinValidResistorMode GetSupportedResistorModes(Cpu.Pin pin)
        {
            throw new NotImplementedException();
        }

        override public bool IsSupportedBaudRate(int com, ref uint baudrateHz)
        {
            throw new NotImplementedException();
        }

        override public bool SupportsNonStandardBaudRate(int com)
        {
            throw new NotImplementedException();
        }
    }
}
