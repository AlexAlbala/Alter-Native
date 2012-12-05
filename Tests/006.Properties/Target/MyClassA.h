#pragma once
#include "System/System.h"
#include "System/Console.h"
#include "MyClassB.h"

using namespace System;
namespace Properties {
	class MyClassA : public virtual Object
	{
		private:
			int _data;
		public:
			int getdata();
		public:
			void setdata(int value);
		public:
			int getEmptyProperty();
		public:
			void setEmptyProperty(int value);
		public:
			void CallProperties();
		private:
			int EmptyProperty_var;
		public:
			MyClassA();
	};
}
