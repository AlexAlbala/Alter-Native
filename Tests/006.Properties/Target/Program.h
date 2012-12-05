#pragma once
#include "System/System.h"
#include "MyClassA.h"

using namespace System;
namespace Properties {
	class Program : public virtual Object
	{
		public:
			static void Main(String* args[]);
	};
}
