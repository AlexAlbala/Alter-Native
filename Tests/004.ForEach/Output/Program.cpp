#include "Program.h"
namespace ForEach{

	void Program::Main(String args[]){
		List_T<float>* myList = new List_T<float>();
		myList->Add(5.6f);
		myList->Add(5.7f);
		myList->Add(5.2f);
		myList->Add(5.9f);
		myList->Add(3.6f);
		myList->Add(52.6f);
		myList->Add(523.6f);
		auto &&__range = myList;
		auto __begin = __range->begin();
		auto __end = __range->end();
		for (; __begin != __end; ++__begin){
			float f = *__begin;
			Console::WriteLine(f);
		}
	}

}