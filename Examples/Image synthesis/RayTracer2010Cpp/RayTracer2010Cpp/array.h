#pragma once
#include <assert.h>

template <class T>
struct Array
{
	Array(){data = 0; s = 0; c = 0;}
	Array(const Array<T> &a){data = 0; s = 0; c = 0; copy(a);}
	~Array(){clear();}

	T *data;
	int s, c;

	void operator=(const Array<T> &a) {copy(a);}
	void copy(const Array<T> &a)
	{
		if (a.s)
		{
			resize(a.s);
			for (int i = 0; i < s; i++)
				data[i] = a.data[i];
		}
	}

	int size(){return s;}
	bool empty(){return s == 0;}

	void resize(int S)
	{
		reserve(S);
		s = S;
	}

	void reserve(int C)
	{
		assert(C >= s);
		
		if (C <= 0)
		{
			c = 0;
			data = 0;
			return;
		}

		T *d = new T[C];

		for (int i = 0; i < s; i++)
			d[i] = data[i];
		delete[] data;
		data = d;
		c = C;
	}

	T &operator[](int i)
	{
		assert(c >= s);
		assert(i >= 0 && i < s);
		return data[i];
	}

	void clear()
	{
		s = 0;
		c = 0;
		delete[] data;
		data = 0;
	}

	void push_back(const T &v){push(v);}
	void insert(const T &v){push(v);}
	void push(const T &v)
	{
		if (s >= c)
			reserve(c > 0 ? c * 2 : 1);

		data[s] = v;
		s++;
	}

	T& push()
	{
		if (s >= c)
			reserve(c > 0 ? c * 2 : 1);
		return data[s++];
	}
	
	void pop_back(){pop();}
	void pop()
	{
		s--;

		if (s <= c / 2)
			reserve(s);
	}

	void erase(const T &v)
	{
		for (int i = 0; i < s; i++)
		{
			if (data[i] == v)
			{
				data[i] = data[s-1];
				pop();
				break;
			}
		}
	}

	int count(const T& v)
	{
		int n = 0;
		for (int i = 0; i < s; i++)
		{
			if (data[i] == v)
				n++;
		}
		return n;
	}

	bool contains(const T& v) {return count(v);}

	T* begin(){return data;}
	T* end(){return data + size;}
};
