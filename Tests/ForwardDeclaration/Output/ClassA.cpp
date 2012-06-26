#include "ClassA.h"
namespace ForwardDeclaration{

	ClassA::ClassA(){
		this->b = gc_ptr<ClassB> (new(gc)ClassB(this));
	}
	String ClassA::SayHello()
	{
		return this->b->SayHello();
	}

}