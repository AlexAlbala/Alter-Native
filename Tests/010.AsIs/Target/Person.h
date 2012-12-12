#pragma once
#include "System/System.h"

using namespace System;
namespace AsIs {
	class Person : public virtual Object
	{
		protected:
			String* name;
		public:
			String* getName();
		public:
			Person();
	};
}
