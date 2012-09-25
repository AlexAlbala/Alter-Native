#pragma once
#include "System/System.h"
#include "IA.h"
#include "IB.h"
#include "IC.h"
#include "System/Console.h"

using namespace System;
namespace NestedClasses{
	class C : public virtual IC, public virtual Object, public virtual gc_cleanup
	{
		public:
			int f();

		//START Nested Class: _nested_IA****************
		public:
		class _nested_IA : public virtual IA{
			private:
			void f(){
				Console::WriteLine(new String("a"));
			}
		};
		//END Nested Class *********************

		private:
			_nested_IA __nested_ia;
		public:
			operator IA*();

		//START Nested Class: _nested_IB****************
		public:
		class _nested_IB : public virtual IB{
			private:
			void f(){
				Console::WriteLine(new String("b"));
			}
		};
		//END Nested Class *********************

		private:
			_nested_IB __nested_ib;
		public:
			operator IB*();
	};
}
