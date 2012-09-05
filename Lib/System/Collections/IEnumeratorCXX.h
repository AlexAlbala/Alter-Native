#pragma once
#include "../System.h"

namespace System{
	namespace Collections{
		class IEnumerator: public Object{
		
		public:
			virtual void Reset() = 0;
			virtual bool MoveNext() = 0;
			virtual Object* getCurrent() = 0;
		};
	}
}