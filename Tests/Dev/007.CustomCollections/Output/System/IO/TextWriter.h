#pragma once
#include "../IDisposable.h"
#include "../String.h"
#include "../Array.h"

namespace System {
	namespace IO{
		class TextWriter : public IDisposable
		{
		public:
			virtual void Close();
			virtual void Dispose();
			virtual void Dispose(bool disposing);
			virtual void Flush();
		
			virtual void Write(char value);
			//virtual void Write(const char* value);
			virtual void Write(Array<char>* buffer);
			virtual void Write(Array<char>* buffer, int index, int count);
			virtual void Write(bool value);
			virtual void Write(int value);
			virtual void Write(unsigned int value);
			virtual void Write(long value);
			virtual void Write(unsigned long value);
			virtual void Write(float value);
			virtual void Write(String* value);
		
			virtual void WriteLine(char value);
			//virtual void WriteLine(const char* value);
			virtual void WriteLine(Array<char>* buffer);
			virtual void WriteLine(Array<char>* buffer, int index, int count);
			virtual void WriteLine(bool value);
			virtual void WriteLine(int value);
			virtual void WriteLine(unsigned int value);
			virtual void WriteLine(long value);
			virtual void WriteLine(unsigned long value);
			virtual void WriteLine(float value);
			virtual void WriteLine(String* value);
		};
	}
}