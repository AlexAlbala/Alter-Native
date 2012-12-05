#pragma once
#include "System/System.h"
#include "ClassA.h"

using namespace System;
namespace ForwardDeclaration {
	//Forward Declaration
	class ClassA;

	class ClassB : public virtual Object{
		private:
			ClassA* a;
		public:
			ClassB(ClassA* a);
		public:
			String* SayHello();
	};
}
