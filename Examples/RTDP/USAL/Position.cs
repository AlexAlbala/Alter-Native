using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace USAL
{
    [Serializable]
    public class Position
    {
        // Angles range changed by Esther
        /// <summary>North-South angle from the equator, Range[-1.5708(S),1.5708(N)] (rad)</summary>
        public double Latitude;
        /// <summary>East-West angle from the Prime (Greenwich), Meridian Range[-3.1416(W),3.1416(E)] (rad)</summary>
        public double Longitude;
        /// <summary>Down-Up direction (m), Range[UAV Range]</summary>
        public float Altitude;
        /// <summary>(m) Range[UAV Range]</summary>
        public float Press_Altitude; //In Flight Gear Simulator Press_Altitude will be ground level altitude
        
        // Note: GPS gives altitude above the WGS84 ellipsoide. Pressure altitude is above the actual sea level. 
        // The difference can be up to 100m. In Europe the WGS84 ellipsoid is around 30m above sea level.
        // In EPSC it is around 49m above sea level
        
        public Position()
        {
        }

        public Position(Position position)
        {
            this.Latitude = position.Latitude;
            this.Longitude = position.Longitude;
            this.Altitude = position.Altitude;
            this.Press_Altitude = position.Press_Altitude;
        }

        public Position(double Latitude, double Longitude, float Altitude, float Press_Altitude)
        {
            this.Latitude = Latitude;
            this.Longitude = Longitude;
            this.Altitude = Altitude;
            this.Press_Altitude = Press_Altitude;
        }

        public string getuavPosition()
        {
            return (this.Latitude + " " + this.Longitude + " " + this.Altitude + " " + this.Press_Altitude);
        }

        public string ToStringDMS()
        {
            double deg, min, sec;
            string lat = " N", lon = " E";

            deg = this.Latitude * 180 / Math.PI;
            if (deg < 0)
            {
                deg = -deg;
                lat = " S";
            }
            min = deg - (int)deg;
            deg -= min;
            min *= 60.0;
            sec = min - (int)min;
            min -= sec;
            sec *= 60.0;
            string str = deg.ToString("0") + "\x00B0" + min.ToString("0") + "'" + sec.ToString("0.00", CultureInfo.InvariantCulture.NumberFormat) + "\"" + lat;

            deg = this.Longitude * 180 / Math.PI;
            if (deg < 0)
            {
                deg = -deg;
                lon = " W";
            }
            min = deg - (int)deg;
            deg -= min;
            min *= 60.0;
            sec = min - (int)min;
            min -= sec;
            sec *= 60.0;
            str += "  " + deg.ToString("0") + "\x00B0" + min.ToString("0") + "'" + sec.ToString("0.00", CultureInfo.InvariantCulture.NumberFormat) + "\"" + lon;

            str += "  " + this.Altitude.ToString("0.00", CultureInfo.InvariantCulture.NumberFormat) + " m";

            str += " / " + this.Press_Altitude.ToString("0.00", CultureInfo.InvariantCulture.NumberFormat) + " m";

            return str;
        }

    }
}
