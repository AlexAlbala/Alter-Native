using System;
using System.Collections.Generic;
using System.Text;

namespace USAL
{
    [Serializable]
    public class Speed
    {
        /// <summary>+North-South direction (m/s) Range[UAV Range]</summary>
        public float North;
        /// <summary>+East-West direction (m/s) Range[UAV Range]</summary>
        public float East;
        /// <summary>+Down-Up direction (m/s) Range[UAV Range]</summary>
        public float Down;

        /// <summary>IAS Indicated airspeed (m/s) Range[UAV Range]</summary>
        public float Ind_speed;
        /// <summary>TAS True airspeed (m/s) Range[UAV Range]</summary>
        public float True_speed;

        public Speed(float North, float East, float Down)
        {
            this.North = North;
            this.East = East;
            this.Down = Down;
        }

        /// <summary>Non-inertial reference frame NED speed (normal to the mean sea level surface)</summary>
        /// <param name="Y">+North-South direction (m/s) Range[UAV Range]</param>
        /// <param name="X">+East-West direction (m/s) Range[UAV Range]</param>
        /// <param name="Z">+Down-Up direction (m/s) Range[UAV Range]</param>
        public Speed()
        {

        }
        /// <summary>UAV speed</summary>
        /// <param name="Ind_speed">IAS Indicated airspeed (m/s) Range[UAV Range]</param>
        /// <param name="True_speed">TAS True airspeed (m/s) Range[UAV Range]</param>
        public Speed(float Ind_speed, float True_speed)
        {
            this.Ind_speed = Ind_speed;
            this.True_speed = True_speed;
        }


        public string getairspeed()
        {
            return (this.Ind_speed + " " + this.True_speed);
        }
    }
}
