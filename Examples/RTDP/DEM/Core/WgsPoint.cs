using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemManager
{
    /// <summary>
    /// This class implements all needed methods to manage waypoints reffered to WGS84 geoid.
    /// </summary>
    public class WgsPoint : Point
    {
        /// <summary>
        /// Polar Radius of Curvature for WGS84 Geoid.
        /// </summary>
        private static readonly double PRC = 6399593.626;

        /// <summary>
        /// Squared Second Eccentricity for WGS84 Geoid.
        /// </summary>
        private static readonly double SSE = 0.006739497;

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public WgsPoint()
            : base()
        { }
        /// <summary>
        /// Construct WgsPoint class from projected coordinates.
        /// You need to know the reference meridian (timeZone).
        /// </summary>
        /// <param name="utmX">Waypoint UTM projected, X coordinate.</param>
        /// <param name="utmY">Waypoint UTM projected, Y coordinate.</param>
        /// <param name="altitude">Waypoint altitude</param>
        /// <param name="timeZone">Waypoint time zone</param>
        /// <param name="hemisphere">The referred hemmisphere: "N" for northern hemisphere, otherwise "S"</param>
        public WgsPoint(double utmX, double utmY, Nullable<double> altitude, int timeZone, char hemisphere)
            : base(utmX, utmY, altitude, timeZone, PRC, SSE, hemisphere)
        { }

        /// <summary>
        /// Construct WgsPoint class from geografic coordinates.
        /// </summary>
        /// <param name="latitude">Waypoint latitude.</param>
        /// <param name="longitude">Waypoint longitude.</param>
        /// <param name="altitude">Waypoint altitude.</param>
        public WgsPoint(double latitude, double longitude, Nullable<double> altitude)
            : base(latitude, longitude, altitude, PRC, SSE)
        { }

        /// <summary>
        /// Do nothing.
        /// </summary>
        /// <returns>The same waypoint.</returns>
        public override Point fromWgs(WgsPoint p)
        {
            return p;
        }

        /// <summary>
        /// Do nothing.
        /// </summary>
        /// <returns>The same waypoint.</returns>
        public override WgsPoint toWgs()
        {
            return this;
        }


    }
}
