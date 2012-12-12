#pragma once
#include "System/System.h"
#include "Person.h"

using namespace System;
namespace AsIs {
	class John : public virtual Person, public virtual Object
	{
		public:
			John();
	};
}
