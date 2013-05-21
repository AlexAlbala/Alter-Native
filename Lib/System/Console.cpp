#include "Console.h"
#include "String.h"

#include <iostream>

namespace System {

	void Console::Write(char* txt) {
		std::cout << txt;
	}

	void Console::Write(int i) {
		std::cout << i;
	}

	void Console::Write(String* s) {
		std::cout << s->Data;
	}

	void Console::WriteLine(char* txt) {
		std::cout << txt << std::endl;
	}

	void Console::WriteLine(Array<char>* c) {
		std::cout << c->GetData() << std::endl;
	}

	void Console::WriteLine(int i) {
		std::cout << i << std::endl;
	}

	void Console::WriteLine(float f) {
		std::cout << f << std::endl;
	}

	void Console::WriteLine(double d) {
		std::cout << d << std::endl;
	}

	void Console::WriteLine(Object* o) {
		WriteLine(o->ToString());
	}

	void Console::WriteLine(String* s) {
		std::cout << s->Data << std::endl;
	}

	char* Console::ReadLine() {
		String* s = new String();
		s->Length=256;
		s->Data = new char[s->Length];
		std::cin.getline (s->Data,256);
		return s->Data;
	}

}