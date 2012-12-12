#include "Program.h"
namespace Statements {
	void Program::Main(String* args[]){
		Program* p = new Program();
		p->For();
		p->While();
		p->DoWhile();
	}
	void Program::For()
	{
		Console::WriteLine(new String("Testing for"));
		for (int i = 0; i < 5; i += 1) {
			Console::WriteLine(i);
		}
	}
	void Program::While()
	{
		Console::WriteLine(new String("Testing While"));
		for (int i = 0; i < 5; i += 1) {
			Console::WriteLine(i);
		}
	}
	void Program::DoWhile()
	{
		Console::WriteLine(new String("Testing dowhile"));
		int i = 0;
		do {
			Console::WriteLine(i);
			i += 1;
		}
		while (i < 5);
	}

}
