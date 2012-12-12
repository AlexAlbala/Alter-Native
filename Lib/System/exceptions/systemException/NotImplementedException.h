#pragma once
#include "../SystemException.h"

namespace System{
	class String;
	
	class NotImplementedException : public SystemException{
	public:
		NotImplementedException();
		NotImplementedException(String* message);
	};
}