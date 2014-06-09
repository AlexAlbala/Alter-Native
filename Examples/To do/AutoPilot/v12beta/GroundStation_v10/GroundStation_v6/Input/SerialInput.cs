using System;
using System.Collections.Generic;
using System.Text;

using System.IO.Ports;

namespace GroundStation
{
	/// <summary>
	/// Serial input.
	/// </summary>
    public class SerialInput : Input
    {
		/// <summary>
		/// The serial port.
		/// </summary>
        private SerialPort sp;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="GroundStation.SerialInput"/> class.
		/// </summary>
		/// <param name='port'>
		/// Port.
		/// </param>
		/// <param name='baudRate'>
		/// Baud rate.
		/// </param>
        public SerialInput(string port, int baudRate)
        {
            this.sp = new SerialPort(port, baudRate, Parity.None, 8, StopBits.One);
            this.sp.Open();
        }
		
		/// <summary>
		/// Reads the first N bytes from the stream
		/// </summary>
		/// <returns>
		/// The N bytes.
		/// </returns>
		/// <param name='n'>
		/// The number of bytes to be read
		/// </param>
        public override byte[] ReadNBytes(int n)
        {
            byte[] ans = new byte[n];
			for (int i = 0; i < n; i++)
            {
               	ans[i] = (byte)this.sp.ReadByte();
                //Console.Write(ans[i] + " ");
            }
            //Console.WriteLine();
            return ans;
        }
		
		public void WriteNBytes(char[] b)
		{
			this.sp.Write(b, 0, b.Length);
		}
		
		public void WriteNBytes(byte[] b)
		{
			this.sp.Write(b, 0, b.Length);
		}
    }
}
