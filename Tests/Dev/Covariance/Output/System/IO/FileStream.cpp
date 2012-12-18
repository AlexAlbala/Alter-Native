#include "FileStream.h"

namespace System{
	namespace IO{

		//TODO: ENUMS AND NAMESPACES !!
		FileStream::FileStream(String* path, FileMode mode)
		{
			FileStream(path, mode, mode == System::IO::Append ? System::IO::Write : System::IO::ReadWrite);			
		}

		FileStream::FileStream(String* path, FileMode mode, FileAccess access)
		{
			Length = 0;
			Position = 0;

			if(access == System::IO::Read)
			{
				CanRead = true;
				CanWrite = false;

				sr = new StreamReader(path);
			}
			else if(access == System::IO::Write)
			{
				CanRead = false;
				CanWrite = true;

				sw = new StreamWriter(path);
			}
			else
			{
				CanRead = true;
				CanWrite = true;

				sr = new StreamReader(path);
				sw = new StreamWriter(path);
			}
		}

		FileStream::~FileStream()
		{
			if(sw != null)
				sw->Close();
			if(sr != null)
				sr->Close();
		}

		int FileStream::Read(Array<char>* _array, int offset, int count)
		{//TODO Improve method!
			int c = sr->ReadBlock(_array,offset,count);			
			Position += c;
			return c;
	}	

		void FileStream::Write(Array<char>* _array, int offset, int count)
		{
			sw->Write(_array,offset,count);
			Length += count;
		}

		void FileStream::Dispose(bool disposing)
		{
			sw->Close();
			sr->Close();

			delete sw;
			delete sr;

			free(buffer);
			Length = 0;

			Stream::Dispose(true);
		}
	}
}