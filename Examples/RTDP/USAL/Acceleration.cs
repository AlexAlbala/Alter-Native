using System;
using System.Collections.Generic;
using System.Text;

namespace USAL
{
    [Serializable]
    public class Acceleration
    {
        /// <summary>+East-West direction (m/s2) Range[UAV Range]</summary>
        public float X;
        /// <summary>+North-South direction (m/s2) Range[UAV Range]</summary>
        public float Y;
        /// <summary>+Down-Up direction (m/s2) Range[UAV Range]</summary>
        public float Z;

        public Acceleration()
        {
        }

        /// <summary>Non-inertial reference frame NED acceleration (normal to the mean sea level surface)</summary>
        /// <param name="X">+East-West direction (m/s2) Range[UAV Range]</param>
        /// <param name="Y">+North-South direction (m/s2) Range[UAV Range]</param>
        /// <param name="Z">+Down-Up direction (m/s2) Range[UAV Range]</param>
        public Acceleration(float X, float Y, float Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;

        }
        public string getuavAcceleration()
        {
            return (this.X + " " + this.Y + " " + this.Z);
        }

    }
}
