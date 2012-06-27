#include "ClassB.h"
namespace ForwardDeclaration{

	ClassB::ClassB(gc_ptr<ClassA> a){
		this->a = a;
	}
	String ClassB::SayHello()
	{
		return "Hello I am B";
	}

}