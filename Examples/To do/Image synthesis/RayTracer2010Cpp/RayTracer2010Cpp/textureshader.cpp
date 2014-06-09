#include "textureshader.h"
#include "sampler23lerp.h"
#include "ray.h"
#include "scene.h"

TextureShader::TextureShader(string fn)
{
	diffuse_coeff = .5;
	specular_color = .4;
	ambient_light = .01;
	specular_coeff = 100;
	tex = new Sampler23Lerp(fn);
}

void TextureShader::colorize(Ray &r)
{
	// sample texture
	vect3d diffuse_color = tex->sample(r.hit_tex_coord);

	if (r.bounces >= 10)
	{
		r.color = diffuse_color;
		return;
	}

	// calc lighting
	vect3d diff_accum = 0;
	vect3d spec_accum = 0;

	lightDirectPointSpec(r, specular_coeff, diff_accum, spec_accum);

	//// trace reflection ray
	//Ray ref;
	//ref.pos = r.hit_pos;
	//ref.dir = r.dir - 2 * (r.dir * r.hit_norm) * r.hit_norm;
	//ref.scene = r.scene;
	//ref.is_outside = r.is_outside;
	//ref.hit_object = 0;
	//ref.hit_dist = double_max_value;
	//ref.bounces = r.bounces + 1;

	//ref.scene->trace(ref);

	//if (ref.hit_object)
	//	ref.hit_object->shader->colorize(ref);
	//else
	//	ref.color = ref.scene->background_color;

	// combine colors
	r.color = (diff_accum + ambient_light).multiply(diffuse_color) + spec_accum.multiply(specular_color);// + ref.color.multiply(specular_color);
}