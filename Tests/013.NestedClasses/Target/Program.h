#pragma once
#include "System/System.h"
#include "ParentClass.h"

using namespace System;
namespace NestedClasses {
	class Program : public virtual Object
	{
		public:
			static void Main(String* args[]);
	};
}
