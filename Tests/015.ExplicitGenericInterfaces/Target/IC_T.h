#pragma once
#include "System/System.h"

using namespace System;
namespace ExplicitGenericInterfaces {
	template<typename T>
	class IC_T : public virtual Object, public virtual gc_cleanup{
		public:
			virtual typename DeRefBasicType<T*>::Type f() = 0;
	};
}
