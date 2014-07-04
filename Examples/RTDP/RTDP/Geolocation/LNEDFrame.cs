using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using USAL;

namespace RTDP
{
    public class LNEDFrame
    {
        // This class provides support for Coordinate frames transformations
        // 
        // LLA:  Latidude-Longitude-Altidude
        // ECEF: Earth-Centered Earth-Fixed, with X pointing to the 0ºlat 0ºlon, Y pointing to the 0ºlat 90ºlon, Z pointing to the north (90ºlat)
        // LNED: North-East-Down with origin at the GPS antenna
        // BODY: origin at the vehicle center of mass, X pointing to the nose, Y to the right wing, and Z to the belly
        //
        // Units are meters for distances and radians for angles

        // Origin of the LocalNED frame
        Position gpsLLA;
        Vector3  gpsECEF;
        Vector3  gpsBODY;

        // Aircraft navigation angles
        Angles uavAngles;

        // Settings
        bool zeroUavPitchRoll;  // Assume UAV pitch and roll angles are zero
        bool zeroGpsDistance;   // Assume GPS antenna distance to the UAV center of mass is zero

        // Rotation matrices
        Matrix3 R_ECEF_LNED;    // ECEF to LNED
        Matrix3 R_LNED_ECEF;    // LNED to ECEF
        Matrix3 R_LNED_BODY;    // LNED to BODY
        Matrix3 R_BODY_LNED;    // BODY to LNED

        double cosLat,   sinLat;
        double cosLon,   sinLon;
        double cosYaw,   sinYaw;
        double cosPitch, sinPitch;
        double cosRoll,  sinRoll;

        // Datum parameters for a WGS84 geodetic model
        const double a = 6378137;                       // WGS84 semi-major earth axis (m)
        const double b = 6356752.31424518;              // WGS84 semi-major earth axis (m)
        const double f = (a - b) / a;                   // flattening
        const double e2 = f * (2 - f);                  // first eccentricity squared
        const double ep2 = (a * a - b * b) / (b * b);   // second eccentricity squared
        double N;                                       // radius of curvature 
        float localGeoidHeight;                         // geoid height in the area to scan
        
        // Get properties
        public Position GpsLLA
        {
            get { return gpsLLA; }
        }

        // Constructor
        public LNEDFrame(GPSProperties gps)
        {
            localGeoidHeight = RTDPSettings.terrainModelSettings.localGeoidHeight;
            zeroGpsDistance = RTDPSettings.geolocationSettings.zeroGpsDistance;
            zeroUavPitchRoll = RTDPSettings.geolocationSettings.zeroUavPitchRoll;

            gpsBODY = gps.gpsPosition;
        }

        public void Init(Telemetry tel)
        {
            // Do not move N computation below, N is used by LLAtoECEF method!
            gpsLLA = tel.uavPosition;
            gpsLLA.Press_Altitude = gpsLLA.Altitude - localGeoidHeight;

            N = a / Math.Sqrt(1 - e2 * Math.Pow(Math.Sin(gpsLLA.Latitude), 2));
            cosLat = Math.Cos(gpsLLA.Latitude);
            sinLat = Math.Sin(gpsLLA.Latitude);
            cosLon = Math.Cos(gpsLLA.Longitude);
            sinLon = Math.Sin(gpsLLA.Longitude);

            gpsECEF = LLAtoECEF(gpsLLA);

            uavAngles = tel.uavAngles;
            cosYaw = Math.Cos(uavAngles.Yaw);
            sinYaw = Math.Sin(uavAngles.Yaw);
            if (zeroUavPitchRoll)
            {
                cosPitch = cosRoll = 1;
                sinPitch = sinRoll = 0;
            }
            else
            {
                cosPitch = Math.Cos(uavAngles.Pitch);
                sinPitch = Math.Sin(uavAngles.Pitch);
                cosRoll = Math.Cos(uavAngles.Roll);
                sinRoll = Math.Sin(uavAngles.Roll);
            }

            R_ECEF_LNED = null;
            R_LNED_ECEF = null;
            R_LNED_BODY = null;
            R_BODY_LNED = null;
        }

        // LLA to ECEF
        public Vector3 LLAtoECEF(Position LLA)
        {
            return new Vector3 ((N + LLA.Altitude) * Math.Cos(LLA.Latitude) * Math.Cos(LLA.Longitude),
                                (N + LLA.Altitude) * Math.Cos(LLA.Latitude) * Math.Sin(LLA.Longitude),
                                (N * (1 - e2) + LLA.Altitude) * Math.Sin(LLA.Latitude));
        }

        // ECEF to LLA
        public Position ECEFtoLLA(Vector3 ECEF)
        {
            double p = Math.Sqrt(ECEF.x * ECEF.x + ECEF.y * ECEF.y);
            double g = Math.Atan2(ECEF.z * a, p * b);

            Position LLA = new Position();
            LLA.Latitude  = Math.Atan2(ECEF.z + ep2 * b * Math.Pow(Math.Sin(g), 3), p - e2 * a * Math.Pow(Math.Cos(g), 3));
            LLA.Longitude = Math.Atan2(ECEF.y, ECEF.x);
            LLA.Altitude  = Convert.ToSingle(p / Math.Cos(LLA.Latitude) - N);
            LLA.Press_Altitude = LLA.Altitude - localGeoidHeight;
            return (LLA);
        }

        // ECEF to LNED
        public Vector3 ECEFtoLNED(Vector3 ECEF)
        {
            if (R_ECEF_LNED == null)
            {
                R_ECEF_LNED = new Matrix3(-sinLat * cosLon, -sinLat * sinLon,  cosLat,
                                                   -sinLon,           cosLon,       0,
                                          -cosLat * cosLon, -cosLat * sinLon, -sinLat);
            }

            return (R_ECEF_LNED * (ECEF - gpsECEF));
        }

        // LNED to ECEF
        public Vector3 LNEDtoECEF(Vector3 LNED)
        {
            if (R_LNED_ECEF == null)
            {
                R_LNED_ECEF = new Matrix3(-sinLat * cosLon,  sinLat * sinLon, -cosLat,
                                                    sinLon,           cosLon,       0,
                                           cosLat * cosLon, -cosLat * sinLon, -sinLat);
            }
            return (R_LNED_ECEF * LNED + gpsECEF);
        }

        // LLA to LNED
        public Vector3 LLAtoLNED(Position LLA)
        {
            return ECEFtoLNED(LLAtoECEF(LLA));
        }

        // LNED to LLA
        public Position LNEDtoLLA(Vector3 LNED)
        {
            return ECEFtoLLA(LNEDtoECEF(LNED));
        }

        // LNED to BODY
        public Vector3 LNEDtoBODY(Vector3 LNED)
        {
            if (zeroUavPitchRoll && zeroGpsDistance)
            {
                return new Vector3 (cosYaw * LNED.x + sinYaw * LNED.y,
                                    -sinYaw * LNED.x + cosYaw * LNED.y,
                                    LNED.z);
            }
            else if (zeroUavPitchRoll)
            {
                return new Vector3 (cosYaw * LNED.x + sinYaw * LNED.y + gpsBODY.x,
                                    -sinYaw * LNED.x + cosYaw * LNED.y + gpsBODY.y,
                                    LNED.z + gpsBODY.z);
            }
            else
            {
                if (R_LNED_BODY == null)
                {
                    R_LNED_BODY = new Matrix3(cosPitch * cosYaw,
                                             cosPitch * sinYaw,
                                            -sinPitch,
                                             sinRoll * sinPitch * cosYaw - cosRoll * sinYaw,
                                             sinRoll * sinPitch * sinYaw + cosRoll * cosYaw,
                                             sinRoll * cosPitch,
                                             cosRoll * sinPitch * cosYaw + sinRoll * sinYaw,
                                             cosRoll * sinPitch * sinYaw - sinRoll * cosYaw,
                                             cosRoll * cosPitch);
                }
                if (zeroGpsDistance) 
                    return (R_LNED_BODY * LNED);
                else
                    return (R_LNED_BODY * LNED + gpsBODY);
            }
        } 

        // BODY to LNED
        public Vector3 BODYtoLNED(Vector3 BODY)
        {
            if (zeroUavPitchRoll && zeroGpsDistance)
            {
                return new Vector3 (cosYaw * BODY.x - sinYaw * BODY.y,
                                    sinYaw * BODY.x + cosYaw * BODY.y,
                                    BODY.z);
            }
            else if (zeroUavPitchRoll)
            {
                return new Vector3 (cosYaw * (BODY.x - gpsBODY.x) - sinYaw * (BODY.y - gpsBODY.y),
                                    sinYaw * (BODY.x - gpsBODY.x) + cosYaw * (BODY.y - gpsBODY.y),
                                    BODY.z - gpsBODY.z);
            }
            else
            {
                if (R_BODY_LNED == null)
                {
                    R_BODY_LNED = new Matrix3( cosYaw * cosPitch,
                                              -sinYaw * cosRoll + cosYaw * sinPitch * sinRoll,
                                               sinYaw * sinRoll + cosYaw * sinPitch * cosRoll,
                                               sinYaw * cosPitch,
                                               cosYaw * cosRoll + sinYaw * sinPitch * sinRoll,
                                              -cosYaw * sinRoll + sinYaw * sinPitch * cosRoll,
                                              -sinPitch,
                                               cosPitch * sinRoll,
                                               cosPitch * cosRoll);
                }
                if (zeroGpsDistance)
                    return (R_BODY_LNED * BODY);
                else
                    return (R_BODY_LNED * (BODY - gpsBODY));
            }
        }
    }
}
