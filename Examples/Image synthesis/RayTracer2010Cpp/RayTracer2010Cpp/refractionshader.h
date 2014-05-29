#pragma once
#include "shader.h"
#include "ray.h"
#include "scene.h"

struct RefractionShader : public Shader
{
	vect3d diffuse_color, specular_color;
	double specular_coeff, refract_index;

	RefractionShader()
	{
		diffuse_color = .5;
		specular_color = .5;
		specular_coeff = 100;
		refract_index = 1.4;
	}

	virtual void colorize(Ray &r)
	{
		if (r.bounces >= 10)
		{
			r.color = diffuse_color;
			return;
		}

		r.color = 0;

		// calc lighting
		vect3d diff_accum = 0;
		vect3d spec_accum = 0;

		lightDirectPointSpec(r, specular_coeff, diff_accum, spec_accum);
		
		r.color += diff_accum.multiply(diffuse_color)/* + spec_accum.multiply(specular_color)*/;
		
		//------------------------------------------------------
		// trace reflection and refraction
		//------------------------------------------------------
		Ray ref;
		ref.pos = r.hit_pos;
		ref.bounces = r.bounces + 1;
		ref.scene = r.scene;

		// trace reflection ray
		double fresnel_split = 0;
		double c1 = -(r.dir * r.hit_norm);

		ref.hit_dist = double_max_value;
		ref.hit_object = 0;
		ref.is_outside = r.is_outside;
		ref.dir = r.dir + (2 * c1) * r.hit_norm;

		ref.scene->trace(ref);

		if (ref.hit_object)
			ref.hit_object->shader->colorize(ref);
		else
			ref.color = ref.scene->background_color;

		r.color += ref.color.multiply(specular_color) * fresnel_split;

		// trace refraction ray
		double n = 1.0 / (r.is_outside ? refract_index : 1.0 / refract_index);
		double D = 1 - n*n * (1 - c1*c1);
		if (D > 0)
		{
			double c2 = sqrt(D);

			ref.hit_dist = double_max_value;
			ref.hit_object = 0;
			ref.is_outside = !r.is_outside;
			ref.dir = n * r.dir + (n * c1 - c2) * r.hit_norm;
			ref.dir.normalize();
			
			ref.scene->trace(ref);

			if (ref.hit_object)
				ref.hit_object->shader->colorize(ref);
			else
				ref.color = ref.scene->background_color;

			r.color += ref.color.multiply(specular_color) * (1 - fresnel_split);
		}
	}
};
