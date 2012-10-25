#include "List.h"

//C# TO C++ CONVERTER TODO TASK: The .NET System namespace is not available from native C++:
//using namespace System;
//C# TO C++ CONVERTER TODO TASK: The .NET System namespace is not available from native C++:
//using namespace System::Collections::Generic;
//C# TO C++ CONVERTER TODO TASK: The .NET System namespace is not available from native C++:
//using namespace System::Linq;
//C# TO C++ CONVERTER TODO TASK: The .NET System namespace is not available from native C++:
//using namespace System::Text;

namespace List
{

	List::List()
	{
//C# TO C++ CONVERTER WARNING: C# to C++ Converter converted the original 'null' assignment to a call to 'delete', but you should review memory allocation of all pointer variables in the converted code:
		delete first;
		length = 0;
	}

	int List::Length()
	{
		return length;
	}

	void List::Add(Node *n)
	{
		n->next = first;
		first = n;
		length++;
	}

	Node *List::getElementAt(int index)
	{
		if (index >= length)
			return 0;

			Node *n = first;
			for (int i = 0; i < index; i++)
				n = n->next;

			return n;
	}

	void List::BubbleSort()
	{
		bool sorted = false;

		while (!sorted)
		{
			sorted = true;
			for (int i = 0; i < length - 1; i++)
			{
				Node *n1 = getElementAt(i);
				Node *n2 = getElementAt(i + 1);

				if (n1->value > n2->value)
				{
					Swap(i, i + 1);
					sorted = false;
				}
			}
		}
	}

	void List::Swap(int pos1, int pos2)
	{
		Node *n1 = getElementAt(pos1);
		Node *n2 = getElementAt(pos2);

		n1->next = n2->next;
		n2->next = n1;

	   if (pos1 > 0)
	   {
		   Node *nant = getElementAt(pos1 - 1);
		   nant->next = n2;
	   }

		if (pos2 == 1 && pos1 == 0)
			first = n2;
	}
}
