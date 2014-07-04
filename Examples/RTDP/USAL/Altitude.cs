using System;
using System.Collections.Generic;
using System.Text;

namespace USAL
{
    [Serializable]
    public class Altitude
    {
        /// <summary>UNKNOWN</summary>
        public float trueAltitude;

        public Altitude()
        {
        }

        /// <summary>Altitude</summary>
        /// <param name="altitude">UNKNOWN</param>
        public Altitude(float altitude)
        {
            this.trueAltitude = altitude;
        }
    }
}
