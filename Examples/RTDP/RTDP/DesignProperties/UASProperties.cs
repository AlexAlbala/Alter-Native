using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using USAL;
using System.Globalization;

namespace RTDP
{
    public static class UASProperties
    {
        public static CAMProperties irCamProperties;
        public static CAMProperties viCamProperties;
        public static GPSProperties gpsProperties;

        static UASProperties()
        {
            irCamProperties = new CAMProperties();
            viCamProperties = new CAMProperties();
            gpsProperties = new GPSProperties();
        }

        public static void ReadXML()
        {
            ReadXML(Environment.CurrentDirectory + @"\..\..\XMLConfig\UASProperties.xml");
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
                        case "IRCamProperties":
                            irCamProperties.ReadXML(xmltextreader, "IRCamProperties");
                            break;
                        case "VICamProperties":
                            viCamProperties.ReadXML(xmltextreader, "VICamProperties");
                            break;
                        case "GPSProperties":
                            gpsProperties.ReadXML(xmltextreader, "GPSProperties");
                            break;
                    }
                }
            }

            xmltextreader.Close();
        }

    }
}
