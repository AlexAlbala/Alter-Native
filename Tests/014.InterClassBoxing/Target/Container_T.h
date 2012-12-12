#pragma once
#include "System/System.h"

using namespace System;
namespace InterClassBoxing {
	namespace _Internal {

		//The classes defined in namespace _Internal are internal types.
		//DO NOT modify this code

		template<typename T>
		class Container_T_Base : public virtual Object{
			private:
			TypeDecl(T) data;
			public:
			void Set(TypeDecl(T) t){
				this->data = t;
			}
			public:
			TypeDecl(T) Get()
			{
				return this->data;
			}
			public:
			Container_T_Base()
			{
			}
		};

		template<typename T, bool>
		class Container_T  {
		};

		//Basic types template type
		template<typename T>
		class Container_T<T, true> : public Container_T_Base<T>{
		};

		//Generic template type
		template<typename T>
		class Container_T<T, false> : public virtual Container_T_Base<Object>{
			public:
			inline void Set(T* t){
				Container_T_Base<Object>::Set((Object*)(t));
			}
			public:
			inline T* Get() {
				Object* var_tmp = Container_T_Base<Object>::Get();
				return dynamic_cast<T*>(var_tmp);
			}
		};
	}

	//Type definition
	template<typename T>
	class Container_T : public _Internal::Container_T<T, IsBasic(T)>{
	};
}
