using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Custom.Math;

namespace RayTracer2010
{
    public class KDTreeOccluder
    {
        static int max_depth = 0;
        public KDTreeOccluder left, right;
        public List<Occluder> list;
        public box3d bounds;

        public KDTreeOccluder(List<Occluder> list_in, box3d bounds_in, int depth, double parent_val)
        {
            if (depth > max_depth)
            {
                max_depth = depth;
                Console.WriteLine("reached depth {0}", depth);
            }

            bounds = bounds_in;
            list = list_in;
            double area = bounds.area();

            // calc the optimal way to split the node
            int min_axis = 0;
            double min_pos = 0;
            double min_val = Double.MaxValue;

            int samples = 5;
            int a = depth % 3;
            for (int i = 0; i < samples; i++)
            {
                double pos = bounds[0, a] + (bounds[1, a] - bounds[0, a]) * (double)(i + .5) / (double)samples;
                double cnt_l = 0, cnt_r = 0;
                box3d b_l = bounds, b_r = bounds;
                b_l[1, a] = pos;
                b_r[0, a] = pos;

                for (int k = 0; k < list.Count; k++)
                {
                    bool cross_l, cross_r;
                    list[k].check_crossing(pos, a, out cross_l, out cross_r);
                    if (cross_l)
                        cnt_l++;
                    if (cross_r)
                        cnt_r++;
                }

                double val = 1 + (cnt_l * b_l.area() + cnt_r * b_r.area()) / area;

                if (val < min_val)
                {
                    min_val = val;
                    min_axis = a;
                    min_pos = pos;
                }
            }

            // split
            if (min_val < parent_val && depth < 100)
            {
                // is internal
                box3d b_l = bounds, b_r = bounds;
                b_l[1, min_axis] = min_pos;
                b_r[0, min_axis] = min_pos;

                List<Occluder> L = new List<Occluder>(), R = new List<Occluder>();
                foreach (var v in list)
                {
                    if (v.intersects_box(b_l))
                        L.Add(v);
                    if (v.intersects_box(b_r))
                        R.Add(v);
                }

                left = new KDTreeOccluder(L, b_l, depth + 1, min_val);
                right = new KDTreeOccluder(R, b_r, depth + 1, min_val);
            }
            else
            {
                // is leaf
            }
        }

        public void trace(Ray r)
        {
            if (left != null)
            {
                int f_d, f_n;
                double d_l, d_r;
                bool h_l = Custom.Intersect.rayBoxIntersectDist(r.pos, r.dir, left.bounds, out d_l, out f_d, out f_n);
                bool h_r = Custom.Intersect.rayBoxIntersectDist(r.pos, r.dir, right.bounds, out d_r, out f_d, out f_n);

                if (h_l && h_r)
                {
                    if (d_l < d_r)
                    {
                        left.trace(r);
                        if (d_r < r.hit_dist)
                            right.trace(r);
                    }
                    else
                    {
                        right.trace(r);
                        if (d_l < r.hit_dist)
                            left.trace(r);
                    }
                }
                else if (h_l)
                    left.trace(r);
                else if (h_r)
                    right.trace(r);
            }
            else
            {
                foreach (var o in list)
                    o.trace(r);
            }
        }
    }

    public class GroupTree : Occluder
    {
        public KDTreeOccluder tree;

        public GroupTree(List<Occluder> list, Shader s)
        {
            shader = s;

            //// randomize list
            //for (int i = list.Count - 1; i > 1; i--)
            //{
            //    int r = Custom.Rand.rand.Next(i - 1);
            //    var t = list[i];
            //    list[i] = list[r];
            //    list[r] = t;
            //}

            // calculate bounds of whole volume
            box3d bounds = new box3d();
            bounds.xh = bounds.yh = bounds.zh = Double.MinValue;
            bounds.xl = bounds.yl = bounds.zl = Double.MaxValue;

            foreach (var v in list)
            {
                box3d b = v.bounds();

                if (b.xl < bounds.xl)
                    bounds.xl = b.xl;
                if (b.yl < bounds.yl)
                    bounds.yl = b.yl;
                if (b.zl < bounds.zl)
                    bounds.zl = b.zl;
                if (b.xh > bounds.xh)
                    bounds.xh = b.xh;
                if (b.yh > bounds.yh)
                    bounds.yh = b.yh;
                if (b.zh > bounds.zh)
                    bounds.zh = b.zh;
            }

            // build the tree
            tree = new KDTreeOccluder(list, bounds, 0, Double.MaxValue);
        }

        public override void trace(Ray r)
        {
            int f_d, f_n;
            double d_l;
            bool h_l = Custom.Intersect.rayBoxIntersectDist(r.pos, r.dir, tree.bounds, out d_l, out f_d, out f_n);
            if (h_l && d_l >= 0)
                tree.trace(r);
        }
    }

    public class GroupList : Occluder
    {
        public List<Occluder> list = new List<Occluder>();

        public void add(Occluder o)
        {
            list.Add(o);
        }

        public override void trace(Ray r)
        {
            foreach (var o in list)
                o.trace(r);
        }
    }
}
