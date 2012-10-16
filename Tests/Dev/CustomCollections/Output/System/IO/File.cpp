#include "File.h"
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
			return new FileStream(path, FileMode::CreateNew,FileAccess::ReadWrite);
		}

		FileStream* File::OpenRead(String* path)
		{
			if(File::Exists(path))
			{
				return new FileStream(path,FileMode::Open, FileAccess::Read);
			}
			else
				throw std::exception("File not found");
		}
	}
}