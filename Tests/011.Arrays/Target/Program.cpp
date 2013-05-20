#include "Program.h"
namespace Arrays {
	void Program::Main(String* args[]){
		Program* p = new Program();
		Array<int>* arr = new Array<int>(15);
		for (int i = 0; i < arr->Length; i += 1){
			arr->SetData(i, i);
		}
		p->Test1(arr);
	}
	void Program::Test1(Array<int>* myArray)
	{
		Array<long>* arr = new Array<long>(myArray->Length);
		int pos = 0;
		for (int j = 0; j < myArray->Length; j += 1) {
			int i = (*myArray)[j];
			arr->SetData(pos++, (long)(i + 50));
		}
		this->Test2(myArray, arr);
	}
	void Program::Test2(Array<int>* myArray1, Array<long>* myArray2)
	{
		String* s = new String("This is a char array message");
		Array<char>* message = s->ToCharArray();
		this->Finish(myArray1, myArray2, message);
	}
	void Program::Finish(Array<int>* myArray1, Array<long>* myArray2, Array<char>* myArray3)
	{
		for (int i = 0; i < myArray1->Length; i += 1) {
			Console::WriteLine((*myArray1)[i]);
			Console::WriteLine((*myArray2)[i]);
		}
		Console::WriteLine(myArray3);
	}

}
