#include "MyClass_T.h"
namespace Templates {
	void MyClass_T_Base::set(Object* data){
		this->data = data;
	}
	Object* MyClass_T_Base::get()
	{
		return this->data;
	}

}
