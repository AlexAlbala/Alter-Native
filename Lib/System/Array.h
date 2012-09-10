#pragma once
#include <vector>

using namespace std;
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
	};
}