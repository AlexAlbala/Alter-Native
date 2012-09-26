#include "C_T.h"
namespace ExplicitGenericInterfaces {
	Object* C_T_Base::f(){
		Console::WriteLine(new String("c"));
		return this->value;
	}
	C_T_Base::operator IA*() {
		return &__interface_ia;
	}
	C_T_Base::operator IB_T<Object>*() {
		return &__interface_ib_t;
	}

}
