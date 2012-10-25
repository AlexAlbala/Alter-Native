#pragma once

#include "List.h"
#include "Node.h"
#include <string>
#include <vector>
#include <iostream>

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
	class Program
	{
	private:
		std::vector list;

		static void Main(std::string& args[]);

		void Run();

		void Sort();

		void printList();


	};
}
