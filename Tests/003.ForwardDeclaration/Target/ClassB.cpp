#include "ClassB.h"
namespace ForwardDeclaration {
	ClassB::ClassB(ClassA* a){
		this->a = a;
	}
	String* ClassB::SayHello()
	{
		return new String("Hello I am B");
	}

}
