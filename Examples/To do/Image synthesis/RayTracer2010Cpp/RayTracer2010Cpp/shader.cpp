#include "shader.h"

#include "ray.h"
#include "light.h"
#include "scene.h"

void Shader::colorize(Ray &r)
{
	r.color = 1;
}

void Shader::lightDirectPointSpec(Ray &r, double specular_coeff, vect3d &diff_accum, vect3d &spec_accum)
{
	/*Ray shadow_ray;
	shadow_ray.pos = r.hit_pos;

	for (int i = 0; i < r.scene->lights.s; i++)
	{
		Light *l = r.scene->lights[i];

		vect3d dir = l->pos - r.hit_pos;
		double dist2 = dir.length2();
		double dist2_inv = 1.0 / dist2;
		dir *= sqrt(dist2_inv);

		// calculate incidences to do early reject of shadow ray
		double incidence_dif = (dir * r.hit_norm);
		vect3d dir_r = r.hit_norm * ((dir * r.hit_norm) * 2) - dir;
		double incidence_spec = (dir_r * r.hit_norm);

		if (incidence_dif < 0 && incidence_spec < 0)
			continue;

		// shadow ray test
		shadow_ray.dir = dir;
		shadow_ray.hit_dist = double_max_value;
		r.scene->trace(shadow_ray);
		if (shadow_ray.hit_dist * shadow_ray.hit_dist < dist2)
			continue;

		// diffuse
		if (incidence_dif > 0)
			diff_accum += l->color * incidence_dif * dist2_inv;

		// specular highlight
		if (incidence_spec > 0)
			spec_accum += l->color * pow(incidence_spec, specular_coeff) * dist2_inv;
	}*/
}

void Shader::lightDirectPointSpecNoShadow(Ray &r, double specular_coeff, vect3d &diff_accum, vect3d &spec_accum)
{
	/*Ray shadow_ray;
	shadow_ray.pos = r.hit_pos;
	
	for (int i = 0; i < r.scene->lights.s; i++)
	{
		Light *l = r.scene->lights[i];

		vect3d dir = l->pos - r.hit_pos;
		double dist2 = 1.0 / dir.length2();
		dir *= sqrt(dist2);

		// diffuse
		double incidence_dif = (dir * r.hit_norm);
		if (incidence_dif > 0)
			diff_accum += l->color * incidence_dif * dist2;

		// specular highlight
		vect3d dir_r = r.hit_norm * ((dir * r.hit_norm) * 2) - dir;
		double incidence_spec = (dir_r * r.hit_norm);
		if (incidence_spec > 0)
			spec_accum += l->color * pow(incidence_spec, specular_coeff) * dist2;
	}
*/
}

void Shader::lerp_norm(vect3d &ret, vect3d &v0, vect3d &v1, float t)
{
	ret = v0 * (1-t) + v1 * t;
	ret.normalize();
}

void Shader::slerp_norm(vect3d &ret, vect3d &v0, vect3d &v1, float t)
{
	// early out for end cases
	if (t == 0)
	{
		ret = v0;
		return;
	}
	else if (t == 1)
	{
		ret = v1;
		return;
	}

	// find axis
	vect3d axis = v0 % v1;
	axis.normalize();
	
	// find angle
	double angle;
	double cosang = v0 * v1;
	if (cosang > 1)
	{
		ret = v0;
		return;
	}
	else if (cosang < -1)
		angle = pi;
	else
		angle = acos(cosang);

	// rotate around axis
	Quaternion q = Quaternion::makeRotation(angle * t, axis);
	q.transformPoint(v0.v, ret.v);
}
