#pragma once
#include "System/System.h"

using namespace System;
namespace Templates{
	class MyClass_T_Base : public virtual Object, public virtual gc_cleanup
	{
		private:
			Object* data;
		public:
			void set(Object* data);
		public:
			Object* get();
	};

	//Generic template type
	template<typename T>
	class MyClass_T : public virtual Object, public virtual gc_cleanup, public virtual MyClass_T_Base{
		public:
		inline void set(T* data){
			return MyClass_T_Base::set((Object*)(data));
		}
		public:
		inline T* get() {
			Object* var_tmp = MyClass_T_Base::get();
			return dynamic_cast<T*>(var_tmp);
		}
	};
}
