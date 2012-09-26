#pragma once
#include "System/System.h"
#include "MyList.h"
#include "Node.h"
#include "System/Console.h"

using namespace System;
namespace List {
	class Program : public virtual Object, public virtual gc_cleanup
	{
		private:
			MyList* list;
		public:
			static void Main(String* args[]);
		private:
			void Run();
		private:
			void Sort();
		private:
			void printList();
	};
}
