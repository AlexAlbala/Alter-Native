#include "MyList.h"
	namespace CollectionsExample{

		MyList::MyList(List<T*>* values){
			this->mylist = values;
		}
		IEnumerator<T*>* MyList::GetEnumerator()
		{
			return new MyEnumerator<T*>(this->mylist);
		}
		IEnumerator* MyList::GetEnumerator()
		{
			return this->GetEnumerator();
		}

	}