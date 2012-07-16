// iterator example

#pragma once

#include <iostream>
#include <iterator>
#include "IEnumeratorCXX.h"

using namespace std;

namespace System_Collections_Generic{

template<typename T> 
class myiterator : public iterator<input_iterator_tag, T>
{
private:
  //T* p;
  IEnumerator_T<T> *it;
  bool is_end_iterator;
  bool is_end;
public:
	myiterator(IEnumerator_T<T> *_it){
		this->it = _it;
		this->is_end_iterator = false;
		this->is_end = false;
	}

	myiterator(int n){
		if(!n)
			this->is_end_iterator = true;
	}

	T* operator()(){
		return it->getCurrent();
	}

	T& operator*(){
		return *(it->getCurrent());
	}

	myiterator& operator++(){
		is_end = it->MoveNext();
		return *this;
	}

	myiterator operator++(int){
		is_end = it->MoveNext();
		return *this;
	}

	myiterator operator+(int value){
	  for(int i=0; i < value;i++)
		  is_end = it->MoveNext();

	  return *this;
	}

	myiterator operator+=(int value){	
		  return operator+(value);
	}

	bool operator==(const myiterator& rhs){
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

	bool operator!=(const myiterator& rhs){
		return !(operator==(rhs));
	}

  /*myiterator(T* x) :p(x) 
  {
  }

  myiterator(const myiterator& mit) : p(mit.p) 
  {
  }

  myiterator& operator++() 
  {
	  ++p;
	  return *this;
  }

  myiterator operator++(int) 
  {
	  myiterator tmp(*this);
	  operator++(); 
	  return tmp;
  }

  myiterator operator+(int value)
  {
	  p += value;
	  return *this;
  }

  myiterator operator+=(int value)
  {	
	  myiterator tmp(*this);
	  operator+(value);
	  return tmp;
  }

  bool operator==(const myiterator& rhs) 
  {
	  return p==rhs.p;
  }

  bool operator!=(const myiterator& rhs) 
  {
	  return p!=rhs.p;
  }
  T& operator*() 
  {
	  return *p;
  }*/
};
}