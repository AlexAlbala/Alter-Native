using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RTDP
{
    public class ViImage : RTDPImage
    {
        public override Bitmap BMP
        {
            get { return bmp; }
        }

        public override int Width
        {
            get { return bmp.Width; }
        }

        public override int Height
        {
            get { return bmp.Height; }
        }

        public ViImage(string name)
        {
            bmp = new Bitmap(name);
        }

    }
}
