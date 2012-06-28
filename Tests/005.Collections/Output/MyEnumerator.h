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

			class MyEnumerator : public IEnumerator<T*>*, IDisposable*, IEnumerator*, Object, gc_cleanup
			{
				private:
					List<T*>* values;
				private:
					int currentIndex;
				public:
					MyEnumerator(List<T*>* values);
				public:
					void Dispose();
				public:
					bool MoveNext();
				public:
					void Reset();
			};
		}