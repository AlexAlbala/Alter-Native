#pragma once
#include "System/System.h"
#include "System/Console.h"

using namespace System;
namespace Arrays {
	class Program : public virtual Object
	{
		public:
			static void Main(String* args[]);
		public:
			void Test1(Array<int>* myArray);
		public:
			void Test2(Array<int>* myArray1, Array<long>* myArray2);
		public:
			void Finish(Array<int>* myArray1, Array<long>* myArray2, Array<char>* myArray3);
	};
}
