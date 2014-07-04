using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace RTDP
{
    public class Metadata
    {
        public Telemetry uavTelemetry;
        public CameraDelay camDelay;

        public float geoidHeight;
        public float demElevation;
        public float AGL;

        public Metadata()
        {
            uavTelemetry = new Telemetry();
            camDelay = new CameraDelay();
            AGL = 0;
        }

        public void SetHeights(float localGeoidHeight, float elevation)
        {
            geoidHeight = localGeoidHeight;
            demElevation = elevation;
            uavTelemetry.uavPosition.Press_Altitude = uavTelemetry.uavPosition.Altitude - localGeoidHeight;
            AGL = uavTelemetry.uavPosition.Press_Altitude - elevation;
        }

        public void ReadXML(string xmlFile)
        {
            XmlTextReader xmltextreader = new XmlTextReader(xmlFile);

            while (xmltextreader.Read())
            {
                if (xmltextreader.NodeType == XmlNodeType.Element)
                {
                    switch (xmltextreader.Name)
                    {
                        case "Telemetry": 
                            uavTelemetry.ReadXML(xmltextreader, "Telemetry");

                            break;
                        case "Delay":
                            camDelay.ReadXML(xmltextreader, "Delay");
                            break;
                    }
                }
            }
            xmltextreader.Close();
        }
    }
}
