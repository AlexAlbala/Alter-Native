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

void Console::Write(gc_ptr<String> s) {
	std::cout << s->Data;
}

void Console::WriteLine(char* txt) {
	std::cout << txt << std::endl;
}

void Console::WriteLine(int i) {
	std::cout << i << std::endl;
}

void Console::WriteLine(gc_ptr<String> s) {
	std::cout << s->Data << std::endl;
}

char* Console::ReadLine() {
	gc_ptr<String> s (new(gc) String());
	s->Length=256;
	s->Data = new char[s->Length];
	std::cin.getline (s->Data,256);
	return s->Data;
}

}