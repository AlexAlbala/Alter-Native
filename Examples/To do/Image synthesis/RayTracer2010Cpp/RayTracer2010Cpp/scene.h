#pragma once
#include "occluder.h"
#include "array.h"
#include "shader.h"
#include "random.h"

struct LightInfo
{
	Occluder* obj;
	double power;
	double power_sum;
};

struct Scene
{
    Occluder *occluder;
	vect3f background_color;

	double light_power_total;
	Array<LightInfo> lights;
    
	Scene()
	{
		background_color = 1;
	}

    void trace(Ray &r)
    {
        occluder->trace(r);
    }

	void build_lights(Array<Occluder *> &lightlist)
	{
		lights.clear();
		light_power_total = 0;
		for (int i = 0; i < lightlist.s; i++)
		{
			LightInfo l;
			l.obj = lightlist[i];
			l.power = lightlist[i]->light_power();
			l.power_sum = light_power_total;
			lights.push(l);
			light_power_total += l.power;
		}
	}
	
	void get_light_emission(vect3d &pos, vect3d &norm, vect3d &color) 
	{
		double v = light_power_total * rand_mt_real1();

		for (int i = 0; i < lights.s; i++)
		{
			if (lights[i].power_sum <= v && v <= lights[i].power_sum + lights[i].power)
			{
				lights[i].obj->get_light_emission(pos, norm, color);
				color /= color[0] + color[1] + color[2]; // normalize color
				return;
			}
		}
	}
};
