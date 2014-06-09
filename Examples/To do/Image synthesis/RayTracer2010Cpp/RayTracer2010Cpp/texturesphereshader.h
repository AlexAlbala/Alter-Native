#pragma once
#include "shader.h"
#include <string>

using namespace std;

struct Sampler;
struct Ray;

struct TextureSphereShader : public Shader
{
	Sampler *tex;
	vect3d specular_color;
	double specular_coeff;

	TextureSphereShader(string fn);
	virtual void colorize(Ray &r);
};
