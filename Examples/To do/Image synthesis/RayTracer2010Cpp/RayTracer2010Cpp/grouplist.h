#pragma once
#include "array.h"
#include "global.h"
#include "occluder.h"

struct GroupList : public Occluder
{
	Array<Occluder*> list;

	void add(Occluder *o)
	{
		list.push(o);
	}

	virtual void trace(Ray &r)
	{
		for (int i = 0; i < list.s; i++)
			list[i]->trace(r);
	}
};
