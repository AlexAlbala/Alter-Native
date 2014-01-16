using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.SPOT.Exceptions
{

    [Serializable()]
    public class HardwareProvidedNotRegistered : System.Exception
    {
        public HardwareProvidedNotRegistered() : base() { }
        public HardwareProvidedNotRegistered(string message) : base(message) { }
        public HardwareProvidedNotRegistered(string message, System.Exception inner) : base(message, inner) { }

        // A constructor is needed for serialization when an 
        // exception propagates from a remoting server to the client.  
        protected HardwareProvidedNotRegistered(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) { }
    }
}
