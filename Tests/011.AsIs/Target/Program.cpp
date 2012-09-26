#include "Program.h"
namespace AsIs {
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
		if (is_inst_of<Person*>(i)) {
			Person* _j = as_cast<Person*>(i);
			Console::WriteLine(_j->getName());
		}
		if (is_inst_of<Person*>(a)) {
			Person* _a = as_cast<Person*>(a);
			Console::WriteLine(_a->getName());
		}
		Console::WriteLine(new String("Cast to specific class"));
		if (is_inst_of<John*>(i)) {
			John* _j2 = as_cast<John*>(i);
			Console::WriteLine(_j2->getName());
		}
		if (is_inst_of<Anne*>(a)) {
			Anne* _a2 = as_cast<Anne*>(a);
			Console::WriteLine(_a2->getName());
		}
		if (is_inst_of<John*>(j)) {
			Console::WriteLine(new String("error"));
		}
	}

}
