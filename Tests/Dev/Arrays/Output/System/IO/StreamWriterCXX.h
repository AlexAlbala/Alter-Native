#pragma once
#include <iostream>
#include <fstream>
#include "../String.h"
#include "TextWriter.h"
#include "../Array.h"
using namespace std;

namespace System{
	namespace IO{
		class StreamWriter : public TextWriter, public virtual gc_cleanup
		{
		private:
			ofstream* file;
		public:	
			StreamWriter(String* path);
			StreamWriter(const char* path);
			~StreamWriter(void);
			
			
			/*void Write(bool value);
			void Write(int value);
			void Write(float value);
			void Write(unsigned int value);
			void Write(long value);
			void Write(unsigned long value);*/
		
			virtual void Write(char value);
			virtual void Write(const char* value);
			virtual void Write(String* value);
			virtual void Write(Array<char>* text, int length);
			virtual void Write(Array<char>* buffer, int index, int count);
		
			void WriteLine(const char* text);
			void WriteLine(String* text);
			/*void WriteLine(const char* text);
			void WriteLine(bool value);
			void WriteLine(int value);
			void WriteLine(float value);
			void WriteLine(unsigned int value);
			void WriteLine(long value);
			void WriteLine(unsigned long value);
			void WriteLine(char buffer[], int index, int count);*/
			
			virtual void Flush();
			virtual void Close();
			virtual void Dispose(bool disposing);
		};
	}
}

