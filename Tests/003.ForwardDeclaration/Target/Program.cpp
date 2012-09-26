#include "Program.h"
namespace ForwardDeclaration {
	void Program::Main(String* args[]){
		Program* p = new Program();
		p->Run();
	}
	void Program::Run()
	{
		ClassA* a = new ClassA();
		String* hello = a->SayHello();
		Console::WriteLine(hello);
	}

}
