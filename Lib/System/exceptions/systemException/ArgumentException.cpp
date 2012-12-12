#include "ArgumentException.h"

namespace System{
	ArgumentException::ArgumentException() : SystemException(){
	}
	ArgumentException::ArgumentException(String* message) : SystemException(message){
	}
}