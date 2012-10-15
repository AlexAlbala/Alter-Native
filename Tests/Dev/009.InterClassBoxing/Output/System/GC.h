#pragma once
#include "../gc/include/gc_cpp.h"
#include "Object.h"

namespace System{
	//Forward declaration
	class Object;
	class GC : public Object
	{
	public:	
		static void Collect();
	};
}