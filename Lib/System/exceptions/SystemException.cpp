#include "SystemException.h"

namespace System{
	SystemException::SystemException() : Exception() {	
	}
	SystemException::SystemException(String* message) : Exception(message){	
	}
}
