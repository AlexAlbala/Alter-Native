#pragma once

#include "../IEnumerable.h"
#include "MyIterator.h"
#include <iterator>
using namespace std;
using namespace System_Collections;

namespace System_Collections_Generic{

template<typename T> class IEnumerable_T : public IEnumerable{
public:
	virtual IEnumerator_T<T>* GetEnumerator()=0;

	myiterator<T> begin()
	{
		return myiterator<T>(GetEnumerator());
	}
	myiterator<T> end()
	{
		return myiterator<T>(0);
	}
};
}