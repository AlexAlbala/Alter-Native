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
#include <direct.h>

using namespace std;


GroupList *load_obj(string fn, Shader *shader);

void loadSpheres(ifstream &file, Array<vect4d> &spheres)
{
	int num;
	file >> num;

	spheres.resize(num);

	for (int i = 0; i < num; i++)
		file >> spheres[i][0] >> spheres[i][1] >> spheres[i][2] >> spheres[i][3];
}

int main()
{
	ifstream file("../resources/spheres.dat");
	if (file.fail())
	{
		printf("FAILED TO OPEN spheres.dat\n");
		return 1;
	}
	mkdir("out");

	// shaders
	ReflectionShader *sdr_plane = new ReflectionShader();
	sdr_plane->specular_color(.5, .5, .8);
	sdr_plane->glossiness = 0;

	ReflectionShader *sdr_big = new ReflectionShader();
	sdr_big->diffuse_color = 0;
	sdr_big->specular_color(.5,.1,.1);
	sdr_big->glossiness = 0;
	
	ReflectionShader *sdr_small = new ReflectionShader();
	sdr_small->diffuse_color = 0;
	sdr_small->specular_color(.5,.5,.1);
	sdr_small->glossiness = 0;
	
	// create camera
	Camera camera;
	camera.width = 1280 / 1;
	camera.height = 720 / 1;
	camera.subsamples = 1;
	camera.replaceFilm();
	
	Array<vect4d> spheres; 

	int render_start = 2239;
	int render_stop = 2400;
	int render_remainder = 0;

	// render frames
	int physframes = 33;
	for (int frame = 0; frame < 2400*physframes; frame++)
	{
		//for (int i = 0; i < frameskip * physframes; i++)
			loadSpheres(file, spheres);
			
		if (frame / physframes > render_stop)
			break;

		if (frame < render_start*physframes || (frame/physframes) % 2 != render_remainder)
		{
			//printf("skipping frame %d\n", frame / physframes);
			continue;
		}

		//--------------------------------------------------------------------------------
		// Build the scene
		//--------------------------------------------------------------------------------
		double t_init = get_time();

		// camera
		double angle = -4*2*pi*frame / double(2400*physframes);
		camera.fov_y = 55;
		camera.orient = Quaternion::makeRotation(-pi/2, 1, 0, 0)*Quaternion::makeRotation(angle, 0, 0, 1);
		camera.pos(20*sin(angle+pi), 20*cos(angle+pi), 5);
		camera.focal_length = 17;
		camera.aperture = 0.3;



		GroupList occluder;

		// ground plane
		{
			Plane *plane = new Plane(vect3d(0, 0, 0), vect3d(1, 0, 0), vect3d(1, 1, 0), sdr_plane);
			plane->x *= .3;
			plane->y *= .3;
			occluder.add(plane);
		}

		// spheres
		Array<Occluder*> splist;

		for (int i = 0; i < spheres.s; i++)
		{
			vect4d &spv = spheres[i];

			ReflectionShader *sdr;
			if (spv[3] >= .99)
				sdr = sdr_big;
			else
				sdr = sdr_small;
			sdr->glossiness = 0;

			splist.push(new Sphere(vect3d(spv), spv[3], sdr));
		}

		occluder.add(new GroupTree(splist, 0));



		Scene scene;
		scene.occluder = &occluder;
		scene.lights.push(new Light(60, vect3d(-4, 8, 8)));
		scene.lights.push(new Light(40, vect3d(1, 15, 10)));

		//--------------------------------------------------------------------------------
		// Render the scene
		//--------------------------------------------------------------------------------
		double t_start = get_time();
		printf("Time taken to create scene = %fs\n", (t_start - t_init));

		camera.expose(&scene);

		double t_end = get_time();
		printf("Time taken to render = %fs\n", (t_end - t_start));

		//--------------------------------------------------------------------------------
		// Write the image to disk
		//--------------------------------------------------------------------------------
		
		if ((frame % physframes) == physframes-1)
		{
			for (int i = 0; i < camera.width*camera.height; i++)
				camera.film.data[i] /= physframes;
	
			fipImage *bmp = camera.create_image();
			camera.replaceFilm();

			char fn[1024];
			sprintf(fn, "out/frame_%04d.png", frame / physframes);
			bmp->save(fn);

			delete bmp;

			printf("wrote frame %d\n", frame / physframes);
		}

		//--------------------------------------------------------------------------------
		// Free memory
		//--------------------------------------------------------------------------------
		for (int i = 0; i < splist.s; i++)
			delete splist[i];

		for (int i = 0; i < occluder.list.s; i++)
			delete occluder.list[i];
	}

	file.close();
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