#pragma once
#include "../System.h"
#include "IteratorCXX.h"
#include <iterator>
using namespace std;
using namespace System;

namespace System{
	namespace Collections{

		class IEnumerable : public virtual Object{
		public:
			virtual Object* GetEnumerator()=0;
		
			iteratorcxx* begin()
			{
				return new iteratorcxx((IEnumerator*)GetEnumerator());
			}
			iteratorcxx* end()
			{
				return new iteratorcxx(0);
			}
		};
	}
}