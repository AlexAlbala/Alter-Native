#include "Stream.h"

namespace System{
	namespace IO{
		void Stream::Dispose(){
			this->Close();
		}

		void Stream::Dispose(bool disposing){
		}

		void Stream::Close(){
			this->Dispose(true);
		}
	}
}