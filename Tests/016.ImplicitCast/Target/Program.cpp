#include "Program.h"
namespace ImplicitCast {
	void Program::Main(String* args[]){
		A* ea = new A();
		B* eb = new B();
		C* ec = new C();
		ea->f();
		ea = eb;
		ea->f();
		ea = ec;
		ea->f();
	}

}
