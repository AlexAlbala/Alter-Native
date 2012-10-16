#pragma once
#include "System/System.h"
#include "C.h"
#include "A_T.h"
#include "CovIEnumerator_T.h"

using namespace System;
namespace Covariance {
	class Program : public virtual Object, public virtual gc_cleanup
	{
		public:
			static void Main(String* args[]);
	};
}
