#include "MyClassB.h"
namespace Properties {
	int MyClassB::getdata(){
		return this->_data;
	}
	void MyClassB::setdata(int value)
	{
		this->_data = value;
	}
	MyClassB::MyClassB()
	{
		_data = 3;
	}

}
