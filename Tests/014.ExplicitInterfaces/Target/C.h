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
		//Nested Class: _nested_IA
		class _nested_IA : public virtual IA, public virtual Object, public virtual gc_cleanup{
			private:
			void _nested_IA::f(){
				Console::WriteLine(new String("a"));
			}
		};

		private:
			_nested_IA __nested_ia;
		public:
		operator IA* ();
		//Nested Class: _nested_IB
		class _nested_IB : public virtual IB, public virtual Object, public virtual gc_cleanup{
			private:
			void _nested_IB::f(){
				Console::WriteLine(new String("b"));
			}
		};

		private:
			_nested_IB __nested_ib;
		public:
		operator IB* ();
	};
}
