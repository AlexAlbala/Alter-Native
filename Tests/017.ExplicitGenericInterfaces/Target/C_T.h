#pragma once
#include "System/System.h"
#include "IA.h"
#include "IB_T.h"
#include "IC_T.h"
#include "System/Console.h"

using namespace System;
namespace ExplicitGenericInterfaces {
	namespace _Internal {

		//The classes defined in namespace _Internal are internal types.
		//DO NOT modify this code

		template<typename T>
		class C_T_Base : public virtual IC_T<T>, public virtual Object{

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
			operator IA*() {
				return &__interface_ia;
			}
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
				_interface_IB_T<T> __interface_ib_t;
			public:
			operator IB_T<T>*() {
				return &__interface_ib_t;
			}
			//END Explicit interface *********************

			private:
			TypeDecl(T) value;
			public:
			TypeDecl(T) f(){
				Console::WriteLine(new String("c"));
				return this->value;
			}
			public:
			C_T_Base()
			{
				Where_T_is_New(T);
			}
		};

		template<typename T, bool>
		class C_T  {
		};

		//Basic types template type
		template<typename T>
		class C_T<T, true> : public C_T_Base<T>{
			public:
			inline C_T() : C_T_Base<T>()
			{
			}
		};

		//Generic template type
		template<typename T>
		class C_T<T, false> : public virtual C_T_Base<Object>{
			public:
			inline T* f(){
				Object* var_tmp = C_T_Base<Object>::f();
				return dynamic_cast<T*>(var_tmp);
			}
			public:
			inline C_T() : C_T_Base<Object>()
			 {
			}
			inline operator IA*() {
				return (IA*)(C_T_Base<Object>::operator IA*());
			}
			inline operator IB_T<T>*() {
				return (IB_T<T>*)(C_T_Base<Object>::operator IB_T<Object>*());
			}
		};
	}

	//Type definition
	template<typename T>
	class C_T : public _Internal::C_T<T, IsBasic(T)>{
		public:
		C_T() : _Internal::C_T<T, IsBasic(T)>(){
		}
	};
}
