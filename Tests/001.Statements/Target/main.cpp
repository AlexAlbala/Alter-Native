#include "Program.h"
using namespace System;
using namespace Statements;

int main(int argc, char *argv[])
{
	String *args = new String[argc];
	for(int i = 0; i < argc; i++)
		args[i] = argv[i];

	Program::Main(&args);
}