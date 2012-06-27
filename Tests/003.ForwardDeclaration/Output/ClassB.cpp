#include "ClassB.h"
namespace ForwardDeclaration{

	ClassB::ClassB(ClassA* a){
		this->a = a;
	}
	String ClassB::SayHello()
	{
		return "Hello I am B";
	}

}