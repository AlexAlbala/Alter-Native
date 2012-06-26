#define _IENUMERABLE_H_
#pragma once

#include "MyIterator.h"
#include <iterator>
using namespace std;

template<typename T> class IEnumerable{
public:
	virtual IEnumerator<T>* GetEnumerator()=0;

	myiterator<T> begin()
	{
		return myiterator<T>(GetEnumerator());
	}
	myiterator<T> end()
	{
		return myiterator<T>(0);
	}
};