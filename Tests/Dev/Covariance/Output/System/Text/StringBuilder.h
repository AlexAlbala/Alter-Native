#pragma once
#include "../GC.h"
#include "../String.h"
#include "../Array.h"

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
			StringBuilder* Append(Array<char>* value , int startIndex, int charCount);
			StringBuilder* Append(char value);
		};
	}
}