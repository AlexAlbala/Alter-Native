#pragma once

#include "../IEnumeratorCXX.h"
using namespace System_Collections;

namespace System_Collections_Generic{

template<typename T> class IEnumerator_T : public IEnumerator{

public:	
	virtual Box_T<T>* getCurrent() = 0;
};
}