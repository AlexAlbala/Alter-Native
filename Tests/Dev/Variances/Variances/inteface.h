#pragma once
#include <iostream>

template<typename T>
class ICovariant {
public:
	virtual T* Get() = 0;
};

template<typename T>
class IExtCovariant : ICovariant<T> {
public:
	virtual T* Get2() = 0;
};

template<typename T>
class IContravariant {
public:
	virtual void Put(T* o) = 0;
};

template<typename T>
class IExtContravariant : IContravariant<T> {
public:
	virtual void Put2(T* o) = 0;
};