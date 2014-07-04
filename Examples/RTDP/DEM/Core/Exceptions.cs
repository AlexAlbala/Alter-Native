using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemManager
{
    //-------------------------------------------------------------------------
    /// <summary>
    /// Exception class
    /// </summary>
    //-------------------------------------------------------------------------
    public class DemNotFoundException : System.ApplicationException
    {
        //---------------------------------------------------------------------
        /// <summary>
        /// Main constructor.
        /// </summary>
        //---------------------------------------------------------------------
        public DemNotFoundException() { }

        //---------------------------------------------------------------------
        /// <summary>
        /// Alternative constructor.
        /// </summary>
        /// <param name="message">
        /// An error message.
        /// </param>
        //---------------------------------------------------------------------
        public DemNotFoundException(string message) { }
    }
}
