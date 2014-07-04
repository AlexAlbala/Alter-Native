using System;
using System.Collections.Generic;
using System.Text;

namespace USAL
{
    [Serializable]
    class Formulation
    {
        public const double EARTH_RADIUS_FT = 20925646.3;   // Earth's equatorial radius, converted from meters.

        public NEDPosition RoundEarthLatLon_2_NED(double lat_rad, double lon_rad, double OrLat_rad, double OrLon_rad)
        {
            //     Formula:  posNorth_ft = EarthRadius_ft * (lat-lat_origin)
            //     Formula:  posEast_ft = EarthRadius_ft * cos(lat) * (lon-lon_origin)  

            NEDPosition NEDWp = new NEDPosition();
            NEDWp.posNorth = (EARTH_RADIUS_FT * (lat_rad - OrLat_rad))* 0.3048006096;
            NEDWp.posEast= (EARTH_RADIUS_FT * Math.Cos(lat_rad) * (lon_rad - OrLon_rad)) * 0.3048006096 ;

            return NEDWp;

        }

        public Position RoundEarthNE_2_LatLon(double posNorth_ft, double posEast_ft, double OrLat_rad, double OrLon_rad)
        {
            //     Formula:  lat = posNorth_ft / EarthRadius_ft + lat_origin
            //     Formula:  lon = (posEast_ft / (EarthRadius_ft * cos ( lat ))) + lon_origin

            Position ECEFWp = new Position();
            ECEFWp.Latitude = posNorth_ft / EARTH_RADIUS_FT + OrLat_rad;
            ECEFWp.Longitude = (posEast_ft / (EARTH_RADIUS_FT * Math.Cos(ECEFWp.Latitude)));

            return ECEFWp;
        }

    }
}
