#pragma once
	#include "System/System.h"
	#include "T.h"
	#include "IEnumerable.h"
	#include "System/Collections/Generic/List.h"
	#include "IEnumerator.h"
	#include "MyEnumerator.h"

	using namespace System.Collections.Generic;
	using namespace System.Collections;
	namespace CollectionsExample{

		class MyList : public IEnumerable<T*>*, IEnumerable*, Object, gc_cleanup
		{
			private:
				List<T*>* mylist;
			public:
				MyList(List<T*>* values);
			public:
				IEnumerator<T*>* GetEnumerator();
			private:
					IEnumerator* GetEnumerator();
			};
		}