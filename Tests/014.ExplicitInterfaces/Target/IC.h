#pragma once
#include "System/System.h"

using namespace System;
namespace NestedClasses {
	class IC : public virtual Object{
		public:
			virtual int f() = 0;
	};
}
