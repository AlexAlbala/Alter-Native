#include "UTF8Encoding.h"
#include <boost/locale.hpp>

namespace loc = boost::locale;
namespace System{
	namespace Text{

		Array<char>* UTF8Encoding::GetBytes(String* value)
		{
			loc::generator* gen = new loc::generator();
			std::locale blah = gen->generate("en_US.utf-8");			

			std::string converted = loc::conv::to_utf<char>(value->Data, blah);

			delete gen;

			Array<char>* v = new Array<char>((char*)converted.data(),converted.size());
			return v;
		}

		String* UTF8Encoding::GetString(byte* bytes)
		{
			loc::generator* gen = new loc::generator();
			std::locale blah = gen->generate("en_US.utf-8");			
			std::string converted = loc::conv::from_utf(bytes,blah);

			delete gen;

			return new String(converted.data());			
		}

		String* UTF8Encoding::GetString(Array<byte>* bytes)
		{	
			return GetString(bytes->GetData());
		}
	}
}