#pragma once
#include "System/System.h"
#include "System/IO/File.h"
#include "System/IO/FileStream.h"
#include "System/Convert.h"
#include "System/Text/UTF8Encoding.h"
#include "System/Console.h"

using namespace System::IO;
using namespace System;
using namespace System::Text;
class Test : public virtual Object
{
	public:
		static void Main();
	private:
	static void AddText(FileStream* fs, String* value);
};
