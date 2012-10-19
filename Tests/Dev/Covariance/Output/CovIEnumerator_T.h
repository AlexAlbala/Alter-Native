#pragma once
#include "System/System.h"

using namespace System;
namespace Covariance {
	template<typename T>
	class CovIEnumerator_T : public virtual Object, public virtual gc_cleanup{

		public:
		template<typename Q>
		operator CovIEnumerator_T<Q>&()
		{
			return static_cast<CovIEnumerator_T<Q>*>(this);
		}

		template<typename Q>
		CovIEnumerator_T<Q>& operator =(CovIEnumerator_T<Object>* other)
		{
			return static_cast<CovIEnumerator_T<Q>*>(*other);
		}
	};
}
