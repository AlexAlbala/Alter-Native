using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace GroundStation
{
	/// <summary>
	/// Message class.
	/// </summary>
    public abstract class Message
    {
		/// <summary>
		/// A reference to the pid singleton instance
		/// </summary>
		protected PIDManager pid;
		protected GlobalArea ga;
		
		/// <summary>
		/// A timestamp  
		/// </summary>
        public double time;
		
		/// <summary>
		/// A stream write to write to a file
		/// </summary>
		protected StreamWriter sw;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="GroundStation.Message"/> class.
		/// </summary>
        public Message() { }
		
		/// <summary>
		/// Creates the message.
		/// </summary>
		/// <param name='b'>
		/// The messages as an array of bytes
		/// </param>
        public abstract void CreateMessage(ulong time, byte[] b);
		
		/// <summary>
		/// Creates the message.
		/// </summary>
		/// <param name='m'>
		/// The message as a string
		/// </param>
        public abstract void CreateMessage(string m);
    }
}
