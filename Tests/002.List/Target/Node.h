#pragma once
#include "System/System.h"
#include "Node.h"
#include "Utils.h"

namespace List{
	class Node : public virtual Object, public virtual gc_cleanup
	{
		public:
			Node* next;
		public:
			int value;
		public:
			Node();
	};
}
