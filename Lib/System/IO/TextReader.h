#pragma once
#include "../IDisposable.h"
#include "../String.h"
#include "../support.h"
#include "../Array.h"

namespace System {
	namespace IO{
		class TextReader : public IDisposable, public virtual gc_cleanup {
		public:	
			void Dispose();
			virtual void Dispose(bool disposing);
			virtual int Peek();
			virtual int Read();
			virtual int Read(Array<char>* buffer, int index, int count);
			virtual String* ReadToEnd();
			virtual int ReadBlock(Array<char>* buffer, int index, int count);
			virtual String* ReadLine();
			virtual void Close();
		};
	}
}