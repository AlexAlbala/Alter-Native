using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Custom.Math;

namespace RayTracer2010
{
    public class TextureShader : Shader
    {
        Sampler tex;
        public vect3d specular_color = new vect3d(1, 1, 1);
        public double specular_coeff = 100;

        public TextureShader(string fn)
        {
            tex = new Sampler23Lerp(fn);
        }

        public override void colorize(Ray r)
        {
            vect3d diff_accum = new vect3d(0, 0, 0);
            vect3d spec_accum = new vect3d(0, 0, 0);

            lightDirectPointSpec(r, specular_coeff, ref diff_accum, ref spec_accum);

            vect3d diffuse_color = tex.sample(r.hit_tex_coord);

            r.color = diff_accum.times(diffuse_color) + spec_accum.times(specular_color);
        }
    }
}
