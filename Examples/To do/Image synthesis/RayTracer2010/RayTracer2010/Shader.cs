using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Custom.Math;

namespace RayTracer2010
{
    public class Shader
    {
        public virtual void colorize(Ray r)
        {
            r.color = new vect3d(1, 1, 1);
        }

        protected void lightDirectPointSpec(Ray r, double specular_coeff, ref vect3d diff_accum, ref vect3d spec_accum)
        {
            Ray shadow_ray = new Ray();
            shadow_ray.pos = r.hit_pos;

            foreach (var l in r.scene.lights)
            {
                vect3d dir = l.pos - r.hit_pos;
                double dist2 = dir.length2();
                double dist2_inv = 1.0 / dist2;
                dir *= Math.Sqrt(dist2_inv);

                // calculate incidences to do early reject of shadow ray
                double incidence_dif = (dir * r.hit_norm);
                vect3d dir_r = r.hit_norm * ((dir * r.hit_norm) * 2) - dir;
                double incidence_spec = (dir_r * r.hit_norm);

                if (incidence_dif < 0 && incidence_spec < 0)
                    continue;

                // shadow ray test
                shadow_ray.dir = dir;
                shadow_ray.hit_dist = Double.MaxValue;
                r.scene.trace(shadow_ray);
                if (shadow_ray.hit_dist * shadow_ray.hit_dist < dist2)
                    continue;

                // diffuse
                if (incidence_dif > 0)
                    diff_accum += l.color * incidence_dif * dist2_inv;

                // specular highlight
                if (incidence_spec > 0)
                    spec_accum += l.color * Math.Pow(incidence_spec, specular_coeff) * dist2_inv;
            }
        }

        protected void lightDirectPointSpecNoShadow(Ray r, double specular_coeff, ref vect3d diff_accum, ref vect3d spec_accum)
        {
            Ray shadow_ray = new Ray();
            shadow_ray.pos = r.hit_pos;

            foreach (var l in r.scene.lights)
            {
                vect3d dir = l.pos - r.hit_pos;
                double dist2 = 1.0 / dir.length2();
                dir *= Math.Sqrt(dist2);

                // diffuse
                double incidence_dif = (dir * r.hit_norm);
                if (incidence_dif > 0)
                    diff_accum += l.color * incidence_dif * dist2;

                // specular highlight
                vect3d dir_r = r.hit_norm * ((dir * r.hit_norm) * 2) - dir;
                double incidence_spec = (dir_r * r.hit_norm);
                if (incidence_spec > 0)
                    spec_accum += l.color * Math.Pow(incidence_spec, specular_coeff) * dist2;
            }
        }
    }
}
