#pragma once
#include <string.h>
#include "Object.h"
#include "GC.h"
#include "support.h"
#include "Array.h"

namespace System{
	//Forward declaration
	class String : public Object, public virtual gc_cleanup {
	public:
		int Length;
		char* Data;
		String(void);
		String(int txt);
		String(long txt);
		String(char txt);
		String(const char* txt);
		String* operator =(char* text);
		Array<char>* ToCharArray();
		operator const char*();
		virtual ~String(void);
		static String* Concat(Object* elements);	
	};
}