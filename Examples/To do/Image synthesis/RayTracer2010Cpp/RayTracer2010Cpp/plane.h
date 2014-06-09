#pragma once

#include "occluder.h"
#include "ray.h"

struct Shader;

struct Plane : public Occluder
{
	vect3d pos, x, y, z;

	Plane(vect3d p0, vect3d p1, vect3d p2, Shader *shader_)
	{
		shader = shader_;
		pos = p0;

		z = (p1 - p0) % (p2 - p0);
		z.normalize();

		x = p2 % z;
		x.normalize();

		y = z % x;
	}

	virtual void trace(Ray &r)
	{
		double v = r.dir * z;
		if (r.is_outside && v > 0 || !r.is_outside && v < 0)
			return;

		double dist = ((pos - r.pos) * z) / v;
		if (dist > 0 && dist < r.hit_dist)
		{
			r.hit_dist = dist;
			r.hit_pos = r.pos + r.dir * dist;
			vect3d rp = r.hit_pos - pos;
			r.hit_tex_coord(x * rp, y * rp, 0);
			r.hit_norm = z;
			r.hit_object = this;
		}
	}
};
