using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Globalization;

namespace RTDP
{
    public class SegmentationSettings
    {
        public float hotspotThreshold; // Temperature threshold (k) for hotspot detection 

        public SegmentationSettings()
        {
            hotspotThreshold = 330;
        }

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
                        case "hotspotThreshold":
                            hotspotThreshold = float.Parse(xmltextreader.Value, NumberStyles.Float, CultureInfo.InvariantCulture);
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
