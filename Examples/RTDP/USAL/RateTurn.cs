using System;
using System.Collections.Generic;
using System.Text;

namespace USAL
{
    [Serializable]
    public class RateTurn
    { // Sistema levógiro XxY=-Z
        /// <summary>Counterclockwise North Axis rotation (rad/s) Range[UAV Range]</summary>
        public float X;
        /// <summary>Counterclockwise East Axis rotation (rad/s) Range[UAV Range]</summary>
        public float Y;
        /// <summary>Counterclockwise Down Axis rotation (rad/s) Range[UAV Range]</summary>
        public float Z;

        public RateTurn()
        {
        }

        /// <summary>Non-inertial reference frame NED rate of turn (normal to the mean sea level surface)</summary>
        /// <param name="X">Counterclockwise North Axis rotation (rad/s) Range[UAV Range]</param>
        /// <param name="Y">Counterclockwise East Axis rotation (rad/s) Range[UAV Range]</param>
        /// <param name="Z">Counterclockwise Down Axis rotation (rad/s) Range[UAV Range]</param>
        public RateTurn(float X, float Y, float Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }

        public string getuavRateTurn()
        {
            return (this.X + " " + this.Y + " " + this.Z);

        }
    }
}
