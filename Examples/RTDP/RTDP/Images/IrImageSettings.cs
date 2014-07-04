using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Globalization;

namespace RTDP
{
    public class IrImageSettings
    {
        public enum IrScale
        {
            CameraRange,
            DataRange,
            UserRange
        };

        public Gradient.Theme irGradient; // Gradient colors (gray, orange, rainbow, ...)
        public IrScale irScaleMode;    // IR bitmap scale: camera range, data range or user defined
        public float irScaleMin;     // Minimum temperature (k) in color gradient (for user defined scale mode)
        public float irScaleMax;     // Maximum temperature (k) in color gradient (for user defined scale mode)

        public IrImageSettings()
        {
            irGradient = Gradient.Theme.Rainbow;
            irScaleMode = IrScale.DataRange;
            irScaleMin = 273.15F;
            irScaleMax = 373.15F;
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
                        case "irGradient":
                            if (xmltextreader.Value == "gray")
                                irGradient = Gradient.Theme.Gray;
                            if (xmltextreader.Value == "orange")
                                irGradient = Gradient.Theme.Orange;
                            else if (xmltextreader.Value == "rainbow")
                                irGradient = Gradient.Theme.Rainbow;
                            break;
                        case "irScaleMode":
                            if (xmltextreader.Value == "camera")
                                irScaleMode = IrScale.CameraRange;
                            else if (xmltextreader.Value == "data")
                                irScaleMode = IrScale.DataRange;
                            else if (xmltextreader.Value == "user")
                                irScaleMode = IrScale.UserRange;
                            break;
                        case "irScaleMin":
                            irScaleMin = float.Parse(xmltextreader.Value, NumberStyles.Float, CultureInfo.InvariantCulture);
                            break;
                        case "irScaleMax":
                            irScaleMax = float.Parse(xmltextreader.Value, NumberStyles.Float, CultureInfo.InvariantCulture);
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
