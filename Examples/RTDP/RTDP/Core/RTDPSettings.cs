using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Globalization;

namespace RTDP
{
    public static class RTDPSettings
    {
        public static bool sendHotspotAlarm;  // Send a HotSpotAlarm event for each detected hotpot
        public static bool doSegmentation;    // Run segmentation algorithm to detect hotspots
        public static bool doGeolocation;     // Geolocate
        public static bool doImageFusion;     // Fusionate thermal and visual images

        public static TerrainModelSettings terrainModelSettings;
        public static SegmentationSettings segmentationSettings;
        public static GeolocationSettings geolocationSettings;
        public static FusionSettings fusionSettings;
        public static IrImageSettings irImageSettings;
         
        static RTDPSettings()
        {
            sendHotspotAlarm = false;
            doSegmentation = false;
            doGeolocation = false;
            doImageFusion = false;

            terrainModelSettings = new TerrainModelSettings();
            segmentationSettings = new SegmentationSettings();
            geolocationSettings = new GeolocationSettings();
            fusionSettings = new FusionSettings();
            irImageSettings = new IrImageSettings();
        }

        static void Check()
        {
            if (sendHotspotAlarm || geolocationSettings.geolocateHotspots || fusionSettings.fusionMode == FusionSettings.FusionMode.Mark)
            {
                doSegmentation = true;
            }
            if (geolocationSettings.geolocateIRimage || geolocationSettings.geolocateVIimage || geolocationSettings.geolocateHotspots)
            {
                doGeolocation = true;
            }
        }

        static void ReadGlobalXML(XmlTextReader xmltextreader, string endName)
        {
            bool done = false;
            string element = "";

            while (xmltextreader.Read() && !done)
            {
                if (xmltextreader.NodeType == XmlNodeType.Element)
                {
                    element = xmltextreader.Name;
                }
                else if (xmltextreader.NodeType == XmlNodeType.Text)
                {
                    switch (element)
                    {
                        case "sendHotspotAlarm":
                            sendHotspotAlarm = bool.Parse(xmltextreader.Value);
                            break;
                        case "doSegmentation":
                            doSegmentation = bool.Parse(xmltextreader.Value);
                            break;
                        case "doGeolocation":
                            doGeolocation = bool.Parse(xmltextreader.Value);
                            break;
                        case "doImageFusion":
                            doImageFusion = bool.Parse(xmltextreader.Value);
                            break;
                    }
                }
                else if (xmltextreader.NodeType == XmlNodeType.EndElement)
                {
                    if (xmltextreader.Name == endName)
                    {
                        done = true;
                    }
                }
            }
        }

        public static void ReadXML()
        {
            ReadXML(Environment.CurrentDirectory + @"\..\..\XMLConfig\RTDPSettings.xml");
        }

        public static void ReadXML(string xmlName)
        {
            XmlTextReader xmltextreader = new XmlTextReader(xmlName);

            while (xmltextreader.Read())
            {
                if (xmltextreader.IsStartElement())
                {
                    // Get element name and switch on it.

                    switch (xmltextreader.Name)
                    {
                        case "GlobalSettings":
                            ReadGlobalXML(xmltextreader, "GlobalSettings");
                            break;
                        case "TerrainModelSettings":
                            terrainModelSettings.ReadXML(xmltextreader, "TerrainModelSettings");
                            break;
                        case "SegmentationSettings":
                            segmentationSettings.ReadXML(xmltextreader, "SegmentationSettings");
                            break;
                        case "GeolocationSettings":
                            geolocationSettings.ReadXML(xmltextreader, "GeneralSettings");
                            break;
                        case "FusionSettings":
                            fusionSettings.ReadXML(xmltextreader, "TerrainModelSettings");
                            break;
                        case "IrImageSettings":
                            irImageSettings.ReadXML(xmltextreader, "SegmentationSettings");
                            break;
                    }
                }
            }
            xmltextreader.Close();

            Check();
        }

    }
}
