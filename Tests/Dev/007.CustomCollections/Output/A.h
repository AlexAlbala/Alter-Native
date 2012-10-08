#pragma once
#include "System/System.h"

using namespace System;
namespace CustomCollections {
	class A : public virtual Object, public virtual gc_cleanup
	{
		public:
			int value;
		public:
			A();
	};
}
