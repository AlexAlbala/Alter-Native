#pragma once
#include "../SystemException.h"

namespace System{
	class String;
	
	class ArgumentException : public SystemException{
	public:
		ArgumentException();
		ArgumentException(String* message);
	};
}

#include "argumentException/ArgumentNullException.h"
#include "argumentException/ArgumentOutOfRangeException.h"