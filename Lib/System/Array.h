#pragma once
#include <vector>
#include "Collections/Generic/IteratorCXX.h"

using namespace std;
using namespace System::Collections::Generic;
namespace System{
	template<typename T> class Array : public Object{
	private:
		vector<T>* data;		
	public:
		int Length;
		Array()
		{
			data = new vector<T>();
			this->Length = 0;
		}

		Array(int Length)
		{
			data = new vector<T>(Length);
			this->Length = Length;
		}

		Array(T* elements, int Length)
		{
			data = new vector<T>(elements, elements + Length);
			this->Length = Length;
		}

		T* GetData()
		{
			return data->data();
		}

		operator T*()
		{
			return data->data();
		}

		iteratorcxx<T>* begin()
		{			
			return data->begin();
		}

		iteratorcxx<T>* begin() const
		{
			return data->begin();
		}

		iteratorcxx<T>* end()
		{			
			return data->end();
		}

		iteratorcxx<T>* end() const
		{
			return data->end();
		}

		iteratorcxx<T>* rbegin()
		{			
			return data->rbegin();
		}

		iteratorcxx<T>* rbegin() const
		{
			return data->rbegin();
		}

		iteratorcxx<T>* rend()
		{			
			return data->rend();
		}

		iteratorcxx<T>* rend() const
		{
			return data->rend();
		}
	};
}