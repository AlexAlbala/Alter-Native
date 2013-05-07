#include "Program.h"
namespace ExceptionExample {
	void Program::g(int i){
		try{
			i = Program::f(i);
			Console::Write(new String("Result: "));
			Console::WriteLine(i);
		}
		catch (Exception* e) {
			Console::Write(new String("CATCH 2 => "));
			Console::WriteLine(e->Message);
		}
	}
	int Program::f(int i)
	{
		int j = 0;
		Console::Write(new String("f("));
		Console::Write(i);
		Console::WriteLine(new String(")"));
		int result;
		if (i == 0) {
			result = j;
		}
		else {
			if (i == 1) {
				throw new Exception(new String("one"));
			}

			//NEW SCOPE CREATED FOR FINALLY BLOCK!
			{
				//Change finally block for BOOST_SCOPE_EXIT
				finally(&i, &j){
					Console::WriteLine(new String("FINALLY"));
					if (i == 2){
						j = 22;
						Console::Write(new String("RETURN SHOULD BE "));
						Console::WriteLine(j);
					}
					if (i == 5) {
						throw new Exception(new String("five"));
					}
				}
				finally_end
				try {
					if (i == 2) {
						j = 2;
						result = j;
						return result;
					}
					if (i == 3 || i == 4) {
						j = 3;
						throw new Exception();
					}
				}
				catch (Exception* e_7D) {
					Console::WriteLine(new String("CATCH"));
					if (i == 4) {
						j = 4;
						throw new Exception(new String("four"));
					}
				}
				//END OF TRY/CATCH/FINALLY SCOPE
			}
			if (i == 6) {
				throw new Exception(new String("six"));
			}
			Console::WriteLine(new String("RETURN"));
			result = j;
		}
		return result;
	}
	void Program::Main(String* args[])
	{
		Program::g(0);
		Program::g(1);
		Program::g(2);
		Program::g(3);
		Program::g(4);
		Program::g(5);
		Program::g(6);
		Program::g(100);
	}

}
