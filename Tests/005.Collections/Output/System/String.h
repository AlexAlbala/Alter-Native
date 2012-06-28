#pragma once

#include "support.h"
#include <string.h>

class String : public Object, public virtual gc_cleanup {
public:
	int Length;
	char* Data;
	String(void);
	String(const char* txt);
	String* operator =(char* text);
	virtual ~String(void);
	static String* Concat(Object* elements);	
};
