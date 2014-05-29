#pragma once
#include "global.h"
#include "occluder.h"
#include "array.h"

struct KDTreeOccluder;

struct GroupTree : public Occluder
{
	KDTreeOccluder *tree;
	virtual ~GroupTree();

	GroupTree(Array<Occluder*> &list, Shader *s);
	virtual void trace(Ray &r);
};
