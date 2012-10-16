#include "Person.h"
namespace AsIs {
	String* Person::getName(){
		return this->name;
	}
	Person::Person()
	{
		name = new String("Unassigned");
	}

}
