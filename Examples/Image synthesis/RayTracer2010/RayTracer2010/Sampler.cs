using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Custom.Math;

namespace RayTracer2010
{
    public class Sampler
    {
        public virtual vect3d sample(vect3d p)
        {
            return new vect3d(1, 1, 1);
        }
    }
}
