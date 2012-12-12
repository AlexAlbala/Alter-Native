#pragma once
#include "System/System.h"
#include "Person.h"
#include "System/Console.h"
#include "John.h"
#include "Anne.h"

using namespace System;
namespace AsIs {
	class Program : public virtual Object
	{
		public:
			static void Main(String* args[]);
		public:
			void AsIsTest();
	};
}
