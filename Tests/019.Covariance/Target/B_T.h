#pragma once
#include "System/System.h"
#include "CovIEnumerator_T.h"
#include "System/Console.h"

using namespace System;
namespace Covariance {
	namespace _Internal {

		//The classes defined in namespace _Internal are internal types.
		//DO NOT modify this code

		template<typename T>
		class B_T_Base : public virtual CovIEnumerator_T<T>, public virtual Object{
			public:
			B_T_Base(){
				Console::WriteLine(new String("Building B..."));
			}
		};

		template<typename T, bool>
		class B_T  {
		};

		//Basic types template type
		template<typename T>
		class B_T<T, true> : public B_T_Base<T>{
			public:
			inline B_T() : B_T_Base<T>()
			{
			}
		};

		//Generic template type
		template<typename T>
		class B_T<T, false> : public virtual B_T_Base<Object>{
			public:
			inline B_T() : B_T_Base<Object>()
			{
			}
		};
	}

	//Type definition
	template<typename T>
	class B_T : public _Internal::B_T<T, IsBasic(T)>{
		public:
		B_T() : _Internal::B_T<T, IsBasic(T)>(){
		}
	};
}
