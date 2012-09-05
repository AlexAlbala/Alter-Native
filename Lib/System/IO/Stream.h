#pragma once
#include "../IDisposable.h"
namespace System{
	namespace IO{
		class Stream : public IDisposable
		{
		public:
			void Dispose();
		};
	}
}