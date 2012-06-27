#pragma once
#include "System/System.h"
#include "Node.h"
#include "Utils.h"

namespace List{

	class Node : public Object
	{
		public:
			gc_ptr<Node> next;
		public:
			int value;
		public:
			Node();
	};
}