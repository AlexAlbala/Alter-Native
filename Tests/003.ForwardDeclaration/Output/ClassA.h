#pragma once
#include "System/System.h"
#include "ClassB.h"

namespace ForwardDeclaration{

	//Forward Declaration
	class ClassB;

	class ClassA : public Object, gc_cleanup{
		private:
			ClassB* b;
		public:
			ClassA();
		public:
			String SayHello();
	};
}