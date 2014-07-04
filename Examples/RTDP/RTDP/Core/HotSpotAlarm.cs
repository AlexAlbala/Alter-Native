using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RTDP
{
    // The RTDP service sends a RTDP.HotSpotAlarm event with this information
    // for each detected hotspot
    public class HotSpotAlarm
    {
        // Latitude of the hotspot center of mass (radians)
        public double CenterLatitude;
        
        // Longitude of the hotspot center of mass (radians)
        public double CenterLongitude;

        // Maximum temperature of the hotspot (kelvin)
        public float MaxTemperature;

        // Name of the generated fusion image file (null if none)
        public string FusionFileName;

        public HotSpotAlarm() { }

        // Constructor
        public HotSpotAlarm(double latitude, double longitude, float maxTemp, string fusionName)
        {
            this.CenterLatitude = latitude;
            this.CenterLongitude = longitude;
            this.MaxTemperature = maxTemp;
            this.FusionFileName = fusionName;
        }
    }
}