#pragma once
#include "System/System.h"
#include "Node.h"
#include "Utils.h"

namespace List{

	class Node : public Object, public gc_cleanup
	{
		public:
			Node* next;
		public:
			int value;
		public:
			Node();
	};
}