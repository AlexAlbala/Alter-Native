#include "texturesphereshader.h"
#include "sampler23nearest.h"
#include "ray.h"

TextureSphereShader::TextureSphereShader(string fn)
{
	specular_color = 1;
	specular_coeff = 100;
	tex = new Sampler23Nearest(fn);
}

void TextureSphereShader::colorize(Ray &r)
{
	vect3d diff_accum = 0, spec_accum = 0;

	lightDirectPointSpec(r, specular_coeff, diff_accum, spec_accum);

	// calculate the polar coordinates
	vect3d sp;
	sp[0] = -atan2(r.hit_tex_coord[2], r.hit_tex_coord[0]) / (2 * pi);
	sp[1] = -atan2(sqrt(r.hit_tex_coord[0] * r.hit_tex_coord[0] + r.hit_tex_coord[2] * r.hit_tex_coord[2]), r.hit_tex_coord[1]) / pi;
	sp[2] = 0;

	vect3d diffuse_color = tex->sample(sp);

	r.color = diff_accum.multiply(diffuse_color) + spec_accum.multiply(specular_color);
}
