#pragma once
#include "System/System.h"

using namespace System;
namespace ExplicitGenericInterfaces {
	template<typename T>
	class IB_T : public virtual Object{
		public:
			virtual void f() = 0;
	};
}
