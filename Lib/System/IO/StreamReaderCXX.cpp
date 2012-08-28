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

	line.append("\n");//REVIEW THIS
	return new String(line.data());
}

String* StreamReader::ReadToEnd()
{
	string data;
	while(!file->eof())
	{
		data.append(ReadLine()->Data);
	}

	return new String(data.data());
}

void StreamReader::Close()
{
	file->close();
}

}