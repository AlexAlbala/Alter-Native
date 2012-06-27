#include "MyEnumerator.h"
		namespace CollectionsExample{

			MyEnumerator::MyEnumerator(List<T*>* values){
				this->values = new List<T*>(values);
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