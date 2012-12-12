#pragma once
#include "System/System.h"
#include "System/IO/StreamWriterCXX.h"
#include "System/IO/StreamReaderCXX.h"
#include "System/Console.h"

using namespace System::IO;
using namespace System;
namespace File {
	class Program : public virtual Object
	{
		public:
			static void Main(String* args[]);
		private:
			void Run();
	};
}
