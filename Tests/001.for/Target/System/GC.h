#pragma once

#include "Object.h"
#include "../gc/include/gc_cpp.h"


class GC : public Object
{
public:	
	static void Collect();
};