#include "camera.h"
#include "ray.h"
#include "scene.h"
#include "shader.h"
#include "random.h"

#include <cmath>
#include <limits>
#include "FreeImagePlus.h"

static unsigned char clamp(double v)
{
	int i = (int)(v * 256);
	if (i < 0)
		i = 0;
	else if (i > 255)
		i = 255;
	return i;
}

Camera::Camera()
{
	pos = 0;
	orient = Quaternion::makeIdentity();
	width = 600, height = 400;
	fov_y = 70;
	subsamples = 2;
	focal_length = 1;
	aperture = 0;
}

void Camera::replaceFilm()
{
	film.resize(width, height);
	for (int i = 0; i < width*height; i++)
		film.data[i] = 0;
}

void Camera::expose(Scene *scene)
{
	// get the orientation of the camera
	matrix4f rotmat;
	orient.quat2Matrix(rotmat);
	vect3d rt(rotmat.m[0], rotmat.m[1], rotmat.m[2]);
	vect3d up(rotmat.m[4], rotmat.m[5], rotmat.m[6]);
	vect3d fwd(rotmat.m[8], rotmat.m[9], rotmat.m[10]);
	
	//printf("fwd (%f, %f, %f)\n", fwd[0], fwd[1], fwd[2]);
	//printf("up (%f, %f, %f)\n", up[0], up[1], up[2]);
	//printf("rt (%f, %f, %f)\n", rt[0], rt[1], rt[2]);

	vect3d unit_rt = rt;
	vect3d unit_up = up;

	// convert the orientation to a 3D screen
	double aspect = (double)width / (double)height;
	double h = tan(fov_y * pi / 360.0);
	up *= h * 2 * focal_length;
	rt *= aspect * h * 2 * focal_length;
	fwd *= -focal_length;

	// 2D screen conversions
	vect3d dx = rt / width;
	vect3d dy = up / height;
	vect3d corner = fwd - rt * .5 - up * .5;

	// expose each pixel
	Ray r;
	r.scene = scene;

	double subsample_res = 1.0 / subsamples;

	for (int j = 0; j < height; j++)
	{
		for (int i = 0; i < width; i++)
		{
			vect3d color = 0;

			for (int jj = 0; jj < subsamples; jj++)
			{
				for (int ii = 0; ii < subsamples; ii++)
				{
					// depth of field offset
					vect2d dof2d;
					rand_mt_in_sphere(dof2d);
					vect3d dof = (unit_rt * dof2d[0] + unit_up * dof2d[1]) * aperture;

					// set ray properties
					r.pos = pos + dof;
					r.hit_dist = double_max_value;
					r.color = 0;
					r.hit_object = 0;

					r.dir = corner - dof + dx * (i + (ii+rand_mt_real2()) * subsample_res) + dy * (j + (jj+rand_mt_real2()) * subsample_res);
					r.dir.normalize();

					// trace the ray
					scene->trace(r);
					if (r.hit_object)
					{
						r.hit_object->shader->colorize(r);
						color += r.color;
					}
					else
					{
						color += scene->background_color;
					}
				}
			}

			color *= subsample_res * subsample_res;

			film(i,j) += color;
		}
	}

}

fipImage *Camera::create_image()
{
	fipImage *bmp = new fipImage(FIT_BITMAP, width, height, 24);

	for (int j = 0; j < height; j++)
	{
		vect3ub *scanline = (vect3ub*)bmp->getScanLine(j/*height - j - 1*/);

		for (int i = 0; i < width; i++)
		{
			vect3f &color = film(i,j);

			// gamma correct the color
			double brightness = color.length() / 1.7320508075688772;
			color /= sqrt(brightness);

			// store the color
			scanline[i][2] = clamp(color[0]);
			scanline[i][1] = clamp(color[1]);
			scanline[i][0] = clamp(color[2]);
		}
	}

	return bmp;
}
