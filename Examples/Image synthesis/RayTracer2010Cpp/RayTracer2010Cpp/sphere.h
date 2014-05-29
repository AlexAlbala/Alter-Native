#pragma once
#include "occluder.h"
#include "intersect.h"

struct Sphere : public Occluder
{
	vect3d pos;
	double radius, radius2;

	Sphere(vect3d pos_, double r_, Shader *shader_)
	{
		shader = shader_;
		radius = r_;
		radius2 = r_ * r_;
		pos = pos_;
	}

	virtual void trace(Ray &r)
	{
		vect3d dst = r.pos - pos;
		double B = dst * r.dir;
		double C = dst.length2() - radius2;
		double D = B * B - C;
		if (D > 0)
		{
			double dist;
			double flip_norm = 1;
			if (r.is_outside)
				dist = -B - sqrt(D);
			else
			{
				dist = -B + sqrt(D);
				flip_norm = -1;
			}

			if (dist > 0 && dist < r.hit_dist)
			{
				r.hit_dist = dist;
				r.hit_pos = r.pos + r.dir*dist;
				r.hit_norm = (r.hit_pos - pos) * flip_norm;
				r.hit_norm.normalize();
				r.hit_tex_coord = r.hit_norm;
				r.hit_object = this;
			}
		}
	}

	virtual box3d bounds()
	{
		box3d b;
		b[0] = pos - radius;
		b[1] = pos + radius;
		return b;
	}

	virtual void check_crossing(double p, int a, bool &cross_l, bool &cross_r)
	{
		cross_l = (pos[a] - radius <= p);
		cross_r = (pos[a] + radius >= p);
	}

	virtual bool intersects_box(box3d b)
	{
		return dist2_aabb_pt(b[0], b[1], pos) <= radius2;
	}
};
