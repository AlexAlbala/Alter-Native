#include "inteface.h"
#include <typeinfo>

class Object {
public:
	virtual void PrintName() {
		std::cout << "I'm an Object" << std::endl;
	}
};

class String : public Object { 
public:
	void PrintName() {
		std::cout << "I'm a String" << std::endl;
	}
};

// Implementing covariant interface. 
template<typename T>
class Sample2_Base : public virtual ICovariant<T>, public virtual IContravariant<T> {
protected:
	T* data;
public:
	Sample2_Base() { }
	void Put(T* o) {
		data = o;
	}
	T* Get() { 
		std::cout << "Sample2_Base " << typeid(T).name() << std::endl;
		return data; 
	}
};


template<typename T>
class Sample2 : private virtual Sample2_Base<Object>, public virtual ICovariant<T>, public virtual IContravariant<T> {
public:
	Sample2() { }
	void Put(T* o) {
		Sample2_Base::Put((Object*)o);
	}
	T* Get() {
		std::cout << "Sample2 " << typeid(T).name() << std::endl;
		return (T*)Sample2_Base<Object>::Get();
	}

};

void TestCovariant() {
	Sample2<Object>* siobj = new Sample2<Object>();
	Sample2<String>* sistr = new Sample2<String>();

	siobj->Put(new Object());
	sistr->Put(new String());

	ICovariant<Object>* iobj = siobj;
	ICovariant<String>* istr = sistr;

	Object* o = iobj->Get();
	o->PrintName();		
	String* s = istr->Get();
	s->PrintName();
	
	// You can assign istr to iobj because 
	// the ICovariant interface is covariant.
	iobj = (ICovariant<Object>*)istr;			//A SACO CON UN CAST

	o = iobj->Get();
	o->PrintName();
}

void TestContravariant() {
	Sample2<Object>* siobj = new Sample2<Object>();
	Sample2<String>* sistr = new Sample2<String>();

	siobj->Put(new Object());
	sistr->Put(new String());

	IContravariant<Object>* iobj = siobj;
	IContravariant<String>* istr = sistr;

	iobj->Put(new Object());
	istr->Put(new String());

	Object* o = siobj->Get();
	o->PrintName();		
	String* s = sistr->Get();
	s->PrintName();
	
    // You can assign iobj to istr because 
    // the IContravariant interface is contravariant.
	istr = (IContravariant<String>*)iobj;			//A SACO CON UN CAST

	istr->Put(new String());

	o = siobj->Get();
	o->PrintName();	
}

int main(void) {
	TestCovariant();
	TestContravariant();
	return 0;
}