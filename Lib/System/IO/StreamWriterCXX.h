#pragma once
#include <iostream>
#include <fstream>
#include "../String.h"
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
	void Write(String* text);
	void Write(const char* text);
	void Write(bool value);
	void Write(int value);
	void Write(float value);
	void Write(unsigned int value);
	void Write(long value);
	void Write(unsigned long value);
	void Write(char buffer[], int index, int count);

	void WriteLine(String* text);
	void WriteLine(const char* text);
	void WriteLine(bool value);
	void WriteLine(int value);
	void WriteLine(float value);
	void WriteLine(unsigned int value);
	void WriteLine(long value);
	void WriteLine(unsigned long value);
	void WriteLine(char buffer[], int index, int count);
	
	void Flush();
	void Close();
};
}

