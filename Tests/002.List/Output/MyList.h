#pragma once
#include "System/System.h"
#include "Node.h"

namespace List{

	class MyList : public Object
	{
		private:
			Node* first;
		private:
			int length;
		public:
			MyList();
		public:
			int Length();
		public:
			void Add(Node* n);
		public:
			Node* getElementAt(int index);
		public:
			void BubbleSort();
		private:
			void Swap(int pos1, int pos2);
	};
}