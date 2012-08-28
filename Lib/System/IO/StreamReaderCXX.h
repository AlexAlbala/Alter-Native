#pragma once
#include <iostream>
#include <fstream>
#include <string>
#include "../String.h";
using namespace std;

namespace System_IO{
class StreamReader
{
private:
	ifstream* file;
public:	
	StreamReader(String* path);
	StreamReader(const char* path);
	~StreamReader(void);
	String* ReadLine();
	String* ReadToEnd();
	void Close();
};
}

