#include "List.h"
namespace List{

	List::List(){
		this->first = null;
		this->length = 0;
	}
	int List::Length()
	{
		return this->length;
	}
	void List::Add(gc_ptr<Node> n)
	{
		n->next = this->first;
		this->first = n;
		this->length += 1;
	}
	gc_ptr<Node> List::getElementAt(int index)
	{
		gc_ptr<Node> result;
		if (index >= this->length) {
			result = null;
		}
		else {
			gc_ptr<Node> i = this->first;
			for (int j = 0; j < index; j += 1) {
				i = i->next;
			}
			result = i;
		}
		return result;
	}
	void List::BubbleSort()
	{
		bool sorted = false;
		while (!sorted) {
			sorted = true;
			for (int i = 0; i < this->length - 1; i += 1) {
				gc_ptr<Node> n = this->getElementAt(i);
				gc_ptr<Node> n2 = this->getElementAt(i + 1);
				if (n->value > n2->value) {
					this->Swap(i, i + 1);
					sorted = false;
				}
			}
		}
	}
	void List::Swap(int pos1, int pos2)
	{
		gc_ptr<Node> n = this->getElementAt(pos1);
		gc_ptr<Node> n2 = this->getElementAt(pos2);
		n->next = n2->next;
		n2->next = n;
		if (pos1 > 0) {
			gc_ptr<Node> nant = this->getElementAt(pos1 - 1);
			nant->next = n2;
		}
		if (pos2 == 1 && pos1 == 0) {
			this->first = n2;
		}
	}

}