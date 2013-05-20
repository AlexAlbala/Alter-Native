#pragma once
#include "Object.h"
#include "String.h"

namespace System {
	class Math : public Object
	{
	public:
		static double Max(double val1, double val2);
		static double Sqrt(double val);
	};
}