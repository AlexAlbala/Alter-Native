#include "NotImplementedException.h"

namespace System{
	NotImplementedException::NotImplementedException() : SystemException(){
	}
	NotImplementedException::NotImplementedException(String* message) : SystemException(message){
	}
}