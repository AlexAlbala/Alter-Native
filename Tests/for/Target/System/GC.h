#pragma once

#include "Object.h"


class GC : public Object
{
public:	
	static void Collect();
};