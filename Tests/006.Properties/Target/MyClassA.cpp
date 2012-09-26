#include "MyClassA.h"
namespace Properties {
	int MyClassA::getdata(){
		return this->_data;
	}
	void MyClassA::setdata(int value)
	{
		this->_data = value;
	}
	int MyClassA::getEmptyProperty()
	{
		return this->EmptyProperty_var;
	}
	void MyClassA::setEmptyProperty(int value)
	{
		this->EmptyProperty_var = value;
	}
	void MyClassA::CallProperties()
	{
		this->setdata(6);
		int value = this->getdata();
		Console::WriteLine(value);
		this->setEmptyProperty(9);
		int emptyValue = this->getEmptyProperty();
		Console::WriteLine(emptyValue);
		MyClassB* b = new MyClassB();
		b->setdata(5);
		int valueB = b->getdata();
		Console::WriteLine(valueB);
	}
	MyClassA::MyClassA()
	{
		_data = 6;
		EmptyProperty_var = 0;
	}

}
