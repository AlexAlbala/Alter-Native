#pragma once
#include "../String.h"

namespace System{
	namespace Text{
		class UTF8Encoding // : public Encoding
		{
		public:
			char* GetBytes(String* value);
			String* GetString(char* bytes);
		};
	}
}