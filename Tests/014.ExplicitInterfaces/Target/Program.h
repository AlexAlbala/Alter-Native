#pragma once
#include "System/System.h"
#include "C.h"
#include "IA.h"
#include "IB.h"

using namespace System;
namespace NestedClasses{
	class Program : public virtual Object, public virtual gc_cleanup
	{
		public:
			static void Main(String* args[]);
	};
}
