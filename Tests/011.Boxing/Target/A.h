#pragma once
#include "System/System.h"
#include "System/Console.h"

using namespace System;
namespace Boxing {
	class A : public virtual Object, public virtual gc_cleanup
	{
		public:
			void f(Object* arg);
	};
}
