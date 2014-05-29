using System;
using System.Collections.Generic;
using System.Text;

using Custom.Math;

namespace Custom
{
    public static class Intersect
    {

        public static bool rayBoxIntersectDist(vect3d ray_pos, vect3d ray_dir, box3d box, out double dist, out int face_d, out int face_n)
        {
            vect3d box_low = box[0];
            vect3d box_high = box[1];
            dist = 0;
            double Tnear = -1e30;
            double Tfar = 1e30;
            double T1, T2;
            face_d = -1;
            face_n = -1;
            // check bounding box to see if it enters level
            // this method assumes bounding box is already in coordinate system of the level
            for (int d = 0; d < 3; d++)
            {
                int n_temp = 0;
                if (ray_dir[d] == 0)
                {
                    if (ray_pos[d] > box_high[d] || ray_pos[d] < box_low[d])
                        return false;

                    //int dp = (d + 1) % 3;
                    //int dpp = (d + 2) % 3;

                    //if (ray_pos[dp] > box_high[dp] || ray_pos[dp] < box_low[dp])
                    //    return false;
                    //if (ray_pos[dpp] > box_high[dpp] || ray_pos[dpp] < box_low[dpp])
                    //    return false;
                }
                else
                {
                    T1 = (box_low[d] - ray_pos[d]) / ray_dir[d];
                    T2 = (box_high[d] - ray_pos[d]) / ray_dir[d];
                    if (T1 > T2)
                    {
                        //swap(T1, T2); // since T1 intersection should be the nearest
                        double t = T1;
                        T1 = T2;
                        T2 = t;
                        n_temp = (n_temp + 1) % 2;
                    }
                    if (T1 > Tnear)
                    {
                        face_d = d;
                        face_n = n_temp;
                        Tnear = T1; // largest Tnear
                    }
                    if (T2 < Tfar)
                        Tfar = T2; // smallest Tfar
                    if (Tnear > Tfar) // box is missed
                        return false;
                    if (Tfar < 0)
                        return false; // box is behind ray
                }
            }
            dist = Tnear;
            if (dist < 0)
                return false;
            return true;
        }

        public static bool rayBoxIntersectPos(vect3d ray_pos, vect3d ray_dir, box3d box, out double dist, out vect3d pos, out int d, out int n)
        {
            if (rayBoxIntersectDist(ray_pos, ray_dir, box, out dist, out d, out n))
            {
                pos = ray_pos + ray_dir * dist;
                return true;
            }
            pos = new vect3d(0);
            return false;
        }

        public static bool rayTriangleIntersect(vect3d ray_pos, vect3d ray_dir, vect3d v0, vect3d v1, vect3d v2, out double dist)
        {
            vect3d u = v1 - v0;
            vect3d v = v2 - v0;
            vect3d n = u % v; // normal
            dist = -((ray_pos - v0) * n) / (ray_dir * n); // distance to plane
            vect3d pos = ray_pos + ray_dir * dist; // point in plane of triangle
            vect3d w = pos - v0;

            double uv = u * v;
            double wv = w * v;
            double vv = v * v;
            double wu = w * u;
            double uu = u * u;

            double denom = uv * uv - uu * vv;
            double s = (uv * wv - vv * wu) / denom;
            double t = (uv * wu - uu * wv) / denom;

            if (s >= 0 && t >= 0 && s + t <= 1)
                return true;
            else
                return false;
        }

        public static bool intersect_aabb_tri(vect3d mine, vect3d maxe, vect3d v0, vect3d v1, vect3d v2)
        {
            //** use separating axes to determine intersection

            // create points
            vect3d[] tri = { v0, v1, v2 };
            vect3d[] box_ext = { mine, maxe };
            vect3d[] box = new vect3d[8];

            for (int i = 0; i < 8; i++)
                box[i] = new vect3d((box_ext[i & 1])[0], (box_ext[(i >> 1) & 1])[1], (box_ext[i >> 2])[2]);

            // create axes
            vect3d[] axes = new vect3d[13];
            axes[0] = new vect3d(1, 0, 0);
            axes[1] = new vect3d(0, 1, 0);
            axes[2] = new vect3d(0, 0, 1);
            axes[3] = (v1 - v0) % (v2 - v0);
            for (int i = 0; i < 3; i++)
            {
                int ip = (i + 1) % 3;
                for (int j = 0; j < 3; j++)
                {
                    axes[4 + i * 3 + j] = axes[j] % ((tri[i]) - (tri[ip]));
                }
            }

            // check overlaps
            for (int i = 0; i < 13; i++)
            {
                // tri extents
                double tri_min, tri_max;
                tri_min = tri_max = (tri[0]) * axes[i];
                for (int k = 1; k < 3; k++)
                {
                    double d = (tri[k]) * axes[i];
                    if (d < tri_min)
                        tri_min = d;
                    if (d > tri_max)
                        tri_max = d;
                }

                // box extents
                double box_min, box_max;
                box_min = box_max = box[0] * axes[i];
                for (int j = 1; j < 8; j++)
                {
                    double d = box[j] * axes[i];
                    if (d < box_min)
                        box_min = d;
                    if (d > box_max)
                        box_max = d;
                }

                // if disjoint, they don't intersect
                if (box_max < tri_min || tri_max < box_min)
                    return false;
            }

            return true;
        }

        public static double dist2_aabb_pt(vect3d mine, vect3d maxe, vect3d pt)
        {
            double d2 = 0;

            for (int i = 0; i < 3; i++)
            {
                double d = pt[i] - mine[i];
                if (d < 0)
                {
                    d2 += d * d;
                    continue;
                }

                d = pt[i] - maxe[i];
                if (d > 0)
                    d2 += d * d;
            }

            return d2;
        }
    }
}
