#include "FileStream.h"

namespace System_IO{

	FileStream::FileStream(String* path, FileMode mode)
	{
		Length = 0;
		Position = 0;
		buffer = (char*)malloc(0);

		//FileMode Create, CreateNew....
	}

	FileStream::FileStream(String* path, FileMode mode, FileAccess access)
	{
		//FileMode Create, CreateNew... and FileAccess Read/Write
	}

	int FileStream::Read(char* _array, int offset, int count)
	{//TODO Improve method!
		memcpy(_array + offset, buffer + Position, count);
		Position += count;
		return count;
	}

	void FileStream::Write(char* _array, int offset, int count)
	{
		buffer = (char*)realloc(buffer, (count + Length)*sizeof(char));		
		memcpy(buffer + Length, _array + offset, count);

		Length += count;
	}

	void FileStream::Dispose()
	{
		free(buffer);
		Length = 0;
	}
}