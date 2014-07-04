using System;
using System.Collections.Generic;
using System.Text;

namespace USAL
{
    [Serializable]
    public class Waypoint
    {
        /// <summary>UNKNOWN</summary>
        public id_wp id;
        /// <summary>North-South direction (rad) Range[0,6.2832)</summary>
        public double latitude;
        /// <summary>East-West direction (rad) Range[0,6.2832)</summary>
        public double longitude;
        /// <summary>Down-Up direction (m) Range[UAV Range]</summary>
        public float altitude;
        /// <summary>UNKNOWN</summary>
        public float speed;
        /// <summary>UNKNOWN</summary>
        public bool fly_over;

        public Waypoint()
        {
        }

        /// <summary>WGS84 Inertial reference frame ECEF waypoint</summary>
        /// <param name="id">UNKNOWN</param>
        /// <param name="lat">North-South direction (rad) Range[0,6.2832)</param>
        /// <param name="lon">East-West direction (rad) Range[0,6.2832)</param>
        /// <param name="alt">Down-Up direction (m) Range[UAV Range]</param>
        /// <param name="spd">UNKNOWN</param>
        public Waypoint(id_wp id, double lat, double lon, float alt, float spd)
        {
            this.id = id;
            this.latitude = lat;
            this.longitude = lon;
            this.altitude = alt;
            this.speed = spd;

        }

    }
}
