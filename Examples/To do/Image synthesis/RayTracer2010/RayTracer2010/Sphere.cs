using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Custom.Math;

namespace RayTracer2010
{
    public class Sphere : Occluder
    {
        public Sphere(vect3d pos, double r, Shader shader)
        {
            this.shader = shader;
            this.radius = r;
            this.radius2 = r * r;
            this.pos = pos;
        }
        vect3d pos;
        double radius, radius2;

        public override void trace(Ray r)
        {
            vect3d dst = r.pos - this.pos;
            double B = dst * r.dir;
            double C = dst.length2() - this.radius2;
            double D = B * B - C;
            if (D > 0)
            {
                double dist;
                double flip_norm = 1;
                if (r.is_outside)
                    dist = -B - Math.Sqrt(D);
                else
                {
                    dist = -B + Math.Sqrt(D);
                    flip_norm = -1;
                }

                if (dist > 0 && dist < r.hit_dist)
                {
                    r.hit_dist = dist;
                    r.hit_pos = r.pos + r.dir*dist;
                    r.hit_norm = (r.hit_pos - this.pos) * flip_norm;
                    r.hit_norm.normalize();
                    r.hit_tex_coord = r.hit_norm;
                    r.hit_object = this;
                }
            }
        }
    }
}
