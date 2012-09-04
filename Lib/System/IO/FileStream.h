#pragma once
#include <stdlib.h>
#include <stdio.h>
#include "FileMode.h"
#include "FileAccess.h"
#include "Stream.h"
#include "../String.h"

namespace System_IO{
class FileStream : public Stream
{
public:
	bool CanRead;
	bool CanWrite;
	bool CanSeek;
	long Length;
	long Position;

	FileStream(String* path, FileMode mode);
	FileStream(String* path, FileMode mode, FileAccess access);	
	int Read(char* _array, int offset, int count);
	void Write(char* _array, int offset, int count);
	virtual void Dispose();

private:
	char* buffer;
};
}