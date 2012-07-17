#pragma once
#include "System/System.h"

namespace Properties{

	class MyClassB : public Object, public gc_cleanup
	{
		private:
			int _data;
		public:
			int getdata();
		public:
			void setdata(int value);
		public:
			MyClassB();
	};
}