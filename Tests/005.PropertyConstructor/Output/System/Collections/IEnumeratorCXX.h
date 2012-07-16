#pragma once
#include "../System.h"

namespace System_Collections{
class IEnumerator{

public:
	virtual void Reset() = 0;
	virtual bool MoveNext() = 0;
	virtual Object* getCurrent() = 0;
};
}