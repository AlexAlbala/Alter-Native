using System;
using System.Runtime.InteropServices;

namespace GroundStation
{
	public class SpiWrapper
	{
		[DllImport("spi.so", EntryPoint="OpenSpiIn")]
		static extern int _OpenSpiIn ();
		[DllImport("spi.so", EntryPoint="OpenSpiOut")]	
		static extern int _OpenSpiOut ();
		[DllImport("spi.so", EntryPoint="ReadPort")]
		static extern char _ReadPort (int fd);
		[DllImport("spi.so", EntryPoint="WritePort")]
		static extern int _WritePort (int fd, char n);
		
		public SpiWrapper ()
		{ }
		
		public int OpenSpiIn()
		{
			return _OpenSpiIn();
		}
		
		public int OpenSpiOut()
		{
			return _OpenSpiOut();
		}
		
		public char ReadPort(int fd)
		{
			return _ReadPort(fd);
		}
		
		public int WritePort(int fd, char c)
		{
			return _WritePort(fd, c);
		}
	}
}

