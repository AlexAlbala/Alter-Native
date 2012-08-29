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

void StreamWriter::Write(String* text)
{
	*file << text->Data;
}

void StreamWriter::Write(const char* text)
{
	*file << text;
}

void StreamWriter::Write(bool value)
{
	*file << value ? "true": "false";
}

void StreamWriter::Write(int value)
{
	*file << value;
}

void StreamWriter::Write(float value)
{
	*file << value;
}

void StreamWriter::Write(unsigned int value)
{
	*file << value;
}

void StreamWriter::Write(long value)
{
	*file << value;
}

void StreamWriter::Write(unsigned long value)
{
	*file << value;
}

void StreamWriter::Write(char buffer[], int index, int count)
{
	//TODO CHECK THE LENGTH OF BUFFER !!!
	char* tmp = new char[count];
	for(int i=index; i < index+count; i++)
	{
		tmp[i-index] = buffer[i];
	}
	*file << tmp;
	delete tmp;
}

void StreamWriter::WriteLine(String* text)
{
	*file << text->Data << std::endl;
}

void StreamWriter::WriteLine(const char* text)
{
	*file << text << std::endl;
}

void StreamWriter::WriteLine(bool value)
{
	*file << (value ? "true": "false") << std::endl;
}

void StreamWriter::WriteLine(int value)
{
	*file << value << std::endl;
}

void StreamWriter::WriteLine(float value)
{
	*file << value << std::endl;
}

void StreamWriter::WriteLine(unsigned int value)
{
	*file << value << std::endl;
}

void StreamWriter::WriteLine(long value)
{
	*file << value << std::endl;
}

void StreamWriter::WriteLine(unsigned long value)
{
	*file << value << std::endl;
}

void StreamWriter::WriteLine(char buffer[], int index, int count)
{
	//TODO CHECK THE LENGTH OF BUFFER !!!
	char* tmp = new char[count];
	for(int i=index; i < index+count; i++)
	{
		tmp[i-index] = buffer[i];
	}
	*file << tmp << std::endl;
	delete tmp;
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