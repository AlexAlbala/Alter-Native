#pragma once
#include "System/System.h"
#include "System/Console.h"

using namespace System;
class Program : public virtual Object, public virtual gc_cleanup
{
	public:
	static Array<int>* fannkuch(int n);
	public:
		static void Main(String* args[]);
};
