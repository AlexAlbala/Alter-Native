#include "Program.h"
namespace Properties {
	void Program::Main(String* args[]){
		MyClassA* a = new MyClassA();
		a->CallProperties();
	}

}
