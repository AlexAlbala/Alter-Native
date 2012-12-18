#include "TextWriter.h"
#include "../support.h"
#include <exception>
#include <sstream>
#include "../String.h"

namespace System{
	namespace IO{

		void TextWriter::Close()
		{
			this->Dispose(true);
		}

		void TextWriter::Dispose()
		{
			this->Dispose(true);
			//GC::SupressFinalize(this);
		}

		void TextWriter::Dispose(bool disposing)
		{
		}

		void TextWriter::Flush()
		{
		}

		void TextWriter::Write(char value)
		{
		}

		void TextWriter::Write(Array<char>* buffer)
		{
			if(buffer != null)
			{
				this->Write(buffer,0, buffer->Length);
			}
		}

		void TextWriter::Write(Array<char>* buffer, int index, int count)
		{
			if (buffer == null)
				{
					throw std::exception();//TODO: NULL exception
				}
				if (index < 0)
				{
					throw std::exception();//TODO: index out of range exception
				}
				if (count < 0)
				{
					throw std::exception();//TODO: index out of range exception
				}
				if (buffer->Length - index < count)
				{
					throw std::exception();//TODO: Argument exception
					//throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
				}
				for (int i = 0; i < count; i++)
				{
					this->Write(buffer[index + i]);
				}
		}

		void TextWriter::Write(bool value)
		{
			this->Write(value ? "True" : "False");
		}

		void TextWriter::Write(int value)
		{
			std::stringstream ss;//create a stringstream
			ss << value;//add number to the stream
			std::string s = ss.str();//return a string with the contents of the stream

			String* text = new String(s.data());
			this->Write(text);
		}

		void TextWriter::Write(unsigned int value)
		{
			std::stringstream ss;//create a stringstream
			ss << value;//add number to the stream
			std::string s = ss.str();//return a string with the contents of the stream

			String* text = new String(s.data());
			this->Write(text);
		}

		void TextWriter::Write(long value)
		{
			std::stringstream ss;//create a stringstream
			ss << value;//add number to the stream
			std::string s = ss.str();//return a string with the contents of the stream

			String* text = new String(s.data());
			this->Write(text);
		}

		void TextWriter::Write(unsigned long value)
		{
			std::stringstream ss;//create a stringstream
			ss << value;//add number to the stream
			std::string s = ss.str();//return a string with the contents of the stream

			String* text = new String(s.data());
			this->Write(text);
		}

		void TextWriter::Write(float value)
		{
			std::stringstream ss;//create a stringstream
			ss << value;//add number to the stream
			std::string s = ss.str();//return a string with the contents of the stream

			String* text = new String(s.data());
			this->Write(text);
		}

		void TextWriter::Write(String* value)
		{		
			Array<char>* a = new Array<char>(value->Data,value->Length);
			this->Write(a);
		}

		void TextWriter::WriteLine(char value)
		{
		}

		void TextWriter::WriteLine(Array<char>* buffer)
		{
			if(buffer != null)
			{
				this->WriteLine(buffer,0, buffer->Length);
			}
		}

		void TextWriter::WriteLine(Array<char>* buffer, int index, int count)
		{
			if (buffer == null)
				{
					throw std::exception();//TODO: NULL exception
				}
				if (index < 0)
				{
					throw std::exception();//TODO: index out of range exception
				}
				if (count < 0)
				{
					throw std::exception();//TODO: index out of range exception
				}
				if (buffer->Length - index < count)
				{
					throw std::exception();//TODO: Argument exception
					//throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
				}
				for (int i = 0; i < count; i++)
				{
					this->Write(buffer[index + i]);
				}
				this->Write("\n");
		}

		void TextWriter::WriteLine(bool value)
		{
			this->WriteLine(value ? "True" : "False");
		}

		void TextWriter::WriteLine(int value)
		{
			std::stringstream ss;//create a stringstream
			ss << value;//add number to the stream
			std::string s = ss.str();//return a string with the contents of the stream

			String* text = new String(s.data());
			this->WriteLine(text);
		}

		void TextWriter::WriteLine(unsigned int value)
		{
			std::stringstream ss;//create a stringstream
			ss << value;//add number to the stream
			std::string s = ss.str();//return a string with the contents of the stream

			String* text = new String(s.data());
			this->WriteLine(text);
		}

		void TextWriter::WriteLine(long value)
		{
			std::stringstream ss;//create a stringstream
			ss << value;//add number to the stream
			std::string s = ss.str();//return a string with the contents of the stream

			String* text = new String(s.data());
			this->WriteLine(text);
		}

		void TextWriter::WriteLine(unsigned long value)
		{
			std::stringstream ss;//create a stringstream
			ss << value;//add number to the stream
			std::string s = ss.str();//return a string with the contents of the stream

			String* text = new String(s.data());
			this->WriteLine(text);
		}

		void TextWriter::WriteLine(float value)
		{
			std::stringstream ss;//create a stringstream
			ss << value;//add number to the stream
			std::string s = ss.str();//return a string with the contents of the stream

			String* text = new String(s.data());
			this->WriteLine(text);
		}

		void TextWriter::WriteLine(String* value)
		{
			Array<char>* a = new Array<char>(value->Data,value->Length);
			this->WriteLine(a);
		}
	}
}