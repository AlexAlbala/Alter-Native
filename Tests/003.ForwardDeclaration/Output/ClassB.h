#pragma once
#include "System/System.h"
#include "ClassA.h"

namespace ForwardDeclaration{

	//Forward Declaration
	class ClassA;

	class ClassB : public Object, gc_cleanup{
		private:
			ClassA* a;
		public:
			ClassB(ClassA* a);
		public:
			String SayHello();
	};
}