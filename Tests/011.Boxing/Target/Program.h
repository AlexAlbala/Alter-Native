#pragma once
#include "System/System.h"
#include "A.h"
#include "System/Console.h"

using namespace System;
namespace Boxing {
	class Program : public virtual Object
	{
		public:
			static void Main(String* args[]);
		private:
		static void Box();
		private:
		static void UnBox();
		private:
		static void f(Object* arg);
	};
}
