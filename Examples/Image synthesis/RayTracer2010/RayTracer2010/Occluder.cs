using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Custom.Math;

namespace RayTracer2010
{
    public class Occluder
    {
        public Shader shader;

        public virtual void trace(Ray r)
        {
        }

        public virtual box3d bounds()
        {
            box3d b = new box3d();
            b.xh = b.yh = b.zh = Double.MaxValue;
            b.xl = b.yl = b.zl = Double.MinValue;
            return b;
        }

        public virtual bool intersects_box(box3d b)
        {
            return true;
        }

        public virtual void check_crossing(double pos, int a, out bool cross_l, out bool cross_r)
        {
            cross_l = cross_r = true;
        }
    }
}
