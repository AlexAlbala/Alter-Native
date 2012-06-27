#include "String.h"

String::String() {
	Data = null;
	Length = 0;
}

String::String(const char* txt) {
	//TODO It will be nice to not to copy "constant" strings 
	Length = strlen(txt);
	Data = new char[Length+1];
	strncpy(Data,txt,Length);
}

String::~String() {
	if(Data!=null) {
		delete Data;
		Data = null;
	}
}

gc_ptr<String> String::Concat(Object* elements) {
	gc_ptr<String> newstring (new(gc)String("String::Concat NotImplemented"));
	delete elements;
	return newstring;
}