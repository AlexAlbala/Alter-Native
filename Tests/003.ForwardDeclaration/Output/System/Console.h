#pragma once

#include "Object.h"
#include "String.h"

namespace System {
class Console : public Object
{
public:

	static void Write(char* txt);
	static void Write(int i);
	static void Write(String* s);

	static void WriteLine(char* txt);
	static void WriteLine(int i);
	static void WriteLine(String* s);

	static char* ReadLine();
};
}