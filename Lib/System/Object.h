#pragma once
//TODO: If String.h is added, the compiler crashes !
//#include "String.h"

namespace System{
	
	template<typename T>
	class Box_T;


	template<typename T>
	Box_T<T>* Box(T t)
	{
		return new Box_T<T>(t);
	}

	//Forward declarations
	class String;
	

	class Object
	{
	private:
		Box_T<int>* _box_i;
	public:
		Object(void);
		Object(int i);
		operator int();
		Object& operator=(int i);
		//Object* operator=(int i);
		~Object(void);
		virtual String* ToString(void);
	};
}

namespace System{

	template <typename T>
	class Box_T : public Object {
	private:
		T data;
	public:	
		Box_T(T t) : data(t) {}
		Box_T(T* t) : data(*t) {} 	
		operator T () {
			return data;
		}

		T operator =(Box_T<T>  other){
			return data;
		}

		Box_T<T>& operator =(T other){
			return new Box_T<T>(other);
		}

		virtual String* ToString()
		{
			String* s = new String(data);
			return s;
		}
	};
}
