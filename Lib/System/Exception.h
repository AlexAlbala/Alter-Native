#pragma once
#include "String.h"
#include <exception>

using namespace std;
namespace System{
	class Exception : public Object, public exception{

	public:
		Exception();
		Exception(String* message);	
		String* Message;
	};
}