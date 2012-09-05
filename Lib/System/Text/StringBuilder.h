#pragma once
#include "../GC.h"
#include "../String.h"

namespace System{
	namespace Text{

		class StringBuilder : public virtual gc_cleanup /*,ISerializable*/{

		private:
			String* data;
			int DefaultCapacity;
	
		public:
			int Length;
			StringBuilder();
			StringBuilder(int capacity);

			String* ToString();
			StringBuilder* Append(char* value , int startIndex, int charCount);
			StringBuilder* Append(char value);
		};
	}
}