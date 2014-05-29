using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Custom.Math;

namespace RayTracer2010
{
    public class Ray
    {
        public vect3d pos, dir, color;
        public double hit_dist = Double.MaxValue;
        public bool is_outside = true;
        public Scene scene;

        public vect3d hit_pos, hit_norm, hit_tex_coord;
        public Occluder hit_object;
    }
}
