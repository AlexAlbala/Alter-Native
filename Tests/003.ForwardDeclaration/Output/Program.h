#pragma once
#include "System/System.h"
#include "Program.h"
#include "ClassA.h"
#include "System/Console.h"

using namespace System;
namespace ForwardDeclaration{

	class Program : public Object, public gc_cleanup
	{
		public:
			static void Main(String args[]);
		public:
			void Run();
	};
}