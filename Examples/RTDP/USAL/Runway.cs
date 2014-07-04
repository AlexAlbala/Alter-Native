using System;
using System.Collections.Generic;
using System.Text;

namespace USAL
{
    [Serializable]
    public class Runway
    {
        /// <summary>UNKNOWN</summary>
        public string AirportIdentifier;
        /// <summary>UNKNOWN</summary>
        public string RunwayNumber;
        /// <summary>North-South direction (rad) Range[0,6.2832)</summary>
        public double Latitude;
        /// <summary>East-West direction (rad) Range[0,6.2832)</summary>
        public double Longitude;
        /// <summary>Down-Up direction (m) Range[UAV Range]</summary>
        public float Altitude;
        /// <summary>UNKNOWN</summary>
        public float Heading;
        /// <summary>UNKNOWN</summary>
        public float Lenght;


        public Runway()
        {
        }

        /// <summary>Runway Batch Data</summary>
        /// <param name="AirportIdentifier">UNKNOWN</param>
        /// <param name="RunwayNumber">UNKNOWN</param>
        /// <param name="Latitude">North-South direction (rad) Range[0,6.2832)</param>
        /// <param name="Longitude">East-West direction (rad) Range[0,6.2832)</param>
        /// <param name="Altitude">Down-Up direction (m) Range[UAV Range]</param>
        /// <param name="Heading">UNKNOWN</param>
        /// <param name="Lenght">UNKNOWN</param>
        public Runway(string AirportIdentifier, string RunwayNumber, double Latitude, double Longitude, float Altitude, float Heading, float Lenght)
        {
            this.AirportIdentifier = AirportIdentifier;
            this.RunwayNumber = RunwayNumber;
            this.Latitude = Latitude;
            this.Longitude = Longitude;
            this.Altitude = Altitude;
            this.Heading = Heading;
            this.Lenght = Lenght;
        }
    }
}
