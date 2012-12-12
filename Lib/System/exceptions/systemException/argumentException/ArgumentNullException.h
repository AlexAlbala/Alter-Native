#pragma once
#include "../ArgumentException.h"

namespace System{
	class String;
	class ArgumentNullException : public ArgumentException{
	public:
		ArgumentNullException();
		ArgumentNullException(String* message);
	};
}