#include "Exception.h"

namespace System{
	Exception::Exception(){
		std::exception();
		Message = new String("");
	}
	Exception::Exception(String* message){
		std::exception();
		this->Message = message;
	}
}