#include "Program.h"

//C# TO C++ CONVERTER TODO TASK: The .NET System namespace is not available from native C++:
//using namespace System;
//C# TO C++ CONVERTER TODO TASK: The .NET System namespace is not available from native C++:
//using namespace System::Collections::Generic;
//C# TO C++ CONVERTER TODO TASK: The .NET System namespace is not available from native C++:
//using namespace System::Linq;
//C# TO C++ CONVERTER TODO TASK: The .NET System namespace is not available from native C++:
//using namespace System::Text;
//C# TO C++ CONVERTER TODO TASK: The .NET System namespace is not available from native C++:
//using namespace System::Threading;

namespace List
{

	void Program::Main(std::string& args[])
	{
		Program *p = new Program();
		p->Run();
		p->printList();
		p->Sort();
		p->printList();
	}

	void Program::Run()
	{
		list = std::vector();

		for (int i = 0; i < 100; i++)
			list.push_back(new Node());
	}

	void Program::Sort()
	{
		list.BubbleSort();
	}

	void Program::printList()
	{
		std::cout << "****************" << std::endl;
		for (int i = 0; i < list.Length(); i++)
		{
			std::cout << "Node ";
			std::cout << i;
			std::cout << ": ";
			std::cout << list.getElementAt(i)->value << std::endl;
		}
	}
}
