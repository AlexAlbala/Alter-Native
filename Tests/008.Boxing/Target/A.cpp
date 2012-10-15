#include "A.h"
namespace Boxing {
	void A::f(Object* arg){
		Console::WriteLine(new String("I'm A"));
		Console::WriteLine(arg);
	}

}
