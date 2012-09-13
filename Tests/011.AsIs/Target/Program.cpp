#include "Program.h"
namespace AsIs{
	void Program::Main(String* args[]){
		Program* p = new Program();
		p->AsIsTest();
	}
	void Program::AsIsTest()
	{
		Person* p = new Person();
		Console::Write(new String("Person: "));
		Console::WriteLine(p->getName());
		Object* i = new John();
		Object* a = new Anne();
		Object* j = new Person();
		Console::WriteLine(new String("Cast to super class"));
		if (isinstof<Person*>(i)) {
			Person* _j = ascast<Person*>(i);
			Console::WriteLine(_j->getName());
		}
		if (isinstof<Person*>(a)) {
			Person* _a = ascast<Person*>(a);
			Console::WriteLine(_a->getName());
		}
		Console::WriteLine(new String("Cast to specific class"));
		if (isinstof<John*>(i)) {
			John* _j2 = ascast<John*>(i);
			Console::WriteLine(_j2->getName());
		}
		if (isinstof<Anne*>(a)) {
			Anne* _a2 = ascast<Anne*>(a);
			Console::WriteLine(_a2->getName());
		}
		if (isinstof<John*>(j)) {
			Console::WriteLine(new String("error"));
		}
	}

}
