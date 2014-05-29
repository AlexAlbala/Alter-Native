#pragma once
#include "shader.h"
#include "ray.h"

struct ShadowShader : public Shader
{
	vect3d diffuse_color, specular_color, ambient_light;
	double specular_coeff;

	ShadowShader()
	{
		diffuse_color = 1;
		specular_color = 1;
		ambient_light = .01;
		specular_coeff = 100;
	}

	virtual void colorize(Ray &r)
	{
		vect3d diff_accum = 0;
		vect3d spec_accum = 0;

		lightDirectPointSpec(r, specular_coeff, diff_accum, spec_accum);

		r.color = (diff_accum + ambient_light).multiply(diffuse_color) + spec_accum.multiply(specular_color);

		////*** Distance coloring
		//double d = r.hit_pos.length() - .4;
		//double scale = 128 * 10;
		//if (d > 0)
		//{
		//	float a = d*scale;
		//	r.color = vect3d(.75, .75, .75)*(1-a) + vect3d(1,0,0)*a;
		//}
		//else
		//{
		//	float a = -d*scale;
		//	r.color = vect3d(.75, .75, .75)*(1-a) + vect3d(0,0,1)*a;
		//}

		////*** normal coloring
		//double d = acos(~r.hit_norm * ~r.hit_pos) * 180 / pi;
		//float a = d / 15;
		//r.color = vect3d(.75, .75, .75)*(1-a) + vect3d(0,0,1)*a;
	}
};
