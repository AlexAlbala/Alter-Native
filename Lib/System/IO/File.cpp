#include "File.h"
namespace System_IO{
	bool File::Exists(String* path)
	{
		return std::ifstream(*path);
	}

	void File::Delete(String* path)
	{
		remove(*path);
	}

	FileStream* File::Create(String* path)
	{
		//return new FileStream(*path);
		return null;
	}

	FileStream* File::OpenRead(String* path)
	{
		//return new FileStream(*path,FileMode::Read);
		return null;
	}
}