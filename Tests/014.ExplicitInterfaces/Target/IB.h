#pragma once
#include "System/System.h"

using namespace System;
namespace NestedClasses{
	class IB : public virtual Object, public virtual gc_cleanup{
		public:
			virtual void f() = 0;
	};
}
