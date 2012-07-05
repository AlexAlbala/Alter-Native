#pragma once

#include "../IEnumeratorCXX.h"
using namespace System::Collections;

namespace System::Collections::Generic{

template<typename T> class IEnumerator_T : public IEnumerator{

public:	
	virtual T* getCurrent() = 0;
};
}