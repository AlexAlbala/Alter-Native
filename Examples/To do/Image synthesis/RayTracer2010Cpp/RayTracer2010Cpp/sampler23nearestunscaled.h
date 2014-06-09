#pragma once
#include "sampler.h"
#include "FreeImagePlus.h"
#include <string>
#include <math.h>

using namespace std;

struct Sampler23NearestUnscaled : Sampler
{
	fipImage bmp;
	int w, h;

	Sampler23NearestUnscaled(string name)
	{
		bmp.load(name.c_str());
		w = bmp.getWidth();
		h = bmp.getHeight();
	}

	virtual vect3d sample(vect3d &pos)
	{
		// find modulo value (texture wrapping)
		vect2d p(fmod(pos[0], w), fmod(pos[1], h));
		if (p[0] < 0)
			p[0] += w;
		if (p[1] < 0)
			p[1] += h;

		// get color out of bmp
		vect3ub color = ((vect3ub*)bmp.getScanLine(p[1]))[(int)p[0]];

		return vect3d(color[2] * (1.0 / 255.0), color[1] * (1.0 / 255.0), color[0] * (1.0 / 255.0));
	}
};
