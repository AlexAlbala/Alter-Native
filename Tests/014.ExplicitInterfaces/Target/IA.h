#pragma once
#include "System/System.h"

using namespace System;
namespace NestedClasses {
	class IA : public virtual Object{
		public:
			virtual void f() = 0;
	};
}
