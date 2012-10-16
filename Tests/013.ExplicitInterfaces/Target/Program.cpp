#include "Program.h"
namespace NestedClasses {
	void Program::Main(String* args[]){
		C* c = new C();
		IA* a = *c;
		a->f();
		IB* b = *c;
		b->f();
		c->f();
	}

}
