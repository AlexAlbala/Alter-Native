#pragma once
#include "System/System.h"
#include "System/Random.h"

using namespace System;
namespace List{

	class Utils : public Object, public gc_cleanup
	{
		public:
		static Random* random;
	};
}