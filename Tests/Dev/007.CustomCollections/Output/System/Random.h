#pragma once

#include <stdlib.h>
#include "Object.h"
#include <time.h>

namespace System {

	class Random : public Object {	
	public:
		Random();
	public:
		float NextDouble();
	
	};
}