#pragma once
#include "System/System.h"
#include "MyClassA.h"

namespace Properties{

	class Program : public Object, public gc_cleanup
	{
		public:
			static void Main(String args[]);
	};
}