#pragma once
#include "global.h"

struct Ray;

struct Shader
{
	Shader()
	{
		light_per_area = 0;
		light_color = 1;
	}
	double light_per_area;
	vect3d light_color;

	virtual void colorize(Ray &r);

	void lightDirectPointSpec(Ray &r, double specular_coeff, vect3d &diff_accum, vect3d &spec_accum);
	void lightDirectPointSpecNoShadow(Ray &r, double specular_coeff, vect3d &diff_accum, vect3d &spec_accum);
	
	void lerp_norm(vect3d &ret, vect3d &v0, vect3d &v1, float t);
	void slerp_norm(vect3d &ret, vect3d &v0, vect3d &v1, float t);
};
