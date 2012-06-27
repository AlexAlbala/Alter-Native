#include "Program.h"
namespace For{

	void Program::Main(String args[]){
		for (int i = 0; i < 500; i += 1){
			Console::WriteLine(i);
		}
	}

}