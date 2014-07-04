using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DemManager;
using System.Xml;
using System.Globalization;

namespace RTDP
{
    public class TerrainModelSettings
    {
        public Dem.Precision demPrecision;  // "low" (SRTM-900m/cell), "medium" (SRTM-90m/cell) or "high" (ICC-30m/cell) DEM precision 
        public float localGeoidHeight;      // WGS84 geoid height in the area to scan

        public TerrainModelSettings()
        {
            demPrecision = Dem.Precision.medium;
            localGeoidHeight = 0;
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
                        case "demPrecision":
                            if (xmltextreader.Value == "low")
                                demPrecision = Dem.Precision.low;
                            else if (xmltextreader.Value == "medium")
                                demPrecision = Dem.Precision.medium;
                            else if (xmltextreader.Value == "high")
                                demPrecision = Dem.Precision.high;
                            break;
                        case "localGeoidHeight":
                            localGeoidHeight = float.Parse(xmltextreader.Value, NumberStyles.Float, CultureInfo.InvariantCulture);
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
