#pragma once
#include "global.h"
#include "occluder.h"
#include "ray.h"

struct Quadric : public Occluder
{
	double A, B, C, D, E, F, G, H, I, J;

	Quadric()
	{
		A = B = C = D = E = F = G = H = I = J = 0;
	}

	virtual void trace(Ray &r)
	{
		vect3d o = r.pos, d = r.dir;
		double t = double_max_value;

		double Aq = A * d[0] * d[0] + D * d[0] * d[1] + B * d[1] * d[1] + C * d[2] * d[2] + d[0] * d[2] * E + d[1] * d[2] * F;
		double Bq = d[0] * G + d[1] * H + d[2] * I + 2 * A * d[0] * o[0] + D * d[1] * o[0] + d[2] * E * o[0] + D * d[0] * o[1] + 2 * B * d[1] * o[1] + d[2] * F * o[1] + 2 * C * d[2] * o[2] + d[0] * E * o[2] + d[1] * F * o[2];
		double Cq = J + G * o[0] + A * o[0] * o[0] + H * o[1] + D * o[0] * o[1] + B * o[1] * o[1] + I * o[2] + E * o[0] * o[2] + F * o[1] * o[2] + C * o[2] * o[2];

		if (Aq == 0)
		{
			if (Bq != 0)
				t = -Cq / Bq;
		}
		else
		{
			double Dq = Bq * Bq - 4 * Aq * Cq;
			if (Dq > 0)
			{
				if (r.is_outside)
					t = (-Bq - sqrt(Dq)) / (2 * Aq);
				else
					t = (-Bq + sqrt(Dq)) / (2 * Aq);
			}
		}

		if (t > 0 && t < r.hit_dist)
		{
			r.hit_dist = t;
			r.hit_object = this;

			vect3d p = r.pos + r.dir * t;
			r.hit_pos = p;
			r.hit_tex_coord = p;

			r.hit_norm(
				2 * A * p[0] + D * p[1] + E * p[2] + G, 
				2 * B * p[1] + D * p[0] + F * p[2] + H, 
				2 * C * p[2] + E * p[0] + F * p[1] + I);
			r.hit_norm.normalize();

			if (r.dir * r.hit_norm > 0)
				r.hit_norm *= -1;
		}
	}
};
