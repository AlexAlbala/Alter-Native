using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Custom.Math;

namespace RayTracer2010
{
    public class TextureSphereShader : Shader
    {
        Sampler tex;
        public vect3d specular_color = new vect3d(1, 1, 1);
        public double specular_coeff = 100;

        public TextureSphereShader(string fn)
        {
            tex = new Sampler23Nearest(fn);
        }

        public override void colorize(Ray r)
        {
            vect3d diff_accum = new vect3d(0, 0, 0);
            vect3d spec_accum = new vect3d(0, 0, 0);

            lightDirectPointSpec(r, specular_coeff, ref diff_accum, ref spec_accum);

            // calculate the polar coordinates
            vect3d sp;
            sp.x = -Math.Atan2(r.hit_tex_coord.z, r.hit_tex_coord.x) / (2 * Math.PI);
            sp.y = -Math.Atan2(Math.Sqrt(r.hit_tex_coord.x * r.hit_tex_coord.x + r.hit_tex_coord.z * r.hit_tex_coord.z), r.hit_tex_coord.y) / (Math.PI);
            sp.z = 0;

            vect3d diffuse_color = tex.sample(sp);

            r.color = diff_accum.times(diffuse_color) + spec_accum.times(specular_color);
        }
    }
}
