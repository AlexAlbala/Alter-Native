using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace GroundStation
{
	/// <summary>
	/// File input. Lectura d'un fitxer a nivell de byte
	/// </summary>
    public class FileInput : Input
    {
		/// <summary>
		/// Reads the first N bytes from the stream
		/// </summary>
        public StreamReader sr;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="GroundStation.FileInput"/> class.
		/// </summary>
		/// <param name='path'>
		/// Path to the file.
		/// </param>
        public FileInput(string path)
        {
            this.sr = new StreamReader(path);
        }
		
		/// <summary>
		/// Reads the N bytes.
		/// </summary>
		/// <returns>
		/// The N bytes.
		/// </returns>
		/// <param name='n'>
		/// The number of bytes to be read.
		/// </param>
        public override byte[] ReadNBytes(int n)
        {
            byte[] ans = new byte[n];
            for (int i = 0; i < n; i++)
            {
                byte aux = (byte)this.sr.Read();
                ans[i] = (byte)aux;
            }
            return ans;
        }
    }
}
