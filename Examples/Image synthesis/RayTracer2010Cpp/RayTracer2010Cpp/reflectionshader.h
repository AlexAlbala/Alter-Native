#pragma once
#include "shader.h"
#include "ray.h"
#include "scene.h"
#include "random.h"

struct ReflectionShader : public Shader
{
	vect3d reflect_color;
	double albedo, glossiness;

	ReflectionShader()
	{
		albedo = 1;
		reflect_color = 1;
		reflect_color.normalize();
		glossiness = 0;
	}
	
	virtual void colorize(Ray &r)
	{
		r.color = reflect_color * light_per_area;

		if (r.bounces >= 10)
			return;
		
		if (rand_mt_real1() > albedo)
			return;

		// calc lighting
		//vect3d diff_accum = 0;
		//vect3d spec_accum = 0;

		//lightDirectPointSpec(r, 100, diff_accum, spec_accum);

		//---------------------------------
		// trace reflection ray
		//---------------------------------
		Ray ref;
		ref.pos = r.hit_pos;
		ref.scene = r.scene;
		ref.is_outside = r.is_outside;
		ref.hit_object = 0;
		ref.hit_dist = double_max_value;
		ref.bounces = r.bounces + 1;
		
		// calc reflection dir
		vect3d spec = r.dir - 2 * (r.dir * r.hit_norm) * r.hit_norm;

		vect3d diff; 
		rand_mt_on_sphere(diff);
		if (diff * r.hit_norm < 0)
			diff = -diff;

		slerp_norm(ref.dir, diff, spec, glossiness);

		ref.scene->trace(ref);

		if (ref.hit_object)
			ref.hit_object->shader->colorize(ref);
		else
			ref.color = ref.scene->background_color;
			
		//---------------------------------
		// combine colors
		//---------------------------------
		r.color += /*diff_accum.multiply(diffuse_color) +*/ ref.color.multiply(reflect_color);
	}
};
