#include "Object.h"
#include "String.h"

namespace System{
	Object::Object(void)
	{
	}

	Object::~Object(void)
	{
	}

	Object::Object(int i)
	{
		_box_i = new Box_T<int>(i);
	}

	/*Object::Object(Object& o)
	{

	}*/

	Object::operator int()
	{
		return *_box_i;
	}

	Object& Object::operator=(int i)
	{
		_box_i = new Box_T<int>(i);
		return *this;
	}

	/*Object* Object::operator=(int i)
	{
		_box_i = new Box_T<int>(i);
		return this;
	}*/

	String* Object::ToString(void)
	{
		return new String("System::Object");
	}
}