#pragma once

#include "../Exception.h"
namespace System{

	class String;
	class SystemException : public Exception{
	public:
		SystemException();
		SystemException(String* message);
	};
}