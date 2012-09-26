#include "ClassA.h"
namespace ForwardDeclaration {
	ClassA::ClassA(){
		this->b = new ClassB(this);
	}
	String* ClassA::SayHello()
	{
		return this->b->SayHello();
	}

}
