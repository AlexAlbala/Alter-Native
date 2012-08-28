#pragma once
#include <iostream>
#include <fstream>
#include "../String.h"//TODO CHANGE !
using namespace std;

namespace System_IO{
class StreamWriter
{
private:
	ofstream* file;
public:	
	StreamWriter(String* path);
	StreamWriter(const char* path);
	~StreamWriter(void);
	void WriteLine(String* text);
	void WriteLine(const char* text);
	void Flush();
	void Close();
};
}

