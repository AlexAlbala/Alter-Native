#pragma once
#include "System/System.h"

using namespace System;
namespace Properties {
	class MyClassB : public virtual Object
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
