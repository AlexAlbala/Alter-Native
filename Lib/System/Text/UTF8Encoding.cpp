#include "UTF8Encoding.h"
#include <boost/locale.hpp>

namespace loc = boost::locale;
namespace System{
	namespace Text{

		Array<char>* UTF8Encoding::GetBytes(String* value)
		{
			loc::generator* gen = new loc::generator();
			std::locale blah = gen->generate("es_ES.utf-8");
			std::string* UTF8String = new string(value->Data);

			std::string converted = loc::conv::from_utf(*UTF8String, blah);

			delete gen;
			delete UTF8String;

			Array<char>* v = new Array<char>((char*)converted.data(),converted.size());
			return v;
		}

		String* UTF8Encoding::GetString(char* bytes)
		{
			return GetString(new Array<char>(bytes, strlen(bytes)));
		}

		String* UTF8Encoding::GetString(Array<char>* bytes)
		{	
			loc::generator* gen = new loc::generator();
			std::locale blah = gen->generate("es_ES.utf-8");
			std::string* UTF8String = new string(bytes->GetData());

			std::string converted = loc::conv::from_utf(*UTF8String, blah);

			delete gen;
			delete UTF8String;

			return new String(converted.data());
		}
	}
}