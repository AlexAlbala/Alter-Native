#pragma once
#include "shader.h"
#include <string>

using namespace std;

struct Sampler;
struct Ray;

struct TextureShader : public Shader
{
	Sampler *tex;
	vect3d specular_color, ambient_light;
	double specular_coeff, diffuse_coeff;

	TextureShader(string fn);

	virtual void colorize(Ray &r);
};
