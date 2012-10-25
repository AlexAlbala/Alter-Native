#include "Program.h"

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

	void A::f(object *arg)
	{
		std::cout << "I'm A" << std::endl;
		std::cout << arg << std::endl;
	}

	void Program::Main(std::string& args[])
	{
		Box();
		UnBox();
	}

	void Program::Box()
	{
		int i = 123;
		object *o = i; // implicit boxing

		i = 456; // change the contents of i

		f(i);
		A *a = new A();
		a->f(i);

		std::cout << i << std::endl;
		std::cout << o << std::endl;
	}

	void Program::UnBox()
	{
		object *o = 123;
		int i = static_cast<int>(o);

		o = 456; // change the contents of i



		std::cout << i << std::endl;
		std::cout << o << std::endl;
	}

	void Program::f(object *arg)
	{
		std::cout << "I'm P" << std::endl;
		std::cout << arg << std::endl;
	}
}
