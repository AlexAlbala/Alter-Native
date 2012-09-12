#pragma once
#include "System/System.h"
#include "Node.h"

using namespace System;
namespace List{
	class MyList : public virtual Object, public virtual gc_cleanup
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
