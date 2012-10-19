#pragma once
#include "../String.h"
#include "../System.h"

using namespace std;

namespace System{
	namespace Text{
		class UTF8Encoding // : public Encoding
		{
		public:
			Array<char>* GetBytes(String* value);
			String* GetString(char* bytes);
			String* GetString(Array<char>* bytes);
		};
	}
}