#pragma once
#include "global.h"

struct Scene;
struct Occluder;

struct Ray
{
	Ray()
	{
		is_outside = true;
		hit_object = 0;
		scene = 0;
		bounces = 0;
		hit_dist = double_max_value;
	}

	vect3d pos, dir, color;
	double hit_dist;
	int bounces;
	bool is_outside;
	Scene *scene;

	vect3d hit_pos, hit_norm, hit_tex_coord;
	Occluder *hit_object;
};
