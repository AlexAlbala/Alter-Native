#pragma once
#include "System/System.h"
#include "Node.h"

namespace List{

	class List : public Object
	{
		private:
			gc_ptr<Node> first;
		private:
			int length;
		public:
			List();
		public:
			int Length();
		public:
			void Add(gc_ptr<Node> n);
		public:
			gc_ptr<Node> getElementAt(int index);
		public:
			void BubbleSort();
		private:
			void Swap(int pos1, int pos2);
	};
}