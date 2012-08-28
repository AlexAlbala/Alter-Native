#include "StreamWriterCXX.h"

namespace System_IO{
StreamWriter::StreamWriter(String* path)
{
	file = new ofstream();
	file->open(path->Data);
}

StreamWriter::StreamWriter(const char* path)
{
	file = new ofstream();
	file->open(path);
}


StreamWriter::~StreamWriter(void)
{
	
}

void StreamWriter::WriteLine(String* text)
{
	*file << text->Data << std::endl;
}

void StreamWriter::WriteLine(const char* text)
{
	*file << text << std::endl;
}

void StreamWriter::Flush()
{
	file->flush();
}

void StreamWriter::Close()
{
	file->close();
}

}