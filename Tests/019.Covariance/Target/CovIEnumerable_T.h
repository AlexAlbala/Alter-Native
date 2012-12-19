#pragma once
#include "System/System.h"
#include "CovIEnumerator_T.h"

using namespace System;
namespace Covariance {
	template<typename T>
	class CovIEnumerable_T : public virtual Object{
		public:
			virtual Object* Get() = 0;
	};
}
