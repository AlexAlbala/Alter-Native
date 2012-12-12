#include "Exception.h"

namespace System{
	Exception::Exception(){
		std::exception();
		message = new String("");
	}
	Exception::Exception(String* message){
		std::exception();
		this->message = message;
	}
}