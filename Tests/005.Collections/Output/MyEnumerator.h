#pragma once
		#include "System/System.h"
		#include "T.h"
		#include "IEnumerator.h"
		#include "IDisposable.h"
		#include "System/Collections/Generic/List.h"

		using namespace System.Collections.Generic;
		using namespace System;
		using namespace System.Collections;
		namespace CollectionsExample{

			class MyEnumerator : public gc_ptr<IEnumerator<gc_ptr<T>>>, gc_ptr<IDisposable>, gc_ptr<IEnumerator>
			{
				private:
					gc_ptr<List<gc_ptr<T>>> values;
				private:
					int currentIndex;
				public:
					MyEnumerator(gc_ptr<List<gc_ptr<T>>> values);
				public:
					void Dispose();
				public:
					bool MoveNext();
				public:
					void Reset();
			};
		}