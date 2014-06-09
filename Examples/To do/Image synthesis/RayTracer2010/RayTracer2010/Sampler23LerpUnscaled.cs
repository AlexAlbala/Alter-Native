using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using Custom.Math;

namespace RayTracer2010
{
    class Sampler23LerpUnscaled : Sampler
    {
        public Bitmap bmp;

        public Sampler23LerpUnscaled(string name)
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
            int i = (int)p.x, j = (int)p.y;
            int ip = (i + 1) % bmp.Width, jp = (j + 1) % bmp.Height;
            double dx = p.x - i, dy = p.y - j;

            Color color;

            color = bmp.GetPixel(i, j);
            vect3d c00 = new vect3d(color.R / 255.0, color.G / 255.0, color.B / 255.0);

            color = bmp.GetPixel(i, jp);
            vect3d c01 = new vect3d(color.R / 255.0, color.G / 255.0, color.B / 255.0);

            color = bmp.GetPixel(ip, j);
            vect3d c10 = new vect3d(color.R / 255.0, color.G / 255.0, color.B / 255.0);

            color = bmp.GetPixel(ip, jp);
            vect3d c11 = new vect3d(color.R / 255.0, color.G / 255.0, color.B / 255.0);

            // combine samples with lerp
            return (1 - dx) * (1 - dy) * c00 + (1 - dx) * dy * c01 + dx * (1 - dy) * c10 + dx * dy * c11;
        }
    }
}
