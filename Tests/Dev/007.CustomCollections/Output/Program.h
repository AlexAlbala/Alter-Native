#pragma once
#include "System/System.h"
#include "MyList_T.h"
#include "System/Console.h"
#include "A.h"

using namespace System;
namespace CustomCollections {
	class Program : public virtual Object, public virtual gc_cleanup
	{
		public:
			static void Main(String* args[]);
	};
}
