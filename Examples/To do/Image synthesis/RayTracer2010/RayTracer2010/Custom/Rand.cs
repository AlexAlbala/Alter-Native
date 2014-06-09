using System;
using System.Collections.Generic;
using System.Text;

using Custom.Math;

namespace Custom
{
    public class Rand
    {
        public static System.Random rand = new System.Random();

        public static vect3d rand_in_sphere()
        {
            vect3d p;

            do
            {
                p = new vect3d(rand.NextDouble() * 2 - 1, rand.NextDouble() * 2 - 1, rand.NextDouble() * 2 - 1);
            } while (p.length2() > 1);

            return p;
        }
    }
}
