#include "StreamReaderCXX.h"

namespace System_IO{
StreamReader::StreamReader(String* path)
{
	file = new ifstream();
	file->open(path->Data);
}

StreamReader::StreamReader(const char* path)
{
	file = new ifstream();
	file->open(path);
}


StreamReader::~StreamReader(void)
{
	if (file->is_open())
	{
		file->close();
	}
	delete file;
}

//REVIEW THIS METHOD !
String* StreamReader::ReadLine()
{
	string line;
	if(file->is_open())
	{
		if(file->good())
		{
			std::getline(*file,line);
		}
	}
	return new String(line.data());
}

String* StreamReader::ReadToEnd()
{
	if(file->is_open())
	{
		char* buffer;	

		//Get file length
		file->seekg(0,file->end);
		int length = file->tellg();
		file->seekg(0,ios::beg);

		buffer = new char[length];
		file->read(buffer,length);	

		return new String(buffer);
	}
	else
		throw exception();//TODO MESSAGE
}

void StreamReader::Close()
{
	file->close();
}

}