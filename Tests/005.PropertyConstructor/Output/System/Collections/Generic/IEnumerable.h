#pragma once

#include "../IEnumerable.h"
#include "IteratorCXX.h"
#include <iterator>
using namespace std;
using namespace System_Collections;

namespace System_Collections_Generic{

template<typename T> class IEnumerable_T : public virtual IEnumerable{
public:
	virtual IEnumerator_T<T>* GetEnumerator()=0;

	iteratorcxx<T> begin()
	{
		return iteratorcxx<T>(GetEnumerator());
	}
	iteratorcxx<T> end()
	{
		return iteratorcxx<T>(0);
	}
};
}