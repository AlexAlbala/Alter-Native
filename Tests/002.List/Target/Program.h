#pragma once
#include "System/System.h"
#include "System/Collections/Generic/List.h"
#include "Program.h"
#include "Node.h"
#include "System/Console.h"

using namespace System;
namespace List{

	class Program : public Object
	{
		private:
			gc_ptr<List> list;
		public:
			static void Main(String args[]);
		private:
			void Run();
		private:
			void Sort();
		private:
			void printList();
	};
}