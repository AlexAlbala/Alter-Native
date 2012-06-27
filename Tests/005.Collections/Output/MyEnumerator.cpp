#include "MyEnumerator.h"
		namespace CollectionsExample{

			MyEnumerator::MyEnumerator(gc_ptr<List<gc_ptr<T>>> values){
				this->values = gc_ptr<List<gc_ptr<T>>> (new(gc)List<gc_ptr<T>>(values));
				this->Reset();
			}
			void MyEnumerator::Dispose()
			{
			}
			bool MyEnumerator::MoveNext()
			{
				this->currentIndex += 1;
				return this->currentIndex < this->values->Count;
			}
			void MyEnumerator::Reset()
			{
				this->currentIndex = -1;
			}

		}