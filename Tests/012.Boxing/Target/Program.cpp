#include "Program.h"
namespace Boxing {
	void Program::Main(String* args[]){
		Program::Box();
		Program::UnBox();
	}
	void Program::Box()
	{
		int i = 123;
		Object* o = BOX<int>(i);
		i = 456;
		Program::f(BOX<int>(i));
		A* a = new A();
		a->f(BOX<int>(i));
		Console::WriteLine(i);
		Console::WriteLine(o);
	}
	void Program::UnBox()
	{
		Object* o = BOX<int>(123);
		int i = UNBOX<int>(o);
		o = BOX<int>(456);
		Console::WriteLine(i);
		Console::WriteLine(o);
	}
	void Program::f(Object* arg)
	{
		Console::WriteLine(new String("I'm P"));
		Console::WriteLine(arg);
	}

}
