using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace RTDP
{
    public abstract class RTDPImage
    {
        protected Bitmap bmp;             // Bitmap
        protected GeorefInfo georefInfo;  // Georeference data
    
        public abstract Bitmap BMP { get; }

        public abstract int Width { get; }

        public abstract int Height { get; }

        public GeorefInfo GeorefInfo
        {
            set { georefInfo = value; }
            get { return georefInfo; }
        }

        public void Dispose()
        {
            if (bmp != null)
                bmp.Dispose();
        }

        public void SaveJpeg(Stream stream)
        {
            SaveJpeg(stream, 80);
        }

        public void SaveJpeg(Stream stream, long quality)
        {
            if (BMP == null) return;

            // Encoder parameter for image quality
            if (quality < 0 || quality > 100)
                quality = 80;
            EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);

            // Jpeg image codec
            ImageCodecInfo jpegCodecInfo = getCodecInfo(ImageFormat.Jpeg);
            if (jpegCodecInfo == null)
                return;

            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;

            bmp.Save(stream, jpegCodecInfo, encoderParams);
        }

        public void SaveJpeg(string name)
        {
            SaveJpeg(name, 80);
        }

        public void SaveJpeg(string name, long quality)
        {
            if (BMP == null) return;

            // Encoder parameter for image quality
            if (quality < 0 || quality > 100)
                quality = 80;
            EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);

            // Jpeg image codec
            ImageCodecInfo jpegCodecInfo = getCodecInfo(ImageFormat.Jpeg);
            if (jpegCodecInfo == null)
                return;

            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;

            bmp.Save(name, jpegCodecInfo, encoderParams);
        }

        private ImageCodecInfo getCodecInfo(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                    return codec;
            }
            return null;
        }
    }
}
