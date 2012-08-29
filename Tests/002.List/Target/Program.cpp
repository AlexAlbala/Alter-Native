#include "Program.h"
namespace List{

	void Program::Main(String* args[]){
		Program* p = new Program();
		p->Run();
		p->printList();
		p->Sort();
		p->printList();
	}
	void Program::Run()
	{
		this->list = new MyList();
		for (int i = 0; i < 100; i += 1) {
			this->list->Add(new Node());
		}
	}
	void Program::Sort()
	{
		this->list->BubbleSort();
	}
	void Program::printList()
	{
		Console::WriteLine("****************");
		for (int i = 0; i < this->list->Length(); i += 1) {
			Console::Write("Node ");
			Console::Write(i);
			Console::Write(": ");
			Console::WriteLine(this->list->getElementAt(i)->value);
		}
	}

}