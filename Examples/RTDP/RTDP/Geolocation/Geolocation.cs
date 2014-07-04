using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using USAL;
using System.Globalization;
using System.Collections;
using System.IO;

using System.Threading;

namespace RTDP
{
    public class Geolocation
    {
        private CAMProperties camera;
        private GPSProperties gps;
        
        LNEDFrame nedFrame;
        CAMFrame camFrame;
        Vector3 cameraNED;
        GeorefInfo georefInfo;

        public Geolocation(CAMProperties camera, GPSProperties gps)
        {
            this.camera = camera;
            this.gps = gps;

            this.nedFrame = new LNEDFrame(gps);
            this.camFrame = new CAMFrame(camera);
        }

        public void Init(Telemetry telemetry)
        {
            this.nedFrame.Init(telemetry);
            this.cameraNED = nedFrame.BODYtoLNED(camFrame.CamBODY);
            this.georefInfo = null;
        }

        public double GetDemAltitude(Position lla)
        {
            return (double)DemManager.DemManagerService.GetInstance().FunctionCall("GetAltitude", new object[] { lla });
        }

        // Get LLA coordinates of pixel [m,n] (m:row, n:column)
        public Position GetPixelGeolocation(int m, int n)
        {
            Position targetLLA = new Position(nedFrame.GpsLLA);
            Vector3  targetNED = new Vector3();

            ArrayList historic_dem = new ArrayList();
            int index, i = 0;

            if (RTDPSettings.geolocationSettings.zeroCamDistance)
            {
                Vector3 pixelNED = nedFrame.BODYtoLNED(camFrame.PIXELtoBODY(m, n));

                double aux_h = nedFrame.GpsLLA.Press_Altitude;
                double aux_x = pixelNED.x / pixelNED.z;
                double aux_y = pixelNED.y / pixelNED.z;
                double dem_alt, target_alt= 0;

                do
                {
                    dem_alt = GetDemAltitude(targetLLA);
                    index = historic_dem.IndexOf(dem_alt);
                    if (index == -1)
                    {
                        target_alt = dem_alt;
                        targetNED.z = aux_h - target_alt;
                        targetNED.x = targetNED.z * aux_x;
                        targetNED.y = targetNED.z * aux_y;
                        targetLLA = nedFrame.LNEDtoLLA(targetNED);
                        historic_dem.Add(dem_alt);
                        i++;   
                    }
                    else if (index != historic_dem.Count - 1)
                    {
                        double sum_dem = 0;
                        for (int j = index; j < historic_dem.Count; j++)
                            sum_dem += (double)historic_dem[j];
                        target_alt = sum_dem / (historic_dem.Count - index);
                        targetNED.z = aux_h - target_alt;
                        targetNED.x = targetNED.z * aux_x;
                        targetNED.y = targetNED.z * aux_y;
                        targetLLA = nedFrame.LNEDtoLLA(targetNED);
                        i++;   
                   }
                }
                while ((index == -1) && (i < RTDPSettings.geolocationSettings.maxIterations));
            }
            else
            {
                Vector3 pixelNED = nedFrame.BODYtoLNED(camFrame.PIXELtoBODY(m, n));

                double aux_h = nedFrame.GpsLLA.Press_Altitude;
                double aux_x = (pixelNED.x - cameraNED.x) / (pixelNED.z - cameraNED.z);
                double aux_y = (pixelNED.y - cameraNED.y) / (pixelNED.z - cameraNED.z);
                double dem_alt, target_alt= 0;

                do
                {
                    dem_alt = GetDemAltitude(targetLLA);
                    index = historic_dem.IndexOf(dem_alt);
                    if (index == -1)
                    {
                        target_alt = dem_alt;
                        targetNED.z = aux_h - target_alt;
                        targetNED.x = cameraNED.x + (targetNED.z - cameraNED.z) * aux_x;
                        targetNED.y = cameraNED.y + (targetNED.z - cameraNED.z) * aux_y;
                        targetLLA = nedFrame.LNEDtoLLA(targetNED);
                        historic_dem.Add(dem_alt);
                        i++;   
                    }
                    else if (index != historic_dem.Count - 1)
                    {
                        double sum_dem = 0;
                        for (int j = index; j < historic_dem.Count; j++)
                            sum_dem += (double)historic_dem[j];
                        target_alt = sum_dem / (historic_dem.Count - index);
                        targetNED.z = aux_h - target_alt;
                        targetNED.x = cameraNED.x + (targetNED.z - cameraNED.z) * aux_x;
                        targetNED.y = cameraNED.y + (targetNED.z - cameraNED.z) * aux_y;
                        targetLLA = nedFrame.LNEDtoLLA(targetNED);
                        i++;   
                    }
                }
                while ((index == -1) && (i < RTDPSettings.geolocationSettings.maxIterations));
            }
            return targetLLA;
        }

        public GeorefInfo GetImageGeolocation()
        {
            if (georefInfo == null)
            {
                georefInfo = new GeorefInfo();
                georefInfo.up_left_corner = GetPixelGeolocation(0, 0);
                georefInfo.up_right_corner = GetPixelGeolocation(0, camera.imageWidth - 1);
                georefInfo.bottom_left_corner = GetPixelGeolocation(camera.imageHeight - 1, 0);
                georefInfo.bottom_right_corner = GetPixelGeolocation(camera.imageHeight - 1, camera.imageWidth - 1);
                georefInfo.center = GetPixelGeolocation(camera.imageHeight / 2, camera.imageWidth / 2);
            }

            return georefInfo;
        }
    }
}
