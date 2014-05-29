#pragma once
#include "global.h"
#include "array2d.h"

using namespace std;

struct fipImage;
struct Scene;

struct Camera
{
	vect3d pos;
	Quaternion orient;
	int width, height;
	double fov_y, focal_length, aperture;
	int subsamples;
	Array2D<vect3f> film;

	Camera();
	void replaceFilm();
	void expose(Scene *scene);
	fipImage *create_image();
};
