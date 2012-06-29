#pragma once
#include "System/System.h"
#include "IEnumerable.h"
#include "System/Collections/Generic/List.h"
#include "IEnumerator.h"
#include "MyEnumerator.h"

using namespace System.Collections.Generic;
using namespace System.Collections;
namespace CollectionsExample{

	template<typename T>
	class MyList : public IEnumerable<T>, public IEnumerable, public Object, public gc_cleanup{
		List<T>* MyList::mylist;
		MyList::MyList(List<T>* values){
			this->mylist = values;
		}
		IEnumerator<T>* MyList::GetEnumerator()
		{
			return new MyEnumerator<T>(this->mylist);
		}
		IEnumerator* MyList::GetEnumerator()
		{
			return this->GetEnumerator();
		}
	};
}