#pragma once
#include <stdlib.h>
#include <stdio.h>
#include "FileMode.h"
#include "FileAccess.h"
#include "Stream.h"
#include "../String.h"
#include "StreamReaderCXX.h"
#include "StreamWriterCXX.h"
#include "../Array.h"

namespace System{
	namespace IO{
		class FileStream : public Stream
		{
		public:
			bool CanRead;
			bool CanWrite;
			bool CanSeek;
			long Length;
			long Position;

			StreamWriter* sw;
			StreamReader* sr;
		
			FileStream(String* path, FileMode mode);
			FileStream(String* path, FileMode mode, FileAccess access);
			~FileStream();
			int Read(Array<char>* _array, int offset, int count);
			void Write(Array<char>* _array, int offset, int count);
			virtual void Dispose(bool disposing);
		
		private:
			char* buffer;
		};
	}
}