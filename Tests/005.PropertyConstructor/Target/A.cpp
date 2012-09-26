#include "A.h"
namespace PropertyConstructor {
	int A::getP(){
		return this->p;
	}
	void A::setP(int value)
	{
		this->p = value;
	}
	void A::Main(String* args[])
	{
		A::Main1();
		A::Main2();
	}
	void A::Main1()
	{
		A* a = new A();
		a->setP(3);
		Console::WriteLine(a->getP());
	}
	void A::Main2()
	{
		A* a2 = new A();
		a2->setP(3);
		A* a = a2;
		Console::WriteLine(a->getP());
	}

}
