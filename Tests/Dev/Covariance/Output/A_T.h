#pragma once
#include "System/System.h"
#include "CovIEnumerable_T.h"
#include "CovIEnumerator_T.h"
#include "System/Console.h"
#include "B_T.h"

using namespace System;
namespace Covariance {
	namespace _Internal {

		//The classes defined in namespace _Internal are internal types.
		//DO NOT modify this code

		template<typename T>
		class A_T_Base : public virtual CovIEnumerable_T<T>, public virtual Object{
			public:
			Object* Get(){
				Console::WriteLine(new String("A<T>.Get()"));
				return new Covariance::B_T<T>();
			}
			public:
			A_T_Base()
			{
			}
		};

		template<typename T, bool>
		class A_T  {
		};

		//Basic types template type
		template<typename T>
		class A_T<T, true> : public A_T_Base<T>{
			public:
			inline CovIEnumerator_T<T>* Get(){
				Object* var_tmp = A_T_Base<T>::Get();
				return dynamic_cast<CovIEnumerator_T<T>*>(var_tmp);
			}
		};

		//Generic template type
		template<typename T>
		class A_T<T, false> : public virtual A_T_Base<Object>{
			public:
			inline CovIEnumerator_T<T>* Get(){
				Object* var_tmp = A_T_Base<Object>::Get();
				return dynamic_cast<CovIEnumerator_T<T>*>(var_tmp);
			}
		};
	}

	//Type definition
	template<typename T>
	class A_T : public _Internal::A_T<T, IsBasic(T)>{
	};
}
