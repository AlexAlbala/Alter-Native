using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemManager
{
    /// <summary>
    /// This class defines the base methods that every single waypoint definition.
    /// </summary>
    /// <remarks>
    /// This class is mandatory because thre is not an only geodetic system. If you have
    /// a DEM refered to a different geoid you have to implement some conversion routines.
    /// </remarks>
    public abstract class Point
    {
        public enum refSystem { WGS84, HAYFORD };
        /// <summary>
        /// Waypoint longitude.
        /// </summary>
        protected double longitude;

        /// <summary>
        /// Waypoint latitude.
        /// </summary>
        protected double latitude;

        /// <summary>
        /// Waypoint altitude.
        /// </summary>
        protected Nullable<double> altitude;

        /// <summary>
        /// Waypoint UTM projection X coordinate.
        /// </summary>
        protected double utmX;

        /// <summary>
        /// Waypoint UTM projection Y coordinate.
        /// </summary>
        protected double utmY;

        /// <summary>
        /// Time altitudeone where the waypoint is.
        /// </summary>
        protected int timeZone;
		
		protected char hemisphere;

        /// <summary>
        /// Default constructor. Do NOT use it.
        /// </summary>
        public Point()
        {
            this.longitude = 0;
            this.latitude = 0;
            this.altitude = null;
            this.utmX = 0;
            this.utmY = 0;
            this.timeZone = 0;
			this.hemisphere = 'N';
        }

        /// <summary>
        /// Construct Point class from geografic coordinates.
        /// </summary>
        /// <param name="latitude">Waypoint latitude.</param>
        /// <param name="longitude">Waypoint longitude.</param>
        /// <param name="altitude">Waypoint altitude.</param>
        /// <param name="PRC">Geoid's Polar Radius of Curvature.</param>
        /// <param name="SSE">Geoid's Squared Second Eccentricity.</param>
        public Point(double latitude, double longitude, Nullable<double> altitude,
            double PRC, double SSE)
        {
            this.latitude = latitude;
            this.longitude = longitude;
            this.altitude = altitude;
			if (this.latitude > 0)
				this.hemisphere = 'N';
			else 
				this.hemisphere = 'S';
            toUTM(PRC, SSE);
            this.timeZone = (int)(latitude / 60.0 + 31);
        }

        /// <summary>
        /// Construct Point class from projected coordinates.
        /// You need to know the reference meridian (timeZone).
        /// </summary>
        /// <param name="utmX">Waypoint UTM projected, X coordinate.</param>
        /// <param name="utmY">Waypoint UTM projected, Y coordinate.</param>
        /// <param name="altitude">Waypoint altitude</param>
        /// <param name="timeZone">Waypoint time zone</param>
        /// <param name="PRC">Geoid's Polar Radius of Curvature.</param>
        /// <param name="SSE">Geoid's Squared Second Eccentricity.</param>
        /// <param name="hemisphere">The referred hemmisphere: "N" for northern hemisphere, otherwise "S"</param>
        public Point(double utmX, double utmY, Nullable<double> altitude, int timeZone,
            double PRC, double SSE, char hemisphere)
        {
            this.utmX = utmX;
            this.utmY = utmY;
            this.timeZone = timeZone;
            this.altitude = altitude;
			this.hemisphere = hemisphere;
            toGEO(PRC, SSE, hemisphere);
        }

        //Note that there are no set methods because of encapsulation issues.
        /// <summary>
        /// Returns waypoint longitude.
        /// </summary>
        /// <returns>The waypoint longitude.</returns>
        public double getLongitude()
        {
            return this.longitude;
        }

        /// <summary>
        /// Returns waypoint latitude.
        /// </summary>
        /// <returns>The waypoint latitude.</returns>
        public double getLatitude()
        {
            return this.latitude;
        }

        /// <summary>
        /// Returns waypoint projection, X coordinate.
        /// </summary>
        /// <returns>The waypoint projection, X coordinate.</returns>
        public double getUtmX()
        {
            return this.utmX;
        }

        /// <summary>
        /// Returns waypoint projection, Y coordinate.
        /// </summary>
        /// <returns>The waypoint projection, Y coordinate.</returns>
        public double getUtmY()
        {
            return this.utmY;
        }
		
		public char GetHemisphere()
		{
			return this.hemisphere;
		}

        /// <summary>
        /// Returns waypoint altitude. An exeption is thrown if altitude points to null.
        /// </summary>
        /// <returns>The waypoint altitude.</returns>
        public Nullable<double> getAltitude()
        {
            return this.altitude;
        }

        /// <summary>
        /// Returns the waypoint time zone.
        /// </summary>
        /// <returns>The waypoint time zone.</returns>
        public int getTimeZone()
        {
            return this.timeZone;
        }

        /// <summary>
        /// Geoid Conversion from Waypoint origin geoid to WGS84 geoid.
        /// </summary>
        /// <returns>WgsPoint converted waypoint</returns>
        public abstract WgsPoint toWgs();

        /// <summary>
        /// Geoid Conversion from WGS84 origin geoid to current waypoint geoid.
        /// </summary>
        /// <param name="p">The object waypoint.</param>
        /// <returns>Converted waypoint.</returns>
        public abstract Point fromWgs(WgsPoint p);

        /// <summary>
        /// Obtain projected UTM coordinates.
        /// </summary>
        /// <param name="PRC">Geoid's Polar Radius of Curvature.</param>
        /// <param name="SSE">Geoid's Squared Second Eccentricity.</param>
        private void toUTM(double PRC, double SSE)
        {
            double lat = this.latitude * Math.PI / 180.0;
            double lon = this.longitude * Math.PI / 180.0;
            double lamda0 = Convert.ToInt32(Math.Floor(this.longitude / 6.0) + 31) * 6 - 183;
            lamda0 = lamda0 * Math.PI / 180.0;
            double deltaLamda = lon - lamda0;
            double a = Math.Cos(lat) * Math.Sin(deltaLamda);
            double psi = 0.5 * Math.Log((1 + a) / (1 - a));
            double nu = Math.Atan(Math.Tan(lat) / Math.Cos(deltaLamda)) - lat;
            double v = PRC * 0.9996 / Math.Sqrt(1 +
                SSE * Math.Pow(Math.Cos(lat), 2));
            double xi = SSE * Math.Pow(psi, 2) *
                Math.Pow(Math.Cos(lat), 2) / 2.0;
            double a1 = Math.Sin(2 * lat);
            double a2 = a1 * Math.Pow(Math.Cos(lat), 2);
            double j2 = lat + a1 / 2.0;
            double j4 = (3 * j2 + a2) / 4.0;
            double j6 = (5 * j4 + a2 * Math.Pow(Math.Cos(lat), 2)) / 3.0;
            double alpha = 0.75 * SSE;
            double beta = 1.6666666666666666666666666666667 * Math.Pow(alpha, 2);
            double gamma = 1.2962962962963 * Math.Pow(alpha, 3);
            double B = 0.9996 * PRC * (lat - alpha * j2 + beta
                * j4 - gamma * j6);
            this.utmX = psi * v * (1 + xi / 3.0) + 500000.0;
            this.utmY = nu * v * (1 + xi) + B;
            if (this.latitude < 0)
                this.utmY += 10000000;
        }

        /// <summary>
        /// Obtain geographic coordinates.
        /// </summary>
        /// <param name="PRC">Geoid's Polar Radius of Curvature.</param>
        /// <param name="SSE">Geoid's Squared Second Eccentricity.</param>
        private void toGEO(double PRC, double SSE, char hemisphere)
        {
            double xR = this.utmX - 500000.0;
            double yR = this.utmY;
            if (hemisphere == 'S')
                yR = yR - 10000000;
            int lambda0 = timeZone * 6 - 183;
            double phi = yR / (6366197.724 * 0.9996);
            double v = (PRC * 0.9996) /
                Math.Sqrt((1 + SSE *
                Math.Pow(Math.Cos(phi), 2)));
            double a = xR / v;
            double a1 = Math.Sin(2.0 * phi);
            double a2 = a1 * Math.Pow(Math.Cos(phi), 2);
            double j2 = phi + a1 / 2.0;
            double j4 = (3.0 * j2 + a2) / 4.0;
            double j6 = (5.0 * j4 + a2 * Math.Pow(Math.Cos(phi), 2))
                / 3.0;
            double alpha = 0.75 * SSE;
            double beta = 1.6666666667 * Math.Pow(alpha, 2);
            double gamma = 1.2962962962963 * Math.Pow(alpha, 3);
            double B = 0.9996 * PRC *
                (phi - alpha * j2 + beta * j4 - gamma * j6);
            double b = (yR - B) / v;
            double psi = (SSE *
                Math.Pow(a, 2) * Math.Pow(Math.Cos(phi), 2)) / 2.0;
            double shi = a * (1 - psi / 3.0);
            double nu = b * (1 - psi) + phi;
            double deltalamnda = Math.Atan(Math.Sinh(shi) / Math.Cos(nu));
            double tau = Math.Atan(Math.Cos(deltalamnda) * Math.Tan(nu));
            this.longitude = lambda0 + deltalamnda * 180.0 / Math.PI;
            this.latitude = (phi + (1 + SSE
                * Math.Pow(Math.Cos(phi), 2) - 1.5 *
                SSE * Math.Sin(phi) *
                Math.Cos(phi) * (tau - phi)) * (tau - phi));
            this.latitude = this.latitude * 180.0 / Math.PI;

        }
    }
}



