#pragma once
#include "System/System.h"

using namespace System;
namespace Constraints {
	namespace _Internal {

		//The classes defined in namespace _Internal are internal types.
		//DO NOT modify this code

		template<typename T>
		class Example_T_Base : public virtual Object{
			public:
			Example_T_Base(){
				Where_T_is(T, Person);
			}
		};

		template<typename T, bool>
		class Example_T  {
		};

		//Basic types template type
		template<typename T>
		class Example_T<T, true> : public Example_T_Base<T>{
			public:
			inline Example_T() : Example_T_Base<T>()
			{
			}
		};

		//Generic template type
		template<typename T>
		class Example_T<T, false> : public virtual Example_T_Base<Object>{
			public:
			inline Example_T() : Example_T_Base<Object>()
			{
			}
		};
	}

	//Type definition
	template<typename T>
	class Example_T : public _Internal::Example_T<T, IsBasic(T)>{
		public:
		Example_T() : _Internal::Example_T<T, IsBasic(T)>(){
		}
	};
}
