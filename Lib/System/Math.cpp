#include "Math.h"
#include <math.h>

namespace System {

	double Math::Max(double val1, double val2){
		return val1 > val2 ? val1 : val2;
	}

	double Math::Sqrt(double val){
		return sqrt(val);
	}
}