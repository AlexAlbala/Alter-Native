using System;
using System.Collections.Generic;
using System.Text;

namespace USAL
{
    [Serializable]
    public class VectorSpeed
    {
        /// <summary>+North-South direction (m/s) Range[UAV Range]</summary>
        public float North;
        /// <summary>+East-West direction (m/s) Range[UAV Range]</summary>
        public float East;
        /// <summary>+Down-Up direction (m/s) Range[UAV Range]</summary>
        public float Down;

        public VectorSpeed()
        {
        }

        /// <summary>Non-inertial reference frame NED speed (normal to the mean sea level surface)</summary>
        /// <param name="North">+North-South direction (m/s2) Range[UAV Range]</param>
        /// <param name="East">+East-West direction (m/s2) Range[UAV Range]</param>
        /// <param name="Down">+Down-Up direction (m/s2) Range[UAV Range]</param>
        public VectorSpeed(float North, float East, float Down)
        {
            this.North = North;
            this.East = East;
            this.Down = Down;
        }
        public string getuavSpeed()
        {
            return (this.North + " " + this.East + " " + this.Down);

        }

    }
}
