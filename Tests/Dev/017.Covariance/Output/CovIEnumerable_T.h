#pragma once
#include "System/System.h"
#include "CovIEnumerator_T.h"

using namespace System;
namespace Covariance {
	template<typename T>
	class CovIEnumerable_T : public virtual Object, public virtual gc_cleanup{
		public:
			virtual CovIEnumerator_T<T>* Get() = 0;
	};
}
