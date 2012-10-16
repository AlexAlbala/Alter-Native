#include "Program.h"
namespace File {
	void Program::Main(String* args[]){
		Program* p = new Program();
		p->Run();
	}
	void Program::Run()
	{
		StreamWriter* sw = new StreamWriter(new String("test.txt"));
		sw->WriteLine(new String("Hi! I'm a stream writer"));
		sw->Flush();
		sw->Close();
		StreamReader* sr = new StreamReader(new String("test.txt"));
		String* text = sr->ReadToEnd();
		sr->Close();
		Console::WriteLine(text);
	}

}
