#pragma once

#include "MyIterator.h"
#include <iterator>
using namespace std;

namespace System::Collections{

class IEnumerable{
public:
	virtual IEnumerator* GetEnumerator()=0;

	myiterator<Object>* begin()
	{
		return myiterator<T>(GetEnumerator());
	}
	myiterator<Object>* end()
	{
		return myiterator<T>(0);
	}
};
}