#pragma once

#include "../IEnumerable.h"
#include "IteratorCXX.h"
#include <iterator>
using namespace std;
using namespace System::Collections;

namespace System{
	namespace Collections{
		namespace Generic{
			
			template<typename T> class IEnumerable_T : public virtual IEnumerable{
			public:
				virtual IEnumerator_T<typename TypeTrait<T, true>::Type>* GetEnumerator()=0;
			
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
	}
}