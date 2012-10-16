// iterator example

#pragma once

#include <iostream>
#include <iterator>
#include "IEnumeratorCXX.h"

using namespace std;

namespace System{
	namespace Collections{

		class iteratorcxx : public iterator<input_iterator_tag, Object>
		{
		private:
		//T* p;
		IEnumerator *it;
		bool is_end_iterator;
		bool is_end;
		public:
			iteratorcxx(IEnumerator *_it){
				this->it = _it;
				this->is_end_iterator = false;
				this->is_end = false;
			}
					
			iteratorcxx(int n){
				if(!n)
					this->is_end_iterator = true;
			}
		
			Object* operator()(){
				return it->getCurrent();
			}
		
			Object& operator*(){
				return *(it->getCurrent());
			}
		
			iteratorcxx& operator++(){
				is_end = !it->MoveNext();
				return *this;
			}
		
			iteratorcxx operator++(int){
				is_end = !it->MoveNext();
				return *this;
			}
		
			iteratorcxx operator+(int value){
			for(int i=0; i < value;i++)
				is_end = !it->MoveNext();
		
			return *this;
			}
		
			iteratorcxx operator+=(int value){	
				return operator+(value);
			}
		
			bool operator==(const iteratorcxx& rhs){
				if(!rhs.is_end && !this->is_end) throw;
				else{
					if(rhs.is_end_iterator)
						return this->is_end;
					else if(this->is_end_iterator)
						return rhs.is_end;
					else
						throw;
				}
			}
		
			bool operator!=(const iteratorcxx& rhs){
				return !(operator==(rhs));
			}  
		};
	}
}