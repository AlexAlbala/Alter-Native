#include "Program.h"
namespace ExplicitGenericInterfaces {
	void Program::Main(String* args[]){
		C_T<A>* c = new C_T<A>();
		IA* a = *c;
		a->f();
		IB_T<A>* b = *c;
		b->f();
		c->f();
	}

}
