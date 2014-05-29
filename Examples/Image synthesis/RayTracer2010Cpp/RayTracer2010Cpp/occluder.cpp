#include "occluder.h"
#include "global.h"

box3d Occluder::bounds()
{
	box3d b;
	b[0] = double_max_value;
	b[1] = double_min_value;
	return b;
}
