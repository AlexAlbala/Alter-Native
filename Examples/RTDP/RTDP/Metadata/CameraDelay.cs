using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Globalization;

namespace RTDP
{
    public class CameraDelay
    {
        // Package identifier
        public string id;
        // Average delay of visual camera (s)
        public double viAverage;
        // Standard Desviation of the visual camera delay (s)
        public double viStdDev;
        // Delay of the thermal camera (s)
        public double irDelay;


        public void ReadXML(XmlTextReader xmltextreader, string endName)
        {
            string aux = "";

            while (xmltextreader.Read() && aux != endName)
            {
                if (xmltextreader.NodeType == XmlNodeType.EndElement)
                {
                    aux = xmltextreader.Name;
                }
                else if (xmltextreader.NodeType == XmlNodeType.Element)
                {
                    aux = xmltextreader.Name;
                    if (aux.StartsWith("ID_"))
                        id = aux.Substring(3);
                }
                else if (xmltextreader.NodeType == XmlNodeType.Text)
                {
                    switch (aux)
                    {
                        case "Average":
                            viAverage = double.Parse(xmltextreader.Value, NumberStyles.Float, CultureInfo.InvariantCulture) / 1000.0;
                            break;
                        case "StdDev":
                            viStdDev = double.Parse(xmltextreader.Value, NumberStyles.Float, CultureInfo.InvariantCulture) / 1000.0;
                            break;
                        case "Thermal":
                            irDelay = double.Parse(xmltextreader.Value, NumberStyles.Float, CultureInfo.InvariantCulture) / 1000.0;
                            break;
                    }
                }
            }
        }
    }
}
