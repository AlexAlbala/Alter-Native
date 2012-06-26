#pragma once
#include "System/System.h"
#include "System/Random.h"

using namespace System;
namespace List{

	class Utils : public Object
	{
		public:
		static gc_ptr<Random> random;
	};
}