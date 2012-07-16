#pragma once
#include "../System.h"
#include "Generic/MyIterator.h"
#include <iterator>
using namespace std;
using namespace System;
using namespace System_Collections_Generic;

namespace System_Collections{

class IEnumerable{
public:
	virtual IEnumerator* GetEnumerator()=0;

	myiterator<Object>* begin()
	{
		return new myiterator<Object>(GetEnumerator());
	}
	myiterator<Object>* end()
	{
		return new myiterator<Object>(0);
	}
};
}