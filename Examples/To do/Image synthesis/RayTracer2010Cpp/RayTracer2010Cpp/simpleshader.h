#pragma once
#include "shader.h"

struct SimpleShader : public Shader
{
	vect3d diffuse_color;
	vect3d specular_color;
	double specular_coeff;

	SimpleShader()
	{
		diffuse_color = 1;
		specular_color = 1;
		specular_coeff = 100;
	}

	virtual void colorize(Ray &r)
	{
		vect3d diff_accum = 0;
		vect3d spec_accum = 0;

		lightDirectPointSpecNoShadow(r, specular_coeff, diff_accum, spec_accum);

		r.color = diff_accum.multiply(diffuse_color) + spec_accum.multiply(specular_color);
	}
};
