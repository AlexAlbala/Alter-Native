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

		class MyList : public gc_ptr<IEnumerable<gc_ptr<T>>>, gc_ptr<IEnumerable>
		{
			private:
				gc_ptr<List<gc_ptr<T>>> mylist;
			public:
				MyList(gc_ptr<List<gc_ptr<T>>> values);
			public:
				gc_ptr<IEnumerator<gc_ptr<T>>> GetEnumerator();
			private:
					gc_ptr<IEnumerator> GetEnumerator();
			};
		}