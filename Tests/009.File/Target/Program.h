#pragma once
#include "System/System.h"
#include "Program.h"
#include "System/IO/StreamWriterCXX.h"
#include "System/IO/StreamReaderCXX.h"
#include "System/Console.h"

using namespace System_IO;
using namespace System;
namespace File{

	class Program : public virtual Object, public virtual gc_cleanup
	{
		public:
			static void Main(String* args[]);
		private:
			void Run();
	};
}