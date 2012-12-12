#include "Test.h"
void Test::Main(){
	String* path = new String("MyTest.txt");
	if (File::Exists(path)){
		File::Delete(path);
	}
	FileStream* fs = File::Create(path);
	Test::AddText(fs, new String("This is some text"));
	Test::AddText(fs, new String("This is some more text,"));
	Test::AddText(fs, new String("\r\nand this is on a new line"));
	Test::AddText(fs, new String("\r\n\r\nThe following is a subset of characters:\r\n"));
	for (int i = 100; i < 220; i += 1) {
		Test::AddText(fs, new String(Convert::ToChar(i)));
		if (i % 10 == 0) {
			Test::AddText(fs, new String("\r\n"));
		}
	}
	fs->Close();
	fs = File::OpenRead(path);
	Array<char>* b = new Array<char>(1024);
	UTF8Encoding* temp = new UTF8Encoding();
	while (fs->Read(b, 0, b->Length) > 0) {
		String* s = temp->GetString(b);
		Console::WriteLine(s);
	}
}
void Test::AddText(FileStream* fs, String* value)
{
	Array<char>* info = (new UTF8Encoding())->GetBytes(value);
	fs->Write(info, 0, info->Length);
}

