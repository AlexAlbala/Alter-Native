#include "Program.h"
	namespace CollectionsExample{

		void Program::Main(String args[]){
			MyList<int>* myList = new MyList<int>(new List<int>{
				12,
				25,
				6
			});
			auto &&__range = myList;
			auto __begin = __range->begin();
			auto __end = __range->end();
			for (; __begin != __end; ++__begin) {
				int i = *__begin;
				Console::WriteLine(i);
			}
		}

	}