#include "Program.h"
namespace NestedClasses {
	void Program::Main(String* args[]){
		ParentClass* p = new ParentClass();
		ParentClass::NestedClass1* n = new ParentClass::NestedClass1();
		ParentClass::NestedClass2* n2 = new ParentClass::NestedClass2();
		ParentClass::NestedClass2::NestedClass3* n3 = new ParentClass::NestedClass2::NestedClass3();
		p->f();
		n->f();
		n2->f();
		n3->f();
	}

}
