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
				virtual IEnumerator_T<TypeArg(T)>* GetEnumerator()=0;
			
				iteratorcxx<TypeArg(T)> begin()
				{
					return iteratorcxx<T>(GetEnumerator());
				}
				iteratorcxx<TypeArg(T)> end()
				{
					return iteratorcxx<T>(0);
				}
			};
		}
	}
}