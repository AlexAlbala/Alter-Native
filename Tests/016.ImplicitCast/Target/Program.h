#pragma once
#include "System/System.h"
#include "A.h"
#include "B.h"
#include "C.h"

using namespace System;
namespace ImplicitCast {
	class Program : public virtual Object, public virtual gc_cleanup
	{
		public:
			static void Main(String* args[]);
	};
}
