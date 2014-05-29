using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using Custom.Math;

namespace RayTracer2010
{
    public class Sampler23NearestUnscaled : Sampler
    {
        public Bitmap bmp;

        public Sampler23NearestUnscaled(string name)
        {
            bmp = new Bitmap(name);
        }

        public override vect3d sample(vect3d pos)
        {
            // find modulo value (texture wrapping)
            vect2d p = new vect2d(pos.x % bmp.Width, (bmp.Height - pos.y) % bmp.Height);
            if (p.x < 0)
                p.x += bmp.Width;
            if (p.y < 0)
                p.y += bmp.Height;

            // get color out of bmp
            Color color = bmp.GetPixel((int)p.x, (int)p.y);

            return new vect3d(color.R / 255.0, color.G / 255.0, color.B / 255.0);
        }
    }
}
