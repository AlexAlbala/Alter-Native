#include "grouptree.h"
#include "intersect.h"
#include "ray.h"

struct KDTreeOccluder
{
	KDTreeOccluder *left, *right;
	Array<Occluder*> list;
	box3d bounds;

	~KDTreeOccluder()
	{
		delete left;
		delete right;
	}

	KDTreeOccluder(Array<Occluder*> &list_in, box3d &bounds_in, int depth, double parent_val)
	{
		bounds = bounds_in;
		list = list_in;
		double area = bounds.area();

		// calc the optimal way to split the node
		int min_axis = 0;
		double min_pos = 0;
		double min_val = double_max_value;

		int samples = 5;
		int a = depth % 3;
		for (int i = 0; i < samples; i++)
		{
			double pos = bounds[0][a] + (bounds[1][a] - bounds[0][a]) * (double)(i + .5) / (double)samples;
			double cnt_l = 0, cnt_r = 0;
			box3d b_l = bounds, b_r = bounds;
			b_l[1][a] = pos;
			b_r[0][a] = pos;

			for (int k = 0; k < list.s; k++)
			{
				bool cross_l, cross_r;
				list[k]->check_crossing(pos, a, cross_l, cross_r);
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
			b_l[1][min_axis] = min_pos;
			b_r[0][min_axis] = min_pos;

			Array<Occluder*> L, R;
			for (int i = 0; i < list.s; i++)
			{
				if (list[i]->intersects_box(b_l))
					L.push(list[i]);
				if (list[i]->intersects_box(b_r))
					R.push(list[i]);
			}

			left = new KDTreeOccluder(L, b_l, depth + 1, min_val);
			right = new KDTreeOccluder(R, b_r, depth + 1, min_val);
			
			list.clear();
		}
		else
		{
			// is leaf
			left = right = 0;
		}
	}

	void trace(Ray &r)
	{
		if (left)
		{
			double d_l, d_r;
			bool sb_l, sb_r;

			bool h_l = rayBoxIntersectDist(r.pos, r.dir, left->bounds[0], left->bounds[1], d_l, sb_l);
			bool h_r = rayBoxIntersectDist(r.pos, r.dir, right->bounds[0], right->bounds[1], d_r, sb_r);

			if (h_l && h_r)
			{
				if (sb_l || d_l < d_r)
				{
					left->trace(r);
					if (d_r < r.hit_dist)
						right->trace(r);
				}
				else
				{
					right->trace(r);
					if (d_l < r.hit_dist)
						left->trace(r);
				}
			}
			else if (h_l)
				left->trace(r);
			else if (h_r)
				right->trace(r);
		}
		else
		{
			for (int i = 0; i < list.s; i++)
				list[i]->trace(r);
		}
	}
};

GroupTree::~GroupTree()
{
	delete tree;
}

GroupTree::GroupTree(Array<Occluder*> &list, Shader *s)
{
	shader = s;

	// calculate bounds of whole volume
	box3d bounds(double_max_value, double_min_value);

	for (int i = 0; i < list.s; i++)
	{
		Occluder *v = list[i];
		box3d b = v->bounds();

		if (b[0][0] < bounds[0][0])
			bounds[0][0] = b[0][0];
		if (b[0][1] < bounds[0][1])
			bounds[0][1] = b[0][1];
		if (b[0][2] < bounds[0][2])
			bounds[0][2] = b[0][2];
		if (b[1][0] > bounds[1][0])
			bounds[1][0] = b[1][0];
		if (b[1][1] > bounds[1][1])
			bounds[1][1] = b[1][1];
		if (b[1][2] > bounds[1][2])
			bounds[1][2] = b[1][2];
	}

	// build the tree
	tree = new KDTreeOccluder(list, bounds, 0, double_max_value);
}

void GroupTree::trace(Ray &r)
{
    double d_l;
	bool sb;
    bool h_l = rayBoxIntersectDist(r.pos, r.dir, tree->bounds[0], tree->bounds[1], d_l, sb);
    if (h_l)
        tree->trace(r);
}
