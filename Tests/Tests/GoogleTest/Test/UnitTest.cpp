#include "test.h"
#include "gtest\gtest.h"
#include <cstring>

TEST(AlterNativeTest, Run){
	if(strcmp(testCase,"for")){
		ASSERT_EQ(1,RunAlternative("for/For.exe"));
	}
}

TEST(AlterNativeTest, Files){
	EXPECT_TRUE(CompareFiles());

	for(int i = 0; i < files; i++){
		EXPECT_TRUE(CompareFile(pathfiles[i],originPathfiles[i]));
	}
}