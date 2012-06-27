#pragma once
#include "System/System.h"
#include "ClassB.h"

namespace ForwardDeclaration{

	//Forward Declaration
	class ClassB;

	class ClassA : public Object{
		private:
			ClassB* b;
		public:
			ClassA();
		public:
			String SayHello();
	};
}