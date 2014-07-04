using System;
using System.Collections.Generic;
using System.Text;

namespace USAL
{
    [Serializable]
    public class Angles
    {
        /// <summary>Roll=0 looking perpendicular to the mean sea (rad) Range[0,6.2832)</summary>
        public float Roll;
        /// <summary>Pitch=0 looking NED horizont (rad) Range[0,6.2832)</summary>
        public float Pitch;
        /// <summary>Yaw=0 looking north (rad) Range[0,6.2832)</summary>
        public float Yaw;

        public Angles()
        {
        }

        /// <summary>NED attitude (normal to the mean sea level surface)</summary>
        /// <param name="Roll">(rad) Range[0,6.2832)</param>
        /// <param name="Pitch">(rad) Range[0,6.2832)</param>
        /// <param name="Yaw">(rad) Range[0,6.2832)</param>
        public Angles(float Roll, float Pitch, float Yaw)
        {
            this.Roll = Roll;
            this.Pitch = Pitch;
            this.Yaw = Yaw;

        }
        public string getuavAngles()
        {
            return (this.Roll + " " + this.Pitch + " " + this.Yaw);
        }


    }
}
