using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Custom.Math;

namespace RayTracer2010
{
    class Plane : Occluder
    {
        public Plane(vect3d p0, vect3d p1, vect3d p2, Shader shader)
        {
            this.shader = shader;
            this.pos = p0;

            z = (p1 - p0) % (p2 - p0);
            z.normalize();

            x = p2 % z;
            x.normalize();

            y = z % x;
        }
        public vect3d pos, x, y, z;

        public override void trace(Ray r)
        {
            double v = r.dir * z;
            if (r.is_outside && v > 0 || !r.is_outside && v < 0)
                return;

            double dist = ((pos - r.pos) * z) / v;
            if (dist > 0 && dist < r.hit_dist)
            {
                r.hit_dist = dist;
                r.hit_pos = r.pos + r.dir * dist;
                vect3d rp = r.hit_pos - pos;
                r.hit_tex_coord = new vect3d(x * rp, y * rp, 0);
                r.hit_norm = z;
                r.hit_object = this;
            }
        }
    }
}
