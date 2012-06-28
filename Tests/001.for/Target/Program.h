#pragma once
#include "System/System.h"
#include "System/Console.h"

using namespace System;
namespace For{

	class Program : public Object, gc_cleanup
	{
		public:
			static void Main(String args[]);
	};
}