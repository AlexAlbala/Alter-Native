#pragma once
#include "System/System.h"
#include "System/exceptions/systemException/NotImplementedException.h"
#include "System/Console.h"
#include "System/Exception.h"

using namespace System;
namespace Exceptions {
	class Program : public virtual Object
	{
		public:
			static void Main(String* args[]);
		public:
			void ThrowException();
		public:
			void catchException();
		public:
			void catchAndFinally();
	};
}
