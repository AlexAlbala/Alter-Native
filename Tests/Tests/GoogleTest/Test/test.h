#pragma once

int files;
char* pathfiles[];
char* originPathfiles[];
char* testCase;

//Runs AlterNative for the executable For.exe
int RunAlternative(char* target);
//Compare if the output files are the same as the reference
bool CompareFiles();
//Compare if a file equals other
bool CompareFile(char* filepath1, char* filepath2);