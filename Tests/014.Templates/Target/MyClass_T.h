#pragma once
#include "System/System.h"

using namespace System;
namespace Templates {
	namespace _Internal {

		//The classes defined in namespace _Internal are internal types.
		//DO NOT modify this code

		template<typename T>
		class MyClass_T_Base : public virtual Object, public virtual gc_cleanup{
			private:
			T data;
			public:
			void set(T data){
				this->data = data;
			}
			public:
			T get()
			{
				return this->data;
			}
		};

		template<typename T, bool>
		class MyClass_T  {
		};

		//Basic types template type
		template<typename T>
		class MyClass_T<T, true> : public MyClass_T_Base<T>{
		};

		//Generic template type
		template<typename T>
		class MyClass_T<T, false> : public virtual MyClass_T_Base<Object*>{
			public:
			inline void set(T* data){
				return MyClass_T_Base<Object*>::set((Object*)(data));
			}
			public:
			inline T* get() {
				Object* var_tmp = MyClass_T_Base<Object*>::get();
				return dynamic_cast<T*>(var_tmp);
			}
		};
	}

	//Type definition
	template<typename T>
	class MyClass_T : public _Internal::MyClass_T<T, IsFundamentalType<T>::result>{
	};
}
