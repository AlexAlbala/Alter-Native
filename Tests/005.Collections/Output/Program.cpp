#include "Program.h"
namespace CollectionsExample{

	void Program::Main(String args[]){
		List<int>* l = new List<int>();
		l->Add(12);
		l->Add(25);
		l->Add(6);
		MyList<int>* myList = new MyList<int>(l);
		auto &&__range = myList;
		auto __begin = __range->begin();
		auto __end = __range->end();
		for (; __begin != __end; ++__begin) {
			int i = *__begin;
			Console::WriteLine(i);
		}
	}

}