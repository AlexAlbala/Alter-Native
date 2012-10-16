#pragma once

#include "../IEnumeratorCXX.h"
using namespace System::Collections;

namespace System{
	namespace Collections{
		namespace Generic{

			template<typename T> class IEnumerator_T : public IEnumerator{
			
			public:	
				virtual BoxDecl(T) getCurrent() = 0;//OJO CUANDO ES TIPO BASICO SE NECESITA UN BOX AQUI !				
				
				operator IEnumerator_T<Object>*()
				{
					return dynamic_cast<IEnumerator_T<Object>*>(this);
				}

				template <class Q> 
				operator IEnumerator_T<Q> () const
				{ 
					return IEnumerator_T<Q>((IEnumerator_T<Q>)this); 
				}

				//virtual typename Boxing<T,IsFundamentalType<T>::result>::Type getCurrent() = 0;
			};
		}
	}
}