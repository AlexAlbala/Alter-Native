#pragma once
#include "System/System.h"
#include "Utils.h"

using namespace System;
namespace List {
	class Node : public virtual Object
	{
		public:
			Node* next;
		public:
			int value;
		public:
			Node();
	};
}
