using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.SPOT.Hardware
{
    public sealed class SPI : IDisposable
    {
        public enum SPI_module : int
        {
            SPI1 = 0,
            SPI2 = 1,
            SPI3 = 2,
            SPI4 = 3,
        }

        public class Configuration
        {
            public readonly Cpu.Pin ChipSelect_Port;
            public readonly bool ChipSelect_ActiveState;
            public readonly uint ChipSelect_SetupTime;
            public readonly uint ChipSelect_HoldTime;
            public readonly bool Clock_IdleState;
            public readonly bool Clock_Edge;
            public readonly uint Clock_RateKHz;
            public readonly SPI_module SPI_mod;
            public readonly Cpu.Pin BusyPin;
            public readonly bool BusyPin_ActiveState;

            public Configuration(
                                  Cpu.Pin ChipSelect_Port,
                                  bool ChipSelect_ActiveState,
                                  uint ChipSelect_SetupTime,
                                  uint ChipSelect_HoldTime,
                                  bool Clock_IdleState,
                                  bool Clock_Edge,
                                  uint Clock_RateKHz,
                                  SPI_module SPI_mod
                                )
                : this(ChipSelect_Port,
                        ChipSelect_ActiveState,
                        ChipSelect_SetupTime,
                        ChipSelect_HoldTime,
                        Clock_IdleState,
                        Clock_Edge,
                        Clock_RateKHz,
                        SPI_mod,
                        Cpu.Pin.GPIO_NONE,
                        false)
            {
            }

            public Configuration(
                                  Cpu.Pin ChipSelect_Port,
                                  bool ChipSelect_ActiveState,
                                  uint ChipSelect_SetupTime,
                                  uint ChipSelect_HoldTime,
                                  bool Clock_IdleState,
                                  bool Clock_Edge,
                                  uint Clock_RateKHz,
                                  SPI_module SPI_mod,
                                  Cpu.Pin BusyPin,
                                  bool BusyPin_ActiveState

                                )
            {
                this.ChipSelect_Port = ChipSelect_Port;
                this.ChipSelect_ActiveState = ChipSelect_ActiveState;
                this.ChipSelect_SetupTime = ChipSelect_SetupTime;
                this.ChipSelect_HoldTime = ChipSelect_HoldTime;
                this.Clock_IdleState = Clock_IdleState;
                this.Clock_Edge = Clock_Edge;
                this.Clock_RateKHz = Clock_RateKHz;
                this.SPI_mod = SPI_mod;
                this.BusyPin = BusyPin;
                this.BusyPin_ActiveState = BusyPin_ActiveState;
            }
        }

        //--//

        private Configuration m_config;
        private OutputPort m_cs;
        private bool m_disposed;

        //--//

        public SPI(Configuration config)
        {
            HardwareProvider hwProvider = HardwareProvider.HwProvider;

            if (hwProvider != null)
            {
                Cpu.Pin msk;
                Cpu.Pin miso;
                Cpu.Pin mosi;

                hwProvider.GetSpiPins(config.SPI_mod, out msk, out miso, out mosi);

                if (msk != Cpu.Pin.GPIO_NONE)
                {
                    Port.ReservePin(msk, true);
                }

                if (miso != Cpu.Pin.GPIO_NONE)
                {
                    Port.ReservePin(miso, true);
                }

                if (mosi != Cpu.Pin.GPIO_NONE)
                {
                    Port.ReservePin(mosi, true);
                }
            }

            //if (config.ChipSelect_Port != Cpu.Pin.GPIO_NONE)
            //{
            //    //m_cs = new OutputPort(config.ChipSelect_Port, !config.ChipSelect_ActiveState);
            //    Port.ReservePin(config.ChipSelect_Port, true);
            //}

            if (config.ChipSelect_Port != Cpu.Pin.GPIO_NONE)
            {
                Port.ReservePin(config.ChipSelect_Port, true);
            }

            m_config = config;
            m_disposed = false;
        }

        ~SPI()
        {
            Dispose(false);
        }

        [MethodImplAttribute(MethodImplOptions.Synchronized)]
        private void Dispose(bool fDisposing)
        {
            if (!m_disposed)
            {
                try
                {
                    HardwareProvider hwProvider = HardwareProvider.HwProvider;

                    if (hwProvider != null)
                    {
                        Cpu.Pin msk;
                        Cpu.Pin miso;
                        Cpu.Pin mosi;

                        hwProvider.GetSpiPins(m_config.SPI_mod, out msk, out miso, out mosi);

                        if (msk != Cpu.Pin.GPIO_NONE)
                        {
                            Port.ReservePin(msk, false);
                        }

                        if (miso != Cpu.Pin.GPIO_NONE)
                        {
                            Port.ReservePin(miso, false);
                        }

                        if (mosi != Cpu.Pin.GPIO_NONE)
                        {
                            Port.ReservePin(mosi, false);
                        }
                    }

                    if (m_config.ChipSelect_Port != Cpu.Pin.GPIO_NONE)
                    {
                        m_cs.Dispose();
                    }
                }
                finally
                {
                    m_disposed = true;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        public void WriteRead(ushort[] writeBuffer, int writeOffset, int writeCount, ushort[] readBuffer, int readOffset, int readCount, int startReadOffset)
        {
            if (m_disposed)
            {
                throw new ObjectDisposedException("");
            }

            if (
                // write buffer can never be null
                (writeBuffer == null) ||
                // write buffer must be larger than the sum of the offset and the count for writing from it
                (writeOffset + writeCount > writeBuffer.Length) ||
                // read buffer must be larger than the offset and the count for writing from it
                ((readBuffer != null) && (readOffset + readCount > readBuffer.Length))
               )
            {
                throw new ArgumentException();
            }

            InternalWriteReadShort(writeBuffer, writeOffset, writeCount, readBuffer, readOffset, readCount, startReadOffset, new spi_config(m_config));
        }

        public void WriteRead(ushort[] writeBuffer, ushort[] readBuffer, int startReadOffset)
        {
            if (m_disposed)
            {
                throw new ObjectDisposedException("");
            }

            if (writeBuffer == null)
            {
                throw new ArgumentException();
            }

            int readBufLen = 0;

            if (readBuffer != null)
            {
                readBufLen = readBuffer.Length;
            }

            InternalWriteReadShort(writeBuffer, 0, writeBuffer.Length, readBuffer, 0, readBufLen, startReadOffset, new spi_config(m_config));
        }

        public void WriteRead(ushort[] writeBuffer, ushort[] readBuffer)
        {
            if (readBuffer == null)
            {
                throw new ArgumentException();
            }

            WriteRead(writeBuffer, readBuffer, 0);
        }

        public void Write(ushort[] writeBuffer)
        {
            WriteRead(writeBuffer, null, 0);
        }

        public void WriteRead(byte[] writeBuffer, int writeOffset, int writeCount, byte[] readBuffer, int readOffset, int readCount, int startReadOffset)
        {
            if (m_disposed)
            {
                throw new ObjectDisposedException("");
            }

            if (
                // write buffer can never be null
                (writeBuffer == null) ||
                // write buffer must be larger than the sum of the offset and the count for writing from it
                (writeOffset + writeCount > writeBuffer.Length) ||
                // read buffer must be larger than the offset and the count for writing from it
                ((readBuffer != null) && (readOffset + readCount > readBuffer.Length))
               )
            {
                throw new ArgumentException();
            }

            InternalWriteReadByte(writeBuffer, writeOffset, writeCount, readBuffer, readOffset, readCount, startReadOffset, new spi_config(m_config));
        }

        public void WriteRead(byte[] writeBuffer, byte[] readBuffer, int startReadOffset)
        {
            if (m_disposed)
            {
                throw new ObjectDisposedException("");
            }

            if (writeBuffer == null)
            {
                throw new ArgumentException();
            }
            int readBufLen = 0;

            if (readBuffer == null)
            {
                readBuffer = new Byte[writeBuffer.Length];
            }
            readBufLen = readBuffer.Length;

            InternalWriteReadByte(writeBuffer, 0, writeBuffer.Length, readBuffer, 0, readBufLen, startReadOffset, new spi_config(m_config));
        }

        public void WriteRead(byte[] writeBuffer, byte[] readBuffer)
        {
            if (readBuffer == null)
            {
                throw new ArgumentException();
            }

            WriteRead(writeBuffer, readBuffer, 0);
        }

        public void Write(byte[] writeBuffer)
        {
            WriteRead(writeBuffer, null, 0);
        }

        public Configuration Config
        {
            get
            {
                return m_config;
            }

            set
            {
                m_config = value;
            }
        }

        //--//
        /// <summary>
        ///  Writes specified number of bytes from writeBuffer to SPI bus. Reads data from SPI bus and places into readBuffer.
        ///  writeBuffer     - array with data to be written to SPI bus.
        ///  writeElemCount  - number of elements to write to SPI bus. If writeElemCount is -1, then all data is array is written to bus.
        ///  readBuffer      - buffer to place data read from SPI bus
        ///  readOffset      - Number of elements to skip before starting to read.
        /// </summary>
        /// <param name="writeBuffer"></param>
        /// <param name="writeElemCount"></param>
        /// <param name="readBuffer"></param>
        /// <param name="readOffset"></param>

        [DllImport("libIOSharp-c.so", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        static extern private void InternalWriteReadShort(ushort[] writeBuffer, int writeOffset, int writeCount, ushort[] readBuffer, int readOffset, int readCount, int startReadOffset, spi_config spi);

        [DllImport("libIOSharp-c.so", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        static extern private void InternalWriteReadByte(byte[] writeBuffer, int writeOffset, int writeCount, byte[] readBuffer, int readOffset, int readCount, int startReadOffset, spi_config spi);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct spi_config
        {
            public int mode;
            public uint speed;
            public int cs_change;
            public ushort delay;

            public spi_config(Configuration config)
            {
                this.cs_change = (config.ChipSelect_ActiveState) ? 1 : 0;
                this.delay = (ushort)config.ChipSelect_HoldTime;

                if (config.Clock_Edge && !config.Clock_IdleState)
                    this.mode = 0;
                else if (!config.Clock_Edge && !config.Clock_IdleState)
                    this.mode = 1;
                else if (config.Clock_Edge && config.Clock_IdleState)
                    this.mode = 2;
                else
                    this.mode = 3;
                this.speed = config.Clock_RateKHz*1000;
            }
        }
    }
}


