#include "Program.h"
namespace ExplicitGenericInterfaces {
	void Program::Main(String* args[]){
		C_T<A>* c = new C_T<A>();
		IA* a = *c;
		a->f();
		IB_T<A>* b = *c;
		b->f();
		c->f();
		C_T<int>* _int_c = new C_T<int>();
		IA* _int_a = *_int_c;
		_int_a->f();
		IB_T<int>* _int_b = *_int_c;
		_int_b->f();
		_int_c->f();
		C_T<float>* _float_c = new C_T<float>();
		IA* _float_a = *_float_c;
		_float_a->f();
		IB_T<float>* _float_b = *_float_c;
		_float_b->f();
		_float_c->f();
	}

}
