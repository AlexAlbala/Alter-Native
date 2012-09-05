#include "UTF8Encoding.h"

namespace System{
	namespace Text{

		char* UTF8Encoding::GetBytes(String* value)
		{
			return value->Data;
		}

		String* UTF8Encoding::GetString(char* bytes)
		{
			return new String(bytes);
		}
	}
}