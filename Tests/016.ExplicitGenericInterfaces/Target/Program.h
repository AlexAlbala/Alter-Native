#pragma once
#include "System/System.h"
#include "A.h"
#include "C_T.h"
#include "IA.h"
#include "IB_T.h"

using namespace System;
namespace ExplicitGenericInterfaces {
	class Program : public virtual Object
	{
		public:
			static void Main(String* args[]);
	};
}
