#include "TextReader.h"
#include <exception>
#include "../Text/StringBuilder.h"

using namespace System_Text;

namespace System_IO {

	void TextReader::Close()
	{
		this->Dispose(true);
	}

	void TextReader::Dispose()
	{
		this->Dispose(true);
		//GC::SupressFinalize(this);
	}

	void TextReader::Dispose(bool disposing)
	{
	}

	int TextReader::Peek()
	{
		return -1;
	}

	int TextReader::Read()
	{
		return -1;
	}

	int TextReader::Read(char* buffer, int index, int count)
	{
		if (buffer == null)
		{
			throw std::exception("buffer is null");
			//throw new ArgumentNullException("buffer", Environment.GetResourceString("ArgumentNull_Buffer"));
		}
		if (index < 0)
		{
			throw std::exception("index out of range");
			//throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
		}
		if (count < 0)
		{
			throw std::exception("count out of range");
			//throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
		}
		//if (buffer.Length - index < count) //CANNOT BE CHECKED
		//{
		//	//throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
		//}
		int num = 0;
		do
		{
			int num2 = this->Read();
			if (num2 == -1)
			{
				break;
			}
			buffer[index + num++] = (char)num2;
		}
		while (num < count);
		return num;
	}

	String* TextReader::ReadToEnd()
	{
		char* _array = new char[4096];
		StringBuilder* stringBuilder = new StringBuilder(4096);//System.Text.StringBuilder
		int charCount;
		while ((charCount = this->Read(_array, 0, 4096)) != 0)
		{
			stringBuilder->Append(_array, 0, charCount);
		}
		return stringBuilder->ToString();
	}

	int TextReader::ReadBlock(char* buffer, int index, int count)
	{
		int num = 0;
		int num2;
		do
		{
			num += (num2 = this->Read(buffer, index + num, count - num));
		}
		while (num2 > 0 && num < count);
		return num;
	}

	String* TextReader::ReadLine()
	{
		StringBuilder* stringBuilder = new StringBuilder();//System.Text.StringBuilder
		int num;
		while (true)
		{
			num = this->Read();
			if (num == -1)
			{
				goto IL_43;
			}
			if (num == 13 || num == 10)
			{
				break;
			}
			stringBuilder->Append((char)num);
		}
		if (num == 13 && this->Peek() == 10)
		{
			this->Read();
			goto IL_31;
		}
		goto IL_31;
		IL_43:
		if (stringBuilder->Length > 0)
		{
			return stringBuilder->ToString();
		}
		return null;
		IL_31:
		return stringBuilder->ToString();
	}

}