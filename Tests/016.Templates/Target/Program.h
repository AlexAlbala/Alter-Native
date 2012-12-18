#pragma once
#include "System/System.h"
#include "A.h"
#include "MyClass_T.h"
#include "B.h"
#include "System/Console.h"

using namespace System;
namespace Templates {
	class Program : public virtual Object
	{
		public:
			static void Main(String* args[]);
	};
}
