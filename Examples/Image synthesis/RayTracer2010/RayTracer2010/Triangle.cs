using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Custom.Math;

namespace RayTracer2010
{
    public class Triangle : Occluder
    {
        public vect3d n0, n1, n2; // per vertex normals
        public vect3d t0, t1, t2; // tex coords

        vect3d p0, p1, p2; // positions
        vect3d N; // scaled normal for calculating bary coords
        vect3d norm; // unit normal for getting plane intersection

        public Triangle(vect3d p0, vect3d p1, vect3d p2, Shader shader)
        {
            this.shader = shader;

            this.p0 = p0;
            this.p1 = p1;
            this.p2 = p2;

            N = (p1 - p0) % (p2 - p0);
            N /= N * N;

            norm = N;
            norm.normalize();

            n0 = n1 = n2 = norm;
        }

        public override void trace(Ray r)
        {
            double v = r.dir * norm;
            if (r.is_outside && v > 0 || !r.is_outside && v < 0)
                return;

            double dist = ((p0 - r.pos) * norm) / v;
            if (dist > 0 && dist < r.hit_dist)
            {
                vect3d hit_pos = r.pos + r.dir * dist;
                
            	// find barycentric coordinates;
            	double b0 = ((p1 - hit_pos) % (p2 - hit_pos)) * N;
            	double b1 = ((p2 - hit_pos) % (p0 - hit_pos)) * N;
            	double b2 = 1 - b0 - b1;

                if (b0 >= 0 && b1 >= 0 && b2 >= 0)
                {
                    r.hit_dist = dist;
                    r.hit_pos = hit_pos;
                    r.hit_tex_coord = t0 * b0 + t1 * b1 + t2 * b2;
                    r.hit_norm = n0 * b0 + n1 * b1 + n2 * b2;
                    r.hit_norm.normalize();
                    r.hit_object = this;
                }
            }
        }

        public override box3d bounds()
        {
            box3d b = new box3d();
            b[0] = p0;
            b[1] = p0;

            if (p1.x < b.xl)
                b.xl = p1.x;
            if (p1.y < b.yl)
                b.yl = p1.y;
            if (p1.z < b.zl)
                b.zl = p1.z;
            if (p1.x > b.xh)
                b.xh = p1.x;
            if (p1.y > b.yh)
                b.yh = p1.y;
            if (p1.z > b.zh)
                b.zh = p1.z;

            if (p2.x < b.xl)
                b.xl = p2.x;
            if (p2.y < b.yl)
                b.yl = p2.y;
            if (p2.z < b.zl)
                b.zl = p2.z;
            if (p2.x > b.xh)
                b.xh = p2.x;
            if (p2.y > b.yh)
                b.yh = p2.y;
            if (p2.z > b.zh)
                b.zh = p2.z;

            return b;
        }

        public override void check_crossing(double pos, int a, out bool cross_l, out bool cross_r)
        {
            cross_l = (p0[a] <= pos || p1[a] <= pos || p2[a] <= pos);
            cross_r = (p0[a] >= pos || p1[a] >= pos || p2[a] >= pos);
        }

        public override bool intersects_box(box3d b)
        {
            return Custom.Intersect.intersect_aabb_tri(b[0], b[1], p0, p1, p2);
        }
    }
}
