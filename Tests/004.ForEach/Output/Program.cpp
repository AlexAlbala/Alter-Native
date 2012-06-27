#include "Program.h"
namespace ForEach{

	void Program::Main(String args[]){
		auto &&__range = new List<float>{
			5.6f,
			5.7f,
			5.2f,
			5.9f,
			3.6f,
			52.6f,
			523.6f
		};
		auto __begin = __range->begin();
		auto __end = __range->end();
		for (; __begin != __end; ++__begin) {
			float f = *__begin;
			Console::WriteLine(f);
		}
	}

}