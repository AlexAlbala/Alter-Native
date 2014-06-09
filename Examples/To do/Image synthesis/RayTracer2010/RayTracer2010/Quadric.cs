using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Custom.Math;

namespace RayTracer2010
{
    public class Quadric : Occluder
    {
        public double A = 0, B = 0, C = 0, D = 0, E = 0, F = 0, G = 0, H = 0, I = 0, J = 0;

        public override void trace(Ray r)
        {
            vect3d o = r.pos, d = r.dir;
            double t = Double.MaxValue;

            double Aq = A * d.x * d.x + D * d.x * d.y + B * d.y * d.y + C * d.z * d.z + d.x * d.z * E + d.y * d.z * F;
            double Bq = d.x * G + d.y * H + d.z * I + 2 * A * d.x * o.x + D * d.y * o.x + d.z * E * o.x + D * d.x * o.y + 2 * B * d.y * o.y + d.z * F * o.y + 2 * C * d.z * o.z + d.x * E * o.z + d.y * F * o.z;
            double Cq = J + G * o.x + A * o.x * o.x + H * o.y + D * o.x * o.y + B * o.y * o.y + I * o.z + E * o.x * o.z + F * o.y * o.z + C * o.z * o.z;

            if (Aq == 0)
            {
                if (Bq != 0)
                    t = -Cq / Bq;
            }
            else
            {
                double Dq = Bq * Bq - 4 * Aq * Cq;
                if (Dq > 0)
                {
                    if (r.is_outside)
                        t = (-Bq - Math.Sqrt(Dq)) / (2 * Aq);
                    else
                        t = (-Bq + Math.Sqrt(Dq)) / (2 * Aq);
                }
            }

            if (t > 0 && t < r.hit_dist)
            {
                r.hit_dist = t;
                r.hit_object = this;
                
                vect3d p = r.pos + r.dir * t;
                r.hit_pos = p;
                r.hit_tex_coord = p;

                r.hit_norm = new vect3d(
                    2 * A * p.x + D * p.y + E * p.z + G, 
                    2 * B * p.y + D * p.x + F * p.z + H, 
                    2 * C * p.z + E * p.x + F * p.y + I);
                r.hit_norm.normalize();

                if (r.dir * r.hit_norm > 0)
                    r.hit_norm *= -1;
            }
        }
    }
}
