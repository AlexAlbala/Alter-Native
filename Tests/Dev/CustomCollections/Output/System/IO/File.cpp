#include "File.h"
#include "FileMode.h"
#include "FileAccess.h"
namespace System{
	namespace IO{
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
			return new FileStream(path, System::IO::CreateNew,System::IO::ReadWrite);//TODO mmm ENUMS AND NAMESPACES?
		}

		FileStream* File::OpenRead(String* path)
		{
			if(File::Exists(path))
			{
				return new FileStream(path,System::IO::Open, System::IO::Read);//TODO mmm ENUMS AND NAMESPACES?
			}
			else
				throw std::exception();//TODO: FILENOTFOUNDEXCEPTION
		}
	}
}