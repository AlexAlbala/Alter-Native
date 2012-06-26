#include "MyList.h"
	namespace CollectionsExample{

		MyList::MyList(gc_ptr<List<gc_ptr<T>>> values){
			this->mylist = values;
		}
		gc_ptr<IEnumerator<gc_ptr<T>>> MyList::GetEnumerator()
		{
			return gc_ptr<MyEnumerator<gc_ptr<T>>> (new(gc)MyEnumerator<gc_ptr<T>>(this->mylist));
		}
		gc_ptr<IEnumerator> MyList::GetEnumerator()
		{
			return this->GetEnumerator();
		}

	}