#include "UTF8Encoding.h"

namespace System{
	namespace Text{

		Array<char>* UTF8Encoding::GetBytes(String* value)
		{
			Array<char>* v = new Array<char>(value->Data,value->Length);
			return v;
		}

		String* UTF8Encoding::GetString(char* bytes)
		{
			return new String(bytes);
		}

		String* UTF8Encoding::GetString(Array<char>* bytes)
		{
			return new String(bytes->GetData());
		}
	}
}