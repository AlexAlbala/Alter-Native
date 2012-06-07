#include "test.h"
#include "gtest\gtest.h"

int main(int argc, char* argv[])
{
	//COMMANDS REDIRECT TO THE CORRECT TEST
	::testing::InitGoogleTest(&argc, argv);
	testCase = argv[0];
	return 0;
}