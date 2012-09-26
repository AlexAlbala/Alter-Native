#include "MyList.h"
namespace List {
	MyList::MyList(){
		this->first = null;
		this->length = 0;
	}
	int MyList::Length()
	{
		return this->length;
	}
	void MyList::Add(Node* n)
	{
		n->next = this->first;
		this->first = n;
		this->length += 1;
	}
	Node* MyList::getElementAt(int index)
	{
		Node* result;
		if (index >= this->length) {
			result = null;
		}
		else {
			Node* i = this->first;
			for (int j = 0; j < index; j += 1) {
				i = i->next;
			}
			result = i;
		}
		return result;
	}
	void MyList::BubbleSort()
	{
		bool sorted = false;
		while (!sorted) {
			sorted = true;
			for (int i = 0; i < this->length - 1; i += 1) {
				Node* n = this->getElementAt(i);
				Node* n2 = this->getElementAt(i + 1);
				if (n->value > n2->value) {
					this->Swap(i, i + 1);
					sorted = false;
				}
			}
		}
	}
	void MyList::Swap(int pos1, int pos2)
	{
		Node* n = this->getElementAt(pos1);
		Node* n2 = this->getElementAt(pos2);
		n->next = n2->next;
		n2->next = n;
		if (pos1 > 0) {
			Node* nant = this->getElementAt(pos1 - 1);
			nant->next = n2;
		}
		if (pos2 == 1 && pos1 == 0) {
			this->first = n2;
		}
	}

}
