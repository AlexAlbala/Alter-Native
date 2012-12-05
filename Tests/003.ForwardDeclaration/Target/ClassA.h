#pragma once
#include "System/System.h"
#include "ClassB.h"

using namespace System;
namespace ForwardDeclaration {
	//Forward Declaration
	class ClassB;

	class ClassA : public virtual Object{
		private:
			ClassB* b;
		public:
			ClassA();
		public:
			String* SayHello();
	};
}
