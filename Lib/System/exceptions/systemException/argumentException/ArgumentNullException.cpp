#include "ArgumentNullException.h"

namespace System{
	ArgumentNullException::ArgumentNullException() : ArgumentException(){		
	}
	ArgumentNullException::ArgumentNullException(String* message) : ArgumentException(message){
	}
}