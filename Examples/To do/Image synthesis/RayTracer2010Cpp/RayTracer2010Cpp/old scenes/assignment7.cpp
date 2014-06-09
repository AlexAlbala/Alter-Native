#include "global.h"

#include "camera.h"
#include "timer.h"
#include "scene.h"
#include "plane.h"
#include "sphere.h"
#include "triangle.h"
#include "light.h"
#include "grouplist.h"
#include "grouptree.h"
#include "shadowshader.h"
#include "textureshader.h"
#include "texturesphereshader.h"
#include "reflectionshader.h"
#include "refractionshader.h"
#include "array.h"

#include "FreeImagePlus.h"
#include <stdio.h>
#include <string>
#include <fstream>

using namespace std;


GroupList *load_obj(string fn, Shader *shader);

int main()
{
	//--------------------------------------------------------------------------------
	// Build the scene
	//--------------------------------------------------------------------------------
	double t_init = get_time();
	
	Camera camera;
	camera.width = 600 / 1;
	camera.height = 400 / 1;
	camera.subsamples = 12;
	camera.fov_y = 35;//39.5;
	//camera.orient = Quaternion::makeIdentity();
	camera.orient = Quaternion::makeRotation(pi / 9, 1, 0, 0);
	//camera.pos(0, -.7, 5);
	camera.pos(0, .35, 3);
	camera.focal_length = 4.5;
	camera.aperture = 0.1;

	GroupList occluder;

	{
		//Plane *plane = new Plane(vect3d(0, -1, 0), vect3d(1, -1, 0), vect3d(1, -1, -1), new ReflectionShader());
		//TextureShader *sdr = new TextureShader("../resources/images/wall_512_3_05_sml2.jpg");
		ReflectionShader *sdr = new ReflectionShader();
		sdr->specular_color = .7;
		sdr->glossiness = .9;
		Plane *plane = new Plane(vect3d(0, -1, 0), vect3d(1, -1, 0), vect3d(1, -1, -1), sdr);
		plane->x *= .3;
		plane->y *= .3;
		occluder.add(plane);
	}

	for (int i = 0; i < 15; i++)
	{
		ReflectionShader *sdr = new ReflectionShader();
		float t = i * .25;
		float scale = .7;
		sdr->diffuse_color = 0;//vect3d(cos(t), cos(t + 2*pi/3), cos(t + 4*pi/3)) * .5 + .5;
		sdr->specular_color = (vect3d(cos(t), cos(t + 2*pi/3), cos(t + 4*pi/3)) * .5 + .5) * .5;
		sdr->glossiness = pow((i % 4) / 3.0, .25);
		occluder.add(new Sphere(vect3d(-2 + scale*1*i,-.5,2.5 - scale*2*i), .5, sdr));
	}

	{
		//GroupList *obj = load_obj("sphere_new_rotation.obj", sdr);
		//occluder.add(new GroupTree(obj->list, sdr));
	}

	Scene scene;
	scene.occluder = &occluder;
	scene.lights.push(new Light(60, vect3d(-4, 8, 8)));
	scene.lights.push(new Light(40, vect3d(1, 15, 10)));

	//--------------------------------------------------------------------------------
	// Render the scene
	//--------------------------------------------------------------------------------
	double t_start = get_time();
	printf("Time taken to create scene = %fs\n", (t_start - t_init));

	fipImage *bmp = camera.create_image(&scene);

	double t_end = get_time();
	printf("Time taken to render = %fs\n", (t_end - t_start));

	//--------------------------------------------------------------------------------
	// Write the image to disk
	//--------------------------------------------------------------------------------
	bmp->save("output.png");

	return 0;
}

string readline(ifstream &f)
{
	char buf[1024];
	f.getline(buf, 1024);
	return buf;
}

void split(string &str, string &split, Array<string> &toks)
{
	string t;
	for (char *c = (char *)str.c_str();; c++)
	{
		bool do_split = false;
		for (int i = 0; i < split.size(); i++)
		{
			if (*c == split[i])
			{
				do_split = true;
				break;
			}
		}

		if (do_split)
		{
			if (t.size())
			{
				toks.push(t);
				t.clear();
			}
		}
		else if (*c == 0)
		{
			if (t.size())
				toks.push(t);

			return;
		}
		else
			t += *c;
	}
}


void splitEmpty(string &str, string &split, Array<string> &toks)
{
	string t;
	for (char *c = (char *)str.c_str();; c++)
	{
		bool do_split = false;
		for (int i = 0; i < split.size(); i++)
		{
			if (*c == split[i])
			{
				do_split = true;
				break;
			}
		}

		if (do_split)
		{
			toks.push(t);
			t.clear();
		}
		else if (*c == 0)
		{
			toks.push(t);
			return;
		}
		else
			t += *c;
	}
}

GroupList *load_obj(string fn, Shader *shader)
{
	GroupList *mesh = new GroupList();
	Array<vect3d> verts, norms, texc;

	ifstream reader(fn.c_str());
	assert(!reader.fail()); 

	string whitespace = " \t\n";
	string slash = "/";

	string line;
	while (true)
	{
		line = readline(reader);
		if (reader.eof())
			break;

		Array<string> toks;
		split(line, whitespace, toks);
		if (toks.s == 0)
			continue;

		if (toks[0] == "v")
		{
			verts.push(vect3d(atof(toks[1].c_str()), atof(toks[2].c_str()), atof(toks[3].c_str())));
		}
		else if (toks[0] == "vt")
		{
			texc.push(vect3d(atof(toks[1].c_str()), atof(toks[2].c_str()), 0));
		}
		else if (toks[0] == "vn")
		{
			norms.push(vect3d(atof(toks[1].c_str()), atof(toks[2].c_str()), atof(toks[3].c_str())));
		}
		else if (toks[0] == "f")
		{
			Array<string> vals0, vals1, vals2;
			
			splitEmpty(toks[1], slash, vals0);
			splitEmpty(toks[2], slash, vals1);
			splitEmpty(toks[3], slash, vals2);

			int i0 = atoi(vals0[0].c_str()) - 1;
			int i1 = atoi(vals1[0].c_str()) - 1;
			int i2 = atoi(vals2[0].c_str()) - 1;
			
			if (i0 < 0)
				i0 += verts.s + 1;
			if (i1 < 0)
				i1 += verts.s + 1;
			if (i2 < 0)
				i2 += verts.s + 1;

			Triangle *t = new Triangle(verts[i0], verts[i1], verts[i2], shader);

			if (texc.s != 0)
			{
				int j0 = atoi(vals0[1].c_str()) - 1;
				int j1 = atoi(vals1[1].c_str()) - 1;
				int j2 = atoi(vals2[1].c_str()) - 1;

				t->t0 = texc[j0];
				t->t1 = texc[j1];
				t->t2 = texc[j2];
			}
			if (norms.s != 0)
			{
				int k0 = atoi(vals0[2].c_str()) - 1;
				int k1 = atoi(vals1[2].c_str()) - 1;
				int k2 = atoi(vals2[2].c_str()) - 1;

				t->n0 = norms[k0];
				t->n1 = norms[k1];
				t->n2 = norms[k2];
			}

			mesh->add(t);
		}
	}

	reader.close();

	//// calc center
	//vect3f sum = 0;
	//for (int i = 0; i < verts.s; i++)
	//{
	//	sum += verts[i];
	//}
	//sum /= verts.s;
	//printf("CENTER AT %f, %f, %f\n", sum[0], sum[1], sum[2]);

	//// subtract center
	//for (int i = 0; i < mesh->list.s; i++)
	//{
	//	((Triangle*)mesh->list[i])->p0 -= sum;
	//	((Triangle*)mesh->list[i])->p1 -= sum;
	//	((Triangle*)mesh->list[i])->p2 -= sum;
	//}
	return mesh;
}