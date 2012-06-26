#pragma once
template<typename T> class IEnumerator{

public:
	virtual void Reset() = 0;
	virtual bool MoveNext() = 0;
	virtual T* Current() = 0;
};