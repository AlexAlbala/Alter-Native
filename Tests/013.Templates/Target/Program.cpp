#include "Program.h"
namespace Templates{
	void Program::Main(String* args[]){
		MyClass_T<A>* myA = new MyClass_T<A>();
		myA->set(new A());
		MyClass_T<B>* myB = new MyClass_T<B>();
		myB->set(new B());
		MyClass_T<String>* myString = new MyClass_T<String>();
		myString->set(new String("Hello"));
		Console::WriteLine(myA->get()->getText());
		Console::WriteLine(myB->get()->getText());
		Console::WriteLine(myString->get());
	}

}
