#pragma once
#include "System/System.h"
#include "Node.h"
#include "Utils.h"

namespace List{

	class Node : public Object
	{
		public:
			Node* next;
		public:
			int value;
		public:
			Node();
	};
}