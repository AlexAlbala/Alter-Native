#pragma once

class Object
{
public:
	Object(void);
	~Object(void);

	//TODO
	//virtual String ToString(void);
};

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
};
