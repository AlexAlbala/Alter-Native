#pragma once
#include "System/System.h"
#include "System/Collections/Generic/IEnumerable.h"
#include "System/Collections/Generic/List.h"
#include "System/Collections/Generic/IEnumeratorCXX.h"
#include "MyEnumerator.h"

//using namespace System.Collections.Generic;
//using namespace System.Collections;
namespace CollectionsExample{

	template<typename T>
	class MyList : public IEnumerable<T>, /*public IEnumerable,*/ public Object, public gc_cleanup{
	public:
		List<T>* mylist;
		MyList(List<T>* values){
			this->mylist = values;
		}
		IEnumerator<T>* GetEnumerator()
		{
			return new MyEnumerator<T>(this->mylist);
		}
/*		IEnumerator* IEnumerable.GetEnumerator()
		{
			return this->GetEnumerator();
		}*/
	};
}