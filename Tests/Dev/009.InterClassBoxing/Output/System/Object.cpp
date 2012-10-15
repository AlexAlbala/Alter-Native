#include "Object.h"
#include "String.h"

namespace System{
	Object::Object(void){
	}

	Object::~Object(void){
	}

	String* Object::ToString(void){
		return new String("System::Object");
	}
}