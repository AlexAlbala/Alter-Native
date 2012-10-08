#include "Program.h"
namespace CustomCollections {
	void Program::Main(String* args[]){
		Array<int>* i = new Array<int>(3);		
		(*i)[0] = 2;
		(*i)[1] = 3;
		(*i)[2] = 4;
		MyList_T<int>* myList = new MyList_T<int>(i);
		FOREACH(_J, myList){
			int j = *_J;
			Console::WriteLine(j);
		}
		Array<A>* al = new Array<A>(3);		
		//(*al)[0] = A();
		//(*al)[1] = A();
		//(*al)[2] = A();
		//MyList_T<A>* myListA = new MyList_T<A>(al);
		//FOREACH(_NA, myListA) {
		//	//A* na = *_NA;
		//	//Console::WriteLine(na->value);
		//}
	}

}
