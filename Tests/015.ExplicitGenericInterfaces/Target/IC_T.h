#pragma once
#include "System/System.h"

using namespace System;
namespace ExplicitGenericInterfaces {
	template<typename T>
	class IC_T : public virtual Object, public virtual gc_cleanup{
		public:
			virtual TypeTrait(T, false) f() = 0;
	};
}
