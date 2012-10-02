#pragma once

#include "../IEnumeratorCXX.h"
using namespace System::Collections;

namespace System{
	namespace Collections{
		namespace Generic{

			template<typename T> class IEnumerator_T : public IEnumerator{
			
			public:	
				virtual typename Boxing<T,IsFundamentalType<T>::result>::Type getCurrent() = 0;
			};
		}
	}
}