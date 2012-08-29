#pragma once
#include "System/System.h"
#include "A.h"
#include "System/Console.h"

using namespace System;
namespace PropertyConstructor{

	class A : public virtual Object, public virtual gc_cleanup
	{
		private:
			int p;
		public:
			int getP();
		public:
			void setP(int value);
		public:
			static void Main(String* args[]);
		private:
		static void Main1();
		private:
		static void Main2();
	};
}