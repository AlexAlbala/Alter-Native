#include "Program.h"
namespace ForwardDeclaration{

	void Program::Main(String args[]){
		gc_ptr<Program> p (new(gc)Program());
		p->Run();
	}
	void Program::Run()
	{
		gc_ptr<ClassA> a (new(gc)ClassA());
		String hello = a->SayHello();
		Console::WriteLine(hello);
	}

}