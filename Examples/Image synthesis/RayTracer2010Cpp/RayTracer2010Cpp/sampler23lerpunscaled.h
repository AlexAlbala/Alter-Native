#pragma once
#include "sampler.h"
#include "FreeImagePlus.h"
#include <string>
#include <math.h>

using namespace std;

struct Sampler23Lerp : Sampler
{
	fipImage bmp;
	int w, h;

	Sampler23Lerp(string name)
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
        int i = (int)p[0], j = (int)p[1];
        int ip = (i + 1) % w, jp = (j + 1) % h;
        double dx = p[0] - i, dy = p[1] - j;
		
		vect3d c00 = ((vect3ub*)bmp.getScanLine(j))[i];
		vect3d c01 = ((vect3ub*)bmp.getScanLine(jp))[i];
		vect3d c10 = ((vect3ub*)bmp.getScanLine(j))[ip];
		vect3d c11 = ((vect3ub*)bmp.getScanLine(jp))[ip];
		
		// combine samples with lerp
        vect3d c = (((1 - dx) * (1 - dy)) * c00 + ((1 - dx) * dy) * c01 + (dx * (1 - dy)) * c10 + (dx * dy) * c11) * (1.0/ 255.0);
		return vect3d(c[2], c[1], c[0]);
	}
};
