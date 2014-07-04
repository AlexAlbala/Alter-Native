using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;

namespace RTDP
{
    // FLIR Public Format image structures

    public class FPF_IMAGE_DATA_T
    {
        public char[] fpfID = new char[32];
        public UInt32 version;          /*  = 1 */
        public UInt32 pixelOffset;      /*  Offset to pixel values from start of fpfID. */
        public UInt16 ImageType;        /*  Temperature = 0, 
                                            Diff Temp = 2, 
                                            Object Signal = 4,
                                            Diff Object Signal = 5, etc */
        public UInt16 pixelFormat;      /*  0 = short integer = 2 bytes
                                            1 = long integer = 4 bytes 
                                            2 = float (single precision)  = 4 bytes
                                            3 = double (double precision) = 8 bytes */
        public UInt16 xSize;
        public UInt16 ySize;
        public UInt32 trig_count;       /*  external trig counter */
        public UInt32 frame_count;      /*  frame number in sequence */
        public Int32[] sparelong = new Int32[16];  /* = 0 */

        public override string ToString()
        {

            string fpfIDText = new string (fpfID).Split('\0')[0];

            string ImageTypeText = (ImageType == 0) ? "temperature"
                : (ImageType == 2) ? "diff temperature"
                : (ImageType == 4) ? "object signal"
                : (ImageType == 5) ? "diff object signal"
                : "Unknown";

            string pixelFormatTxt = (pixelFormat == 0)? "short"
                :(pixelFormat == 1)? "long"
                 :(pixelFormat == 2)? "float"
                :(pixelFormat == 3)? "double"
                : "Unknown";

           return "ID: " + fpfIDText +
                "\nVersion: " + version +
                "\nPixel offset: " + pixelOffset +
                "\nImage type: " + ImageType + " (" + ImageTypeText + ")" +
                "\nPixel format: " + pixelFormat + " (" + pixelFormatTxt + ")" +
                "\nX size: " + xSize +
                "\nY size: " + ySize +
                "\nTrigger counter: " + trig_count +
                "\nFrame counter: " + frame_count + "\n";
        }
    }

    public class FPF_CAMDATA_T
    {
        public char[] camera_name = new char[32];
        public char[] camera_partn = new char[32];
        public char[] camera_sn = new char[32];
        public float camera_range_tmin;
        public float camera_range_tmax;
        public char[] lens_name = new char[32];
        public char[] lens_partn = new char[32];
        public char[] lens_sn = new char[32];
        public char[] filter_name = new char[32];
        public char[] filter_partn = new char[32];
        public char[] filter_sn = new char[32];
        public Int32[] sparelong = new Int32[16];/* = 0 */

        public override string ToString()
        {
            return "camera_name: " + new string(camera_name).Split('\0')[0] +
            "\ncamera_partn: " + new string(camera_partn).Split('\0')[0]  +
            "\ncamera_sn: " + new string(camera_sn).Split('\0')[0] +
            "\ncamera_range_tmin: " + camera_range_tmin.ToString("0") + "K (" + (camera_range_tmin - 273.15).ToString("0") + "C)" +
            "\ncamera_range_tmax: " + camera_range_tmax.ToString("0") + "K (" + (camera_range_tmax - 273.15).ToString("0") + "C)" +
            "\nvlens_name: " + new string(lens_name).Split('\0')[0] +
            "\nlens_partn: " + new string(lens_partn).Split('\0')[0] +
            "\nlens_sn: " + new string(lens_sn).Split('\0')[0] +
            "\nvfilter_name: " + new string(filter_name).Split('\0')[0] +
            "\nfilter_partn: " + new string(filter_partn).Split('\0')[0] +
            "\nfilter_sn: " + new string(filter_sn).Split('\0')[0] + "\n";
        }
    }

    public class FPF_OBJECT_PAR_T
    {
        public float emissivity;               /* 0 - 1 */
        public float objectDistance;           /* Meters */
        public float ambTemp;                  /* Reflected Ambient temperature in Kelvin */
        public float atmTemp;                  /* Atmospheric temperature in Kelvin */
        public float relHum;                   /* 0 - 1 */
        public float compuTao;                 /* Computed atmospheric transmission 0 - 1*/
        public float estimTao;                 /* Estimated atmospheric transmission 0 - 1*/
        public float refTemp;                  /* Reference temperature in Kelvin */
        public float extOptTemp;               /* Kelvin */
        public float extOptTrans;              /* 0 - 1 */
        public Int64[] sparelong = new Int64[16];/* = 0 */

        public override string ToString()
        {
            return "emissivity: " + emissivity +
                "\nobjectDistance: " + objectDistance +
                "\nambTemp: " + ambTemp +
                "\natmTemp: " + atmTemp +
                "\nrelHum: " + relHum +
                "\ncompuTao: " + compuTao +
                "\nestimTao: " + estimTao +
                "\nrefTemp: " + refTemp +
                "\nextOptTemp: " + extOptTemp +
                "\nextOptTrans: " + extOptTrans + "\n";
        }
    }

    public class FPF_DATETIME_T
    {
        public int Year;
        public int Month;
        public int Day;
        public int Hour;
        public int Minute;
        public int Second;
        public int MilliSecond;
        public Int32[] sparelong = new Int32[16];/* = 0 */

        public override string ToString()
        {
            return "Year: " + Year +
                "\nMonth: " + Month +
                "\nDay: " + Day +
                "\nHour: " + Hour +
                "\nMinute: " + Minute +
                "\nSecond: " + Second +
                "\nMilliSecond: " + MilliSecond + "\n";
        }

    }

    public class FPF_SCALING_T
    {
        public float tMinCam;                  /* Camera scale min, in current output */
        public float tMaxCam;                  /* Camera scale max */
        public float tMinCalc;                 /* Calculated min (almost true min) */
        public float tMaxCalc;                 /* Calculated max (almost true max) */
        public float tMinScale;                /* Scale min */
        public float tMaxScale;                /* Scale max */
        public Int32[] sparelong = new Int32[16];/* = 0 */

        public override string ToString()
        {
            return "tMinCam: " + tMinCam +
                "\ntMaxCam: " + tMaxCam +
                "\ntMinCalc: " + tMinCalc +
                "\ntMaxCalc: " + tMaxCalc +
                "\ntMinScale: " + tMinScale +
                "\ntMaxScale: " + tMaxScale + "\n";
        }
    }

    public class FPFHeader
    {
        public FPF_IMAGE_DATA_T imgData = new FPF_IMAGE_DATA_T();
        public FPF_CAMDATA_T camData = new FPF_CAMDATA_T();
        public FPF_OBJECT_PAR_T objPar = new FPF_OBJECT_PAR_T();
        public FPF_DATETIME_T datetime = new FPF_DATETIME_T();
        public FPF_SCALING_T scaling = new FPF_SCALING_T();
        public Int32[] sparelong = new Int32[32]; /* = 0 */

        public void readFPFHeader(BinaryReader reader)
        {
            imgData.fpfID = reader.ReadChars(32);
            imgData.version = reader.ReadUInt32();
            imgData.pixelOffset = reader.ReadUInt32();
            imgData.ImageType = reader.ReadUInt16();
            imgData.pixelFormat = reader.ReadUInt16();
            imgData.xSize = reader.ReadUInt16();
            imgData.ySize = reader.ReadUInt16();
            imgData.trig_count = reader.ReadUInt32();
            imgData.frame_count = reader.ReadUInt32();
            reader.ReadBytes(16 * 4);

            camData.camera_name = reader.ReadChars(32);
            camData.camera_partn = reader.ReadChars(32);
            camData.camera_sn = reader.ReadChars(32);
            camData.camera_range_tmin = reader.ReadSingle();
            camData.camera_range_tmax = reader.ReadSingle();
            camData.lens_name = reader.ReadChars(32);
            camData.lens_partn = reader.ReadChars(32);
            camData.lens_sn = reader.ReadChars(32);
            camData.filter_name = reader.ReadChars(32);
            camData.filter_partn = reader.ReadChars(32);
            camData.filter_sn = reader.ReadChars(32);
            reader.ReadBytes(16 * 4);

            objPar.emissivity = reader.ReadSingle();
            objPar.objectDistance = reader.ReadSingle();
            objPar.ambTemp = reader.ReadSingle();
            objPar.atmTemp = reader.ReadSingle();
            objPar.relHum = reader.ReadSingle();
            objPar.compuTao = reader.ReadSingle();
            objPar.estimTao = reader.ReadSingle();
            objPar.refTemp = reader.ReadSingle();
            objPar.extOptTemp = reader.ReadSingle();
            objPar.extOptTrans = reader.ReadSingle();
            reader.ReadBytes(16 * 4);

            datetime.Year = reader.ReadInt32();
            datetime.Month = reader.ReadInt32();
            datetime.Day = reader.ReadInt32();
            datetime.Hour = reader.ReadInt32();
            datetime.Minute = reader.ReadInt32();
            datetime.Second = reader.ReadInt32();
            datetime.MilliSecond = reader.ReadInt32();
            reader.ReadBytes(16 * 4);

            scaling.tMinCam = reader.ReadSingle();
            scaling.tMaxCam = reader.ReadSingle();
            scaling.tMinCalc = reader.ReadSingle();
            scaling.tMaxCalc = reader.ReadSingle();
            scaling.tMinScale = reader.ReadSingle();
            scaling.tMaxScale = reader.ReadSingle();
            reader.ReadBytes(16 * 4);

            reader.ReadBytes(32 * 4);
        }

        public override string  ToString()
        {
            return "IMAGE DATA\n" + imgData.ToString() 
                + "\nCAMERA DATA\n" + camData.ToString() 
                + "\nOBJECT\n" + objPar.ToString() 
                + "\nDATE/TIME\n" + datetime.ToString() 
                + "\nSCALING\n" + scaling.ToString();
        }
    }

    public class FPFImage : RTDPImage
    {
        FPFHeader header;       // fpf image header 
        float[] fpf_data;		// fpf image matrix data 
        float range_tmin;	    // min possible fpf temp in data 
        float range_tmax;	    // max possible fpf temp in data 
        float data_tmin;        // min existent fpf temp in data 
        float data_tmax;        // max existent fpf temp in data 
        int x_size;   	        // image horizontal size in pixels
        int y_size;   	        // image vertical size in pixels
        ArrayList sat_list;	    // list of saturated/zero pixels in fpf_data 	

        public FPFHeader Header
        {
            get { return header; }
        }

        public override int Width
        {
            get { return x_size; }
        }

        public override int Height
        {
            get { return y_size; }
        }

        public float[] FPFData
        {
            get { return fpf_data; }
        }

        public float RangeTmin
        {
            get { return range_tmin; }
        }

        public float RangeTmax
        {
            get { return range_tmax; }
        }

        public float DataTmin
        {
            get { return data_tmin; }
        }

        public float DataTmax
        {
            get { return data_tmax; }
        }

         public override Bitmap BMP
        {
            get
            {
                if (bmp == null)
                    CreateBitmap();

                return bmp;
            }
        }
         
        public float GetPixelTemp(int m, int n)
        {
            return fpf_data[m * x_size + n];
        }

        public FPFImage(string fpfName)
        {
            ReadFPFImage(fpfName);
        }

         public void ReadFPFImage(string fpfName)
        {
 
            if (File.Exists(fpfName))
            {
                BinaryReader reader = new BinaryReader(File.Open(fpfName, FileMode.Open));

                try
                {
                    // Read FPF header
                    header = new FPFHeader();
                    header.readFPFHeader(reader);

                    //Set image size
                    x_size = header.imgData.xSize;
                    y_size = header.imgData.ySize;

                    // Set image temperature range
                    range_tmin = header.camData.camera_range_tmin;
                    range_tmax = header.camData.camera_range_tmax;

                    // Read FPF image data 

                    if (header.imgData.ImageType != 0 || header.imgData.pixelFormat != 2)
                        //throw new Exception("Format not supported yet!");
                        return;

                    fpf_data = new float[x_size * y_size];
                    sat_list = new ArrayList();
                    data_tmin = float.MaxValue;
                    data_tmax = float.MinValue;
                    for (int i = 0; i < x_size * y_size; i++)
                    {
                        fpf_data[i] = reader.ReadSingle();
                        if (((int)fpf_data[i]) == 0) sat_list.Add(i); // saturated pixel!!
                        else if (fpf_data[i] < data_tmin) data_tmin = fpf_data[i];
                        else if (fpf_data[i] > data_tmax) data_tmax = fpf_data[i];
                    }    
                    if (sat_list.Count > 0)
                    {
                        data_tmax++;
                        foreach (int i in sat_list)
                        {
                            fpf_data[i] = data_tmax;
                        }
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
        }

        public void CreateBitmap()
        {
            bmp = new Bitmap(x_size, y_size, PixelFormat.Format24bppRgb);

            float scale_tmin, scale_tmax;
            if (RTDPSettings.irImageSettings.irScaleMode == IrImageSettings.IrScale.CameraRange)
            {
                scale_tmin = range_tmin;
                scale_tmax = range_tmax;
            }
            else if (RTDPSettings.irImageSettings.irScaleMode == IrImageSettings.IrScale.DataRange)
            {
                scale_tmin = data_tmin;
                scale_tmax = data_tmax;
            }
            else
            {
                scale_tmin = RTDPSettings.irImageSettings.irScaleMin;
                scale_tmax = RTDPSettings.irImageSettings.irScaleMax;
            }
            IrGradient irGradient = new IrGradient(RTDPSettings.irImageSettings.irGradient, scale_tmin, scale_tmax);
            
            BitmapData bmp_data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
            unsafe
            {
                byte*  rgbptr = (byte*)(bmp_data.Scan0);
                for (int y = 0, i = 0; y < bmp.Height; y++)
                {
                    for (int x = 0; x < bmp.Width; x++, i++)
                    {
                        irGradient.Temp2BGR(fpf_data[i], ref (*rgbptr++), ref (*rgbptr++), ref (*rgbptr++));  // RED
                    }
                    rgbptr += bmp_data.Stride - bmp_data.Width * 3;
                }
            }
            bmp.UnlockBits(bmp_data);
        }
    }
}
