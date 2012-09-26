#pragma once
#include "System/System.h"
#include "IA.h"
#include "IB_T.h"
#include "IC_T.h"
#include "System/Console.h"

using namespace System;
namespace ExplicitGenericInterfaces {
	class C_T_Base : public virtual IC_T<Object>, public virtual Object, public virtual gc_cleanup
	{
		private:
			Object* value;
		public:
			Object* f();

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


		//START Explicit interface: _interface_IB_T ****************
		public:
		template<typename T>
		class _interface_IB_T : public virtual IB_T<T>{
			private:
			void f(){
				Console::WriteLine(new String("b"));
			}
		};
		private:
			_interface_IB_T<Object> __interface_ib_t;
		public:
			operator IB_T<Object>*();
		//END Explicit interface *********************

	};

	//Generic template type
	template<typename T>
	class C_T : public virtual IC_T<T>, public virtual Object, public virtual gc_cleanup, public virtual C_T_Base{
		public:
		inline T* f(){
			Object* var_tmp = C_T_Base::f();
			return dynamic_cast<T*>(var_tmp);
		}
		inline operator IA*() {
			return (IA*)(C_T_Base::operator IA*());
		}
		inline operator IB_T<T>*() {
			return (IB_T<T>*)(C_T_Base::operator IB_T<Object>*());
		}
	};
}
