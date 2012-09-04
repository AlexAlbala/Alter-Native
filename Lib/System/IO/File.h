#pragma once
#include <stdio.h>
#include "../GC.h"
#include "../String.h"
#include "FileStream.h"
#include <fstream>

namespace System_IO{
class File : public virtual gc_cleanup{
public:
	static bool Exists(String* path);
	static void Delete(String* path);
	static FileStream* OpenRead(String* path);
	static FileStream* Create(String* path);
};
}