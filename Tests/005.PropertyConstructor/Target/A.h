#pragma once
		#include "System/System.h"
		#include "A.h"
		#include "System/Console.h"

		using namespace System;
		namespace PropertyConstructor{

			class A : public Object, public gc_cleanup
			{
				private:
					int p;
				private:
						int getP();
					private:
							void setP(int value);
						public:
							static void Main(String args[]);
						private:
						static void Main1();
						private:
						static void Main2();
					};
				}