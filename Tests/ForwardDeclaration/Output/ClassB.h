#pragma once
#include "System/System.h"
#include "ClassA.h"

namespace ForwardDeclaration{

	//Forward Declaration
	class ClassA;

	class ClassB : public Object{
		private:
			gc_ptr<ClassA> a;
		public:
			ClassB(gc_ptr<ClassA> a);
		public:
			String SayHello();
	};
}