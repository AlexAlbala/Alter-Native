#pragma once
#include "System/System.h"
#include "System/Console.h"

using namespace System;
namespace ImplicitCast {
	class A : public virtual Object, public virtual gc_cleanup
	{
		public:
		virtual void f();
	};
}
