using System;
using System.Collections.Generic;
using System.Text;

namespace GroundStation
{
	/// <summary>
	/// Input abstract class.
	/// </summary>
    public abstract class Input
    {
		/// <summary>
		/// Reads the first N bytes from the stream.
		/// </summary>
		/// <returns>
		/// The N bytes.
		/// </returns>
		/// <param name='n'>
		/// The number of bytes to be read
		/// </param>
        public abstract byte[] ReadNBytes(int n);
		
		/// <summary>
		/// Checks the start of message. It drops bytes from
		/// the stream until it finds the '111' secuence of bytes.
		/// </summary>
        public void CheckStartOfMessage()
        {
            bool ok = false;
            while (true)
            {
                byte[] b = this.ReadNBytes(1);
                if (b[0] != (byte)1)
                {
                    if (!ok)
                    {
                        Console.WriteLine("Warning! Not Synchronized!");
                        ok = true;
                    }
                    continue;
                }

                b = this.ReadNBytes(1);
                if (b[0] != (byte)1)
                    continue;
                b = this.ReadNBytes(1);
                if (b[0] != (byte)1)
                    continue;
                break;
            }
        }
    }
}
