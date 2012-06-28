#pragma once
	#include "System/System.h"
	#include "MyList.h"
	#include "System/Collections/Generic/List.h"
	#include "System/Console.h"

	using namespace System.Collections.Generic;
	using namespace System;
	namespace CollectionsExample{

		class Program : public Object, gc_cleanup
		{
			public:
				static void Main(String args[]);
		};
	}