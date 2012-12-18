#pragma once
#include "System/System.h"
#include "A.h"
#include "System/Console.h"

using namespace System;
namespace ImplicitCast {
	class C : public virtual A, public virtual Object
	{
		public:
		virtual void f();
	};
}
