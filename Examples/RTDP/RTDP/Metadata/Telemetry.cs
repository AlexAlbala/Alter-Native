using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using USAL;
using System.Xml;
using System.Globalization;

namespace RTDP
{
    public class Telemetry
    {
        public Angles uavAngles;
        public Position uavPosition;
        public Acceleration uavAcceleration;
        public Speed uavSpeed;

        public Telemetry()
        {
            uavAngles = new Angles();
            uavPosition = new Position();
            uavAcceleration = new Acceleration();
            uavSpeed = new Speed();
        }

        public void ReadXML(XmlTextReader xmltextreader, string endName)
        {
            string aux = "";

            while (xmltextreader.Read() && aux != endName)
            {
                if (xmltextreader.NodeType == XmlNodeType.EndElement || xmltextreader.NodeType == XmlNodeType.Element)
                {
                    aux = xmltextreader.Name;
                }
                else if (xmltextreader.NodeType == XmlNodeType.Text)
                {
                    switch (aux)
                    {
                        case "Latitude":
                            uavPosition.Latitude = double.Parse(xmltextreader.Value, NumberStyles.Float, CultureInfo.InvariantCulture);
                            break;
                        case "Longitude":
                            uavPosition.Longitude = double.Parse(xmltextreader.Value, NumberStyles.Float, CultureInfo.InvariantCulture);
                            break;
                        case "Altitude":
                            uavPosition.Altitude = float.Parse(xmltextreader.Value, NumberStyles.Float, CultureInfo.InvariantCulture);
                            break;
                        case "Roll":
                            uavAngles.Roll = float.Parse(xmltextreader.Value, NumberStyles.Float, CultureInfo.InvariantCulture);
                            break;
                        case "Pitch":
                            uavAngles.Pitch = float.Parse(xmltextreader.Value, NumberStyles.Float, CultureInfo.InvariantCulture);
                            break;
                        case "Yaw":
                            uavAngles.Yaw = float.Parse(xmltextreader.Value, NumberStyles.Float, CultureInfo.InvariantCulture);
                            break;
                        case "X":
                            uavAcceleration.X = float.Parse(xmltextreader.Value, NumberStyles.Float, CultureInfo.InvariantCulture);
                            break;
                        case "Y":
                            uavAcceleration.Y = float.Parse(xmltextreader.Value, NumberStyles.Float, CultureInfo.InvariantCulture);
                            break;
                        case "Z":
                            uavAcceleration.Z = float.Parse(xmltextreader.Value, NumberStyles.Float, CultureInfo.InvariantCulture);
                            break;
                        case "INDSpeed":
                            uavSpeed.Ind_speed = float.Parse(xmltextreader.Value, NumberStyles.Float, CultureInfo.InvariantCulture);
                            break;
                        case "TRUESpeed":
                            uavSpeed.True_speed = float.Parse(xmltextreader.Value, NumberStyles.Float, CultureInfo.InvariantCulture);
                            break;
                        case "North":
                            uavSpeed.North = float.Parse(xmltextreader.Value, NumberStyles.Float, CultureInfo.InvariantCulture);
                            break;
                        case "East":
                            uavSpeed.East = float.Parse(xmltextreader.Value, NumberStyles.Float, CultureInfo.InvariantCulture);
                            break;
                        case "Down":
                            uavSpeed.Down = float.Parse(xmltextreader.Value, NumberStyles.Float, CultureInfo.InvariantCulture);
                            break;

                    }
                }
            }

        }
    }
}
