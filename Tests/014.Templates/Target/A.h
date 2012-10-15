#pragma once
#include "System/System.h"

using namespace System;
namespace Templates {
	class A : public virtual Object, public virtual gc_cleanup
	{
		public:
			String* getText();
	};
}
