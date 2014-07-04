using System;
using System.Collections.Generic;
using System.Text;

namespace USAL
{
    [Serializable]
    public class Bearing
    {
        /// <summary>UNKNOWN</summary>
        public float bearing;

        public Bearing()
        { }

        public Bearing(float bearing)
        {
            this.bearing = bearing;
        }
    }
}
