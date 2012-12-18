#pragma once
#include "System/System.h"
#include "IA.h"
#include "IB.h"
#include "IC.h"
#include "System/Console.h"

using namespace System;
namespace NestedClasses {
	class C : public virtual IC, public virtual Object
	{
		public:
			int f();

		//START Explicit interface: _interface_IA ****************
		public:
		class _interface_IA : public virtual IA{
			private:
			void f(){
				Console::WriteLine(new String("a"));
			}
		};
		private:
			_interface_IA __interface_ia;
		public:
			operator IA*();
		//END Explicit interface *********************


		//START Explicit interface: _interface_IB ****************
		public:
		class _interface_IB : public virtual IB{
			private:
			void f(){
				Console::WriteLine(new String("b"));
			}
		};
		private:
			_interface_IB __interface_ib;
		public:
			operator IB*();
		//END Explicit interface *********************

	};
}
