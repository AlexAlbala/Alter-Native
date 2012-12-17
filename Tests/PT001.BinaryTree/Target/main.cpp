#include "BinaryTrees.h"
using namespace System;
using namespace BinaryTree;

int main(int argc, char *argv[])
{
	String *args = new String[argc];
	for(int i = 0; i < argc; i++)
		args[i] = argv[i];

	BinaryTrees::Main(&args);
}