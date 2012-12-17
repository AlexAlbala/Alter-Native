#pragma once
#include "System/System.h"
#include "System/Console.h"
#include "System/Exception.h"

using namespace System;
namespace ExceptionExample {
	class Program : public virtual Object
	{
		public:
		static void g(int i);
		public:
		static int f(int i);
		public:
			static void Main(String* args[]);
	};
}
