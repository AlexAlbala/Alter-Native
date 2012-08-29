#include "Program.h"
namespace File{

	void Program::Main(String* args[]){
		Program* p = new Program();
		p->Run();
	}
	void Program::Run()
	{
		StreamWriter* sw = new StreamWriter("test.txt");
		sw->WriteLine("Hi! I'm a stream writer");
		sw->Flush();
		sw->Close();
		StreamReader* sr = new StreamReader("test.txt");
		String* text = sr->ReadToEnd();
		sr->Close();
		Console::WriteLine(text);
	}

}