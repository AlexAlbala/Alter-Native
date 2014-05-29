#pragma once
#include "occluder.h"
#include "intersect.h"
#include "random.h"
#include <algorithm>

using namespace std;

struct Triangle : public Occluder
{
	vect3d n0, n1, n2; // per vertex normals
	vect3d t0, t1, t2; // tex coords

	vect3d p0, p1, p2; // positions
	vect3d N; // scaled normal for calculating bary coords
	vect3d norm; // unit normal for getting plane intersection
	double area;

	Triangle(vect3d p0, vect3d p1, vect3d p2, Shader *shader)
	{
		this->shader = shader;

		this->p0 = p0;
		this->p1 = p1;
		this->p2 = p2;

		N = (p1 - p0) % (p2 - p0);
		N /= N * N;

		norm = N;
		area = N.length();
		norm /= area;
		area *= .5;

		n0 = n1 = n2 = norm;
	}

	virtual void trace(Ray &r)
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

	virtual box3d bounds()
	{
		box3d b;
		b[0] = p0;
		b[1] = p0;

		if (p1[0] < b[0][0])
			b[0][0] = p1[0];
		if (p1[1] < b[0][1])
			b[0][1] = p1[1];
		if (p1[2] < b[0][2])
			b[0][2] = p1[2];
		if (p1[0] > b[1][0])
			b[1][0] = p1[0];
		if (p1[1] > b[1][1])
			b[1][1] = p1[1];
		if (p1[2] > b[1][2])
			b[1][2] = p1[2];

		if (p2[0] < b[0][0])
			b[0][0] = p2[0];
		if (p2[1] < b[0][1])
			b[0][1] = p2[1];
		if (p2[2] < b[0][2])
			b[0][2] = p2[2];
		if (p2[0] > b[1][0])
			b[1][0] = p2[0];
		if (p2[1] > b[1][1])
			b[1][1] = p2[1];
		if (p2[2] > b[1][2])
			b[1][2] = p2[2];

		return b;
	}

	virtual void check_crossing(double pos, int a, bool &cross_l, bool &cross_r)
	{
		cross_l = (p0[a] <= pos || p1[a] <= pos || p2[a] <= pos);
		cross_r = (p0[a] >= pos || p1[a] >= pos || p2[a] >= pos);
	}

	virtual bool intersects_box(box3d b)
	{
		return intersect_aabb_tri(b[0], b[1], p0, p1, p2);
	}
	
	virtual double light_power() 
	{
		return area * shader->light_per_area;
	}

	virtual void get_light_emission(vect3d &pos, vect3d &norm_, vect3d &color) 
	{
		norm_ = norm;
		
		double s = rand_mt_real1();
		double t = rand_mt_real1();
		if (t > s)
			swap(s, t);

		pos = p0 + (p1 - p0) * s + (p2 - p1) * t;

		color = shader->light_color;
	}
};
