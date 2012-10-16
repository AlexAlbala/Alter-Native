#include "C.h"
namespace NestedClasses {
	int C::f(){
		Console::WriteLine(new String("c"));
		return 0;
	}
	C::operator IA*() {
		return &__interface_ia;
	}
	C::operator IB*() {
		return &__interface_ib;
	}

}
