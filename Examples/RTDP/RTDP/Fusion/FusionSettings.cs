using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace RTDP
{
    public class FusionSettings
    {
        public enum FusionMode
        {
            Merge,
            Mark
        }

        public FusionMode fusionMode;

        public FusionSettings()
        {
            fusionMode = FusionMode.Mark;
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
                        case "fusionMode":
                            if (xmltextreader.Value == "merge")
                                fusionMode = FusionMode.Merge;
                            else if (xmltextreader.Value == "mark")
                                fusionMode = FusionMode.Mark;
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
