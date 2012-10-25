#pragma once

#include "Node.h"

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
	class List
	{
	private:
		Node *first;
		int length;

	public:
		List();

		int Length();

		void Add(Node *n);

		Node *getElementAt(int index);

		void BubbleSort();

	private:
		void Swap(int pos1, int pos2);


	};
}
