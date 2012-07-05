#pragma once

#include "../IEnumerable.h"
#include "MyIterator.h"
#include <iterator>
using namespace std;
using namespace System::Collections;

namespace System::Collections::Generic{

template<typename T> class IEnumerable : public IEnumerable{
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
}