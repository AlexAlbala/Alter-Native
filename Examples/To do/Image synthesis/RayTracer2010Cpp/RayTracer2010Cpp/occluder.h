#pragma once

#include "box.h"

struct Ray;
struct Shader;

struct Occluder
{
	Shader *shader;

	virtual void trace(Ray &r) {}
	virtual box3d bounds();
	virtual bool intersects_box(box3d b) {return true;}
	virtual void check_crossing(double pos, int a, bool &cross_l, bool &cross_r) {cross_l = cross_r = true;}

	virtual double light_power() {return 0;}
	virtual void get_light_emission(vect3d &pos, vect3d &norm, vect3d &color) {}
};
