#include "Program.h"
namespace InterClassBoxing {
	void Program::Case1(){
		Container_T<int>* a = new Container_T<int>();
		int i = 5;
		a->Set(i);
		int j = a->Get();
		Console::WriteLine(j);
	}
	void Program::Case2()
	{
		Container_T<int>* a = new Container_T<int>();
		Object* i = BOX<int>(5);
		a->Set(UNBOX<int>(i));
		Object* j = BOX<int>(a->Get());
		Console::WriteLine(j);
	}
	void Program::Case3()
	{
		Container_T<Object>* a = new Container_T<Object>();
		int i = 5;
		a->Set(BOX<int>(i));
		int j = UNBOX<int>(a->Get());
		Console::WriteLine(j);
	}
	void Program::Case4()
	{
		Container_T<Object>* a = new Container_T<Object>();
		Object* i = BOX<int>(5);
		a->Set(i);
		Object* j = a->Get();
		Console::WriteLine(j);
	}
	void Program::Main(String* args[])
	{
		Program::Case1();
		Program::Case2();
		Program::Case3();
		Program::Case4();
	}

}
