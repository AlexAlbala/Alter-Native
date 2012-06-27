#pragma once

#include "Object.h"
#include <string.h>
#include "gcptr.h"

class String : public Object
{
public:
	int Length;
	char* Data;
	String(void);
	String(const char* txt);
	~String(void);
	static gc_ptr<String> Concat(Object* elements);	
};
