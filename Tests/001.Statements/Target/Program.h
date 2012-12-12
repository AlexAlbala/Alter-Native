#pragma once
#include "System/System.h"
#include "System/Console.h"

using namespace System;
namespace Statements {
	class Program : public virtual Object
	{
		public:
			static void Main(String* args[]);
		public:
			void For();
		public:
			void While();
		public:
			void DoWhile();
	};
}
