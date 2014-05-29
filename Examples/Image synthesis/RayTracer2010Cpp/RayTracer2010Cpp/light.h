#pragma once
#include "global.h"

struct Light
{
    vect3d pos, color;
	
	Light(){}
	Light(vect3d c, vect3d p)
	{
		color = c;
		pos = p;
	}
};

