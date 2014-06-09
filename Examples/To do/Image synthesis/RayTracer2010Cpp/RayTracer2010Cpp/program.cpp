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

int main()
{
	// shaders
	ReflectionShader *sdr_white = new ReflectionShader();
	sdr_white->reflect_color(.8, .8, .8);
	sdr_white->glossiness = 0;

	ReflectionShader *sdr_red = new ReflectionShader();
	sdr_red->reflect_color(.8, .2, .1);
	sdr_red->glossiness = 0;
	
	ReflectionShader *sdr_blue = new ReflectionShader();
	sdr_blue->reflect_color(.1, .3, .8);
	sdr_blue->glossiness = 0;
	
	RefractionShader *sdr_glass = new RefractionShader();
	sdr_glass->diffuse_color(.8, .8, .8);
	sdr_glass->specular_color(.8, .8, .8);
	sdr_glass->refract_index = 1.5;

	ReflectionShader *sdr_light = new ReflectionShader();
	sdr_light->reflect_color(.8, .8, .8);
	sdr_light->light_color(1,1,1);
	sdr_light->light_per_area = 5;
	sdr_light->glossiness = 0;

	// create camera
	Camera camera;
	camera.width = 600;
	camera.height = 400;
	camera.subsamples = 10;
	camera.replaceFilm();

	//--------------------------------------------------------------------------------
	// Build the scene
	//--------------------------------------------------------------------------------
	double t_init = get_time();

	// camera
	double angle = 0;
	camera.fov_y = 55;
	camera.orient = Quaternion::makeRotation(-pi/2, 1, 0, 0)*Quaternion::makeRotation(angle, 0, 0, 1);
	camera.pos(2*sin(angle+pi), 2*cos(angle+pi), .5);
	camera.focal_length = 17;
	camera.aperture = 0.0;



	GroupList occluder;

	// room triangles
	Array<Occluder*> splist;
	vect3d verts[4*3];
	double rh = 1;
	double rw = 1;
	double rl = .3;
	verts[0](-rw,-rw,0);
	verts[1](rw,-rw,0);
	verts[2](rw,rw,0);
	verts[3](-rw,rw,0);
	
	verts[4](-rw,-rw,rh);
	verts[5](rw,-rw,rh);
	verts[6](rw,rw,rh);
	verts[7](-rw,rw,rh);
	
	verts[8](-rl,-rl,rh);
	verts[9](rl,-rl,rh);
	verts[10](rl,rl,rh);
	verts[11](-rl,rl,rh);

	splist.push(new Triangle(verts[0], verts[1], verts[2], sdr_white)); // floor
	splist.push(new Triangle(verts[0], verts[2], verts[3], sdr_white));

	splist.push(new Triangle(verts[0], verts[4], verts[5], sdr_white)); // front wall
	splist.push(new Triangle(verts[0], verts[5], verts[1], sdr_white));
	
	splist.push(new Triangle(verts[3], verts[2], verts[6], sdr_white)); // back wall
	splist.push(new Triangle(verts[3], verts[6], verts[7], sdr_white));
	
	splist.push(new Triangle(verts[3], verts[7], verts[4], sdr_red)); // left wall
	splist.push(new Triangle(verts[3], verts[4], verts[0], sdr_red));
	
	splist.push(new Triangle(verts[2], verts[1], verts[5], sdr_blue)); // right wall
	splist.push(new Triangle(verts[2], verts[5], verts[6], sdr_blue));
	
	splist.push(new Triangle(verts[8], verts[11], verts[10], sdr_light)); // light panel
	splist.push(new Triangle(verts[8], verts[10], verts[9], sdr_light));
	
	splist.push(new Triangle(verts[4], verts[8], verts[9], sdr_white)); // ceiling
	splist.push(new Triangle(verts[4], verts[9], verts[5], sdr_white));
	splist.push(new Triangle(verts[7], verts[11], verts[8], sdr_white)); // ceiling
	splist.push(new Triangle(verts[7], verts[8], verts[4], sdr_white));
	splist.push(new Triangle(verts[6], verts[10], verts[11], sdr_white)); // ceiling
	splist.push(new Triangle(verts[6], verts[11], verts[7], sdr_white));
	splist.push(new Triangle(verts[5], verts[9], verts[10], sdr_white)); // ceiling
	splist.push(new Triangle(verts[5], verts[10], verts[6], sdr_white));

	splist.push(new Sphere(vect3d(.25, .25, .25), .25, sdr_white)); // a sphere
	
	splist.push(new Sphere(vect3d(-.25, -.35, .35), .35, sdr_glass)); // a sphere

	occluder.add(new GroupTree(splist, 0));


	// finalize and light the scene
	Scene scene;
	scene.background_color = 0;
	scene.occluder = &occluder;

	Array<Occluder *> lights;
	for (int i = 0; i < splist.size(); i++)
	{
		if (splist[i]->light_power() > 0)
			lights.push(splist[i]);
	}
	scene.build_lights(lights);

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

	fipImage *bmp = camera.create_image();
	bmp->save("output.png");
	delete bmp;

	//--------------------------------------------------------------------------------
	// Free memory for scene
	//--------------------------------------------------------------------------------
	for (int i = 0; i < splist.s; i++)
		delete splist[i];

	for (int i = 0; i < occluder.list.s; i++)
		delete occluder.list[i];

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