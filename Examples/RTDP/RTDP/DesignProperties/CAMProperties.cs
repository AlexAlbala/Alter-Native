using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using USAL;
using System.Xml;
using System.Globalization;

namespace RTDP
{
    public class CAMProperties
    {
        public string cameraModel;          // A string to identify the camera

        // Internal parameters
        public int imageWidth;              // Horizontal resolution in pixels
        public int imageHeight;             // Vertical resolution in pixels
        public double pixelPitch;           // Distance between pixels in the sensor (m)
        public double focalLength;          // Focal length
        ////public double  horizontalFOV;   // Horizontal field of view in radians (in degrees in the xml)
        ////public double  verticalFOV;     // Vertical field of view in radians (in degrees in the xml)

        // External parameters
        public Vector3 camPosition;     // Coordinates of the optical centre in the body frame
        public Angles camAngles;        //   pan/yaw/azimut: 0-to the nose, 90-to the right wing
                                        //   tilt/pitch/elevation: 0-vertical down, 90-horizontal 
                                        //   roll

        public CAMProperties()
        {
            camPosition = new Vector3(); 
            camAngles = new Angles();    
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
                        case "cameraModel":
                            cameraModel = xmltextreader.Value;
                            break;
                        case "imageWidth":
                            imageWidth = int.Parse(xmltextreader.Value);
                            break;
                        case "imageHeight":
                            imageHeight = int.Parse(xmltextreader.Value);
                            break;
                        case "pixelPitch":
                            pixelPitch = double.Parse(xmltextreader.Value, NumberStyles.Float, CultureInfo.InvariantCulture);
                            break;
                        case "focalLength":
                            focalLength = double.Parse(xmltextreader.Value, NumberStyles.Float, CultureInfo.InvariantCulture);
                            break;
                        //case "horizontalFOV":
                        //    cam.horizontalFOV = double.Parse(xmltextreader.Value, NumberStyles.Float, CultureInfo.InvariantCulture) * Math.PI / 180;
                        //    break;
                        //case "verticalFOV":
                        //    cam.verticalFOV = double.Parse(xmltextreader.Value, NumberStyles.Float, CultureInfo.InvariantCulture) * Math.PI / 180;
                        //    break;
                        case "X":
                            camPosition.x = double.Parse(xmltextreader.Value, NumberStyles.Float, CultureInfo.InvariantCulture);
                            break;
                        case "Y":
                            camPosition.y = double.Parse(xmltextreader.Value, NumberStyles.Float, CultureInfo.InvariantCulture);
                            break;
                        case "Z":
                            camPosition.z = double.Parse(xmltextreader.Value, NumberStyles.Float, CultureInfo.InvariantCulture);
                            break;
                        case "Pan":
                            camAngles.Yaw = float.Parse(xmltextreader.Value, NumberStyles.Float, CultureInfo.InvariantCulture) * (float)Math.PI / 180;
                            break;
                        case "Tilt":
                            camAngles.Pitch = float.Parse(xmltextreader.Value, NumberStyles.Float, CultureInfo.InvariantCulture) * (float)Math.PI / 180;
                            break;
                        case "Roll":
                            camAngles.Roll = float.Parse(xmltextreader.Value, NumberStyles.Float, CultureInfo.InvariantCulture) * (float)Math.PI / 180;
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
