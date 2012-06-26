#include "Program.h"
namespace List{

	void Program::Main(String args[]){
		gc_ptr<Program> p (new(gc)Program());
		p->Run();
		p->printList();
		p->Sort();
		p->printList();
	}
	void Program::Run()
	{
		this->list = gc_ptr<List> (new(gc)List());
		for (int i = 0; i < 100; i += 1) {
			this->list->Add(gc_ptr<Node> (new(gc)Node()));
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