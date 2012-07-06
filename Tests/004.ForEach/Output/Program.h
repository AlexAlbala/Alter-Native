#pragma once
#include "System/System.h"
#include "System/Console.h"
#include "System/Collections/Generic/List.h"

using namespace System;
using namespace System::Collections::Generic;
namespace ForEach{

	class Program : public Object, public gc_cleanup
	{
		public:
			static void Main(String args[]);
	};
}