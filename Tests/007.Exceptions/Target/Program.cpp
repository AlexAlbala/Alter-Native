#include "Program.h"
namespace Exceptions {
	void Program::Main(String* args[]){
		Program* p = new Program();
		p->catchException();
		p->catchAndFinally();
	}
	void Program::ThrowException()
	{
		throw new NotImplementedException(new String("I'm not implemented"));
	}
	void Program::catchException()
	{
		try {
			this->ThrowException();
		}
		catch (Exception* e) {
			Console::WriteLine(e->Message);
		}
	}
	void Program::catchAndFinally()
	{
		try {
			this->ThrowException();
		}
		catch (Exception* e) {
			Console::WriteLine(e->Message);
		}
		//Finally block deleted
		{
			Console::WriteLine(new String("Finally catched"));
		}
		try {
		}
		catch (Exception* e) {
			Console::WriteLine(e->Message);
		}
		//Finally block deleted
		{
			Console::WriteLine(new String("Finally non catched"));
		}
	}

}
