#pragma once
//TODO: If String.h is added, the compiler crashes !
//#include "String.h"

namespace System{
	//Forward declarations
	class String;

	class Object
	{
	public:
		Object(void);
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
		operator T& () {
			return data;
		}

		/*operator T () {
			return data;
		}*/

		T operator =(Box_T<T>  other){
			return data;
		}

		Box_T<T>& operator =(T other){
			return new Box_T<T>(other);
		}
	};
}
