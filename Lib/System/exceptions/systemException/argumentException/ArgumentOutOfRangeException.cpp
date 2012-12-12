#include "ArgumentOutOfRangeException.h"

namespace System{
	ArgumentOutOfRangeException::ArgumentOutOfRangeException() : ArgumentException(){
	}
	ArgumentOutOfRangeException::ArgumentOutOfRangeException(String* message) : ArgumentException(message){
	}
}