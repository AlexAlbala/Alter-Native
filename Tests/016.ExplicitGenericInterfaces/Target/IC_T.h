#pragma once
#include "System/System.h"

using namespace System;
namespace ExplicitGenericInterfaces {
	template<typename T>
	class IC_T : public virtual Object{
		public:
			virtual TypeDecl(T) f() = 0;
	};
}
