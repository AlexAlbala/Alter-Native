#pragma once
#include "../ArgumentException.h"

namespace System{
	class String;
	class ArgumentOutOfRangeException : public ArgumentException{
	public:
		ArgumentOutOfRangeException();
		ArgumentOutOfRangeException(String* message);
	};
}