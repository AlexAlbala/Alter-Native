using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace RTDP
{
    public class GeolocationSettings
    {
        public bool geolocateHotspots; // Geolocate hotspots in the IR image 
        public bool geolocateIRimage;  // Geolocate the IR image 
        public bool geolocateVIimage;  // Geolocate the visual image 
        public bool zeroUavPitchRoll;  // Assume UAV pitch and roll angles are zero
        public bool zeroCamPanRoll;    // Assume camera pan and roll angles are zero
        public bool zeroCamDistance;   // Assume camera distance to the UAV center of mass is zero
        public bool zeroGpsDistance;   // Assume GPS antenna distance to the UAV center of mass is zero
        public int maxIterations;     // Maximun number of iterations in the geolocation algorithm  

        public void ReadXML(XmlTextReader xmltextreader, string endName)
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
                        case "geolocateIRimage":
                            geolocateIRimage = bool.Parse(xmltextreader.Value);
                            break;
                        case "geolocateVIimage":
                            geolocateVIimage = bool.Parse(xmltextreader.Value);
                            break;
                        case "geolocateHotspots":
                            geolocateHotspots = bool.Parse(xmltextreader.Value);
                            break;
                        case "zeroUavPitchRoll":
                            zeroUavPitchRoll = bool.Parse(xmltextreader.Value);
                            break;
                        case "zeroCamPanRoll":
                            zeroCamPanRoll = bool.Parse(xmltextreader.Value);
                            break;
                        case "zeroCamDistance":
                            zeroCamDistance = bool.Parse(xmltextreader.Value);
                            break;
                        case "zeroGpsDistance":
                            zeroGpsDistance = bool.Parse(xmltextreader.Value);
                            break;
                        case "maxIterations":
                            maxIterations = int.Parse(xmltextreader.Value);
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

    }
}
