#pragma once

#include <string>
#include <iostream>

//C# TO C++ CONVERTER TODO TASK: The .NET System namespace is not available from native C++:
//using namespace System;
//C# TO C++ CONVERTER TODO TASK: The .NET System namespace is not available from native C++:
//using namespace System::Collections::Generic;
//C# TO C++ CONVERTER TODO TASK: The .NET System namespace is not available from native C++:
//using namespace System::Linq;
//C# TO C++ CONVERTER TODO TASK: The .NET System namespace is not available from native C++:
//using namespace System::Text;

namespace Boxing
{
	class A
	{
	public:
		void f(object *arg);
	};

	class Program
	{
		static void Main(std::string& args[]);

	private:
		static void Box();

		static void UnBox();

		static void f(object *arg);
	};
}
