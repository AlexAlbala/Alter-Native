using System;
using System.Collections.Generic;

using System.Text;

namespace GroundStation
{
    /// <summary>
    /// This class implements all needed methods to manage waypoints reffered to Hayford International 1924 geoid.
    /// </summary>
    public class HayPoint : Point
    {
        /// <summary>
        /// Polar Radius of Curvature for Hayford Geoid.
        /// </summary>
        private static readonly double PRC = 6399936.608;

        /// <summary>
        /// Squared Second Eccentricity for Hayford Geoid.
        /// </summary>
        private static readonly double SSE = 0.00676817;

        /// <summary>
        /// Hayford Geoid's major axis.
        /// </summary>
        private static readonly double hayMajorAxis = 6378388;

        /// <summary>
        /// Hayford Geoid's minor axis.
        /// </summary>
        private static readonly double hayMinorAxis = 6356911.94613;

        /// <summary>
        /// WGS84 Geoid's major axis.
        /// </summary>
        private static readonly double wgsMajorAxis = 6378137;

        /// <summary>
        /// WGS84 geoid's minor axis.
        /// </summary>
        private static readonly double wgsMinorAxis = 6356752.31414;

        /// <summary>
        /// Bursa-Wolf's first parameter (deltaX)
        /// </summary>
        private static readonly double dX = -131.032;

        /// <summary>
        /// Bursa-Wolf's second parameter (deltaY)
        /// </summary>
        private static readonly double dY = -100.251;

        /// <summary>
        /// Bursa-Wolf's third parameter (deltaZ)
        /// </summary>
        private static readonly double dZ = -163.354;

        /// <summary>
        /// Bursa-Wolf's fourth parameter (RX)
        /// </summary>
        private static readonly double rX = 0.0000060301;

        /// <summary>
        /// Bursa-Wolf's fifth parameter (RY)
        /// </summary>
        private static readonly double rY = 0.0000000945;

        /// <summary>
        /// Bursa-Wolf's sixth parameter (RZ)
        /// </summary>
        private static readonly double rZ = 0.0000055443;

        /// <summary>
        /// Bursa-Wolf's seventh parameter (Scale Factor)
        /// </summary>
        private static readonly double e = 0.00000939;

        /// <summary>
        /// Constructor.
        /// </summary>
        public HayPoint()
            : base()
        { }

        public HayPoint(WgsPoint p)
            : base()
        {
            HayPoint hayPoint = new HayPoint();
            hayPoint = (HayPoint)hayPoint.fromWgs(p);
            this.latitude = hayPoint.latitude;
            this.longitude = hayPoint.longitude;
            this.altitude = hayPoint.altitude;
            this.timeZone = hayPoint.timeZone;
            this.utmX = hayPoint.utmX;
            this.utmY = hayPoint.utmY;
        }


        /// <summary>
        /// Construct HayPoint class from projected coordinates.
        /// You need to know the reference meridian (timeZone).
        /// </summary>
        /// <param name="utmX">UTM projected coordinate X.</param>
        /// <param name="utmY">UTM projected coordinate Y.</param>
        /// <param name="altitude">Waypoint altitude.</param>
        /// <param name="timeZone">Waypoint time zone.</param>
        /// <param name="hemisphere">The referred hemmisphere: "N" for northern hemisphere, otherwise "S"</param>
        public HayPoint(double utmX, double utmY, Nullable<double> altitude, int timeZone, char hemisphere)
            : base(utmX, utmY, altitude, timeZone, PRC, SSE, hemisphere)
        { }

        /// <summary>
        /// Construct HayPoint class from geographic coordinates.
        /// </summary>
        /// <param name="latitude">Waypoint latitude.</param>
        /// <param name="longitude">Waypoint longitude.</param>
        /// <param name="altitude">Waypoint altitude.</param>
        public HayPoint(double latitude, double longitude, Nullable<double> altitude)
            : base(latitude, longitude, altitude, PRC, SSE)
        { }

        /// <summary>
        /// Geoid Conversion from Waypoint origin geoid to WGS84 geoid.
        /// </summary>
        /// <returns>WgsPoint converted waypoint</returns>
        public override WgsPoint toWgs()
        {
            double lat = this.latitude * Math.PI / 180.0;
            double lon = this.longitude * Math.PI / 180.0;
            double n = Math.Pow(hayMajorAxis, 2) /
            Math.Sqrt(
                Math.Pow(hayMajorAxis, 2) * Math.Pow(Math.Cos(lat), 2) +
                (Math.Pow(hayMinorAxis, 2) * Math.Pow(Math.Sin(lat), 2)));
            double z;
            if (this.altitude == null)
                z = 0;
            else
                z = (double)this.altitude;
            double x = (n + z) * Math.Cos(lat) * Math.Cos(lon);
            double y = (n + z) * Math.Cos(lat) * Math.Sin(lon);
            z = ((Math.Pow((hayMinorAxis / hayMajorAxis), 2)) * n + z)
                * Math.Sin(lat);

            double x_, y_, z_;

            x_ = dX + (1 + e) * (x + rZ * y - rY * z);
            y_ = dY + (1 + e) * (-rZ * x + y + rX * z);
            z_ = dZ + (1 + e) * (rY * x - rX * y + z);

            double p = Math.Sqrt(Math.Pow(x_, 2) + Math.Pow(y_, 2));
            double theta = Math.Atan((z_ * wgsMajorAxis)
                / (p * wgsMinorAxis));
            double pow_eccentricity = (Math.Pow(wgsMajorAxis, 2)
                - Math.Pow(wgsMinorAxis, 2))
                / Math.Pow(wgsMajorAxis, 2);
            double pow_second_eccentricity = (Math.Pow(wgsMajorAxis, 2)
                - Math.Pow(wgsMinorAxis, 2))
                / Math.Pow(wgsMinorAxis, 2);
            double latf = Math.Atan((z_ + pow_second_eccentricity
                * wgsMinorAxis * Math.Pow(Math.Sin(theta), 3))
                / (p - pow_eccentricity * wgsMajorAxis
                * Math.Pow(Math.Cos(theta), 3)));
            double lonf = Math.Atan(y_ / x_);
            double nf = Math.Pow(wgsMajorAxis, 2) /
                Math.Sqrt(
                    Math.Pow(wgsMajorAxis, 2) * Math.Pow(Math.Cos(latf), 2) +
                    Math.Pow(wgsMinorAxis, 2) * Math.Pow(Math.Sin(latf), 2));
            double hf = (p / Math.Cos(latf)) - nf;
            latf = latf * 180 / Math.PI;
            lonf = lonf * 180 / Math.PI;
            WgsPoint point = new WgsPoint(latf, lonf, hf);
            return point;
        }

        public override Point fromWgs(WgsPoint point)
        {
            double lat = point.getLatitude() * Math.PI / 180.0;
            double lon = point.getLongitude() * Math.PI / 180.0;
            double n = Math.Pow(wgsMajorAxis, 2) /
            Math.Sqrt(
                Math.Pow(wgsMajorAxis, 2) * Math.Pow(Math.Cos(lat), 2) +
                (Math.Pow(wgsMinorAxis, 2) * Math.Pow(Math.Sin(lat), 2)));
            double z;
            if (point.getAltitude() == null)
                z = 0;
            else
                z = (double)point.getAltitude();
            double x = (n + z) * Math.Cos(lat) * Math.Cos(lon);
            double y = (n + z) * Math.Cos(lat) * Math.Sin(lon);
            z = ((Math.Pow((wgsMinorAxis / wgsMajorAxis), 2)) * n + z)
                * Math.Sin(point.getLatitude() * Math.PI / 180.0);

            double x_, y_, z_;

            x_ = -dX + (1 + (-1) * e) * (x - rZ * y + rY * z);
            y_ = -dY + (1 + (-1) * e) * (rZ * x + y - rX * z);
            z_ = -dZ + (1 + (-1) * e) * (-rY * x + rX * y + z);

            double p = Math.Sqrt(Math.Pow(x_, 2) + Math.Pow(y_, 2));
            double theta = Math.Atan((z_ * hayMajorAxis)
                / (p * hayMinorAxis));
            double pow_eccentricity = (Math.Pow(hayMajorAxis, 2)
                - Math.Pow(hayMinorAxis, 2))
                / Math.Pow(hayMajorAxis, 2);
            double pow_second_eccentricity = (Math.Pow(hayMajorAxis, 2)
                - Math.Pow(hayMinorAxis, 2))
                / Math.Pow(hayMinorAxis, 2);
            double latf = Math.Atan((z_ + pow_second_eccentricity
                * hayMinorAxis * Math.Pow(Math.Sin(theta), 3))
                / (p - pow_eccentricity * hayMajorAxis
                * Math.Pow(Math.Cos(theta), 3)));
            double lonf = Math.Atan(y_ / x_);
            double nf = Math.Pow(hayMajorAxis, 2) /
                Math.Sqrt(
                    Math.Pow(hayMajorAxis, 2) * Math.Pow(Math.Cos(latf), 2) +
                    Math.Pow(hayMinorAxis, 2) * Math.Pow(Math.Sin(latf), 2));
            double hf = (p / Math.Cos(latf)) - nf;
            latf = latf * 180 / Math.PI;
            lonf = lonf * 180 / Math.PI;
            HayPoint _point = new HayPoint(latf, lonf, hf);
            return _point;
        }
    }
}
