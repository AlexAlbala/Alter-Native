#include "Convert.h"
#include <exception>
using namespace std;
namespace System{
	char Convert::ToChar(int value)
	{
		if(value < 0 || value > 65535)
		{
			//TODO: CREATE EXCEPTIONS !
			throw exception();
		}
		return (char)value;
	}
}