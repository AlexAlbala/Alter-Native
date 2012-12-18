#include "StreamReaderCXX.h"

namespace System{
	namespace IO{
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
				file->seekg(0,ios::end);
				int length = file->tellg();
				file->seekg(0,ios::beg);

				buffer = new char[length];
				file->read(buffer,length);	

				return new String(buffer);
			}
			else
				throw exception();//TODO MESSAGE
		}

		//TODO TEST
		int StreamReader::Peek()
		{
			return file->peek();
		}

		//TODO TEST
		int StreamReader::Read()
		{
			if(file->eof())
				return -1;

			char* tmp = new char[1];
			file->read(tmp,1);

			return int(*tmp);
		}

		//TODO TEST
		int StreamReader::Read(Array<char>* buffer, int index, int count)
		{			
			if(file->eof())
				return 0;

			int actualPos = file->tellg();
			//Get file length
			file->seekg(0,ios::end);
			int length = file->tellg();
#ifdef _WIN32
			file->seekg(0, actualPos);
#elif LINUX
			file->seekg(0,(ios::seekdir)actualPos);
#endif

			if(length - index < count)
				count = length - index;

			file->read(buffer->GetData() + index,count);

			return count;
		}

		void StreamReader::Close()
		{
			file->close();
		}

		void StreamReader::Dispose(bool disposing)
		{
			delete file;
		}

	}
}