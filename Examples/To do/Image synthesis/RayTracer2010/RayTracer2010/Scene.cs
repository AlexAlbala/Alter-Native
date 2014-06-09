using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer2010
{
    public class Scene : Occluder
    {
        public Occluder occluder;
        public List<Light> lights = new List<Light>();

        public override void trace(Ray r)
        {
            occluder.trace(r);
        }
    }
}
