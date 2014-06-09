#pragma once
#include <assert.h>

template <class T>
struct Array2D
{
	Array2D(){data = 0; clear();}
	Array2D(const Array2D<T> &a){data = 0; copy(a);}
	~Array2D(){clear();}

	T *data;
	int data_size;
	int size[2];

	void operator=(const Array2D<T> &a) {copy(a);}
	void copy(const Array2D<T> &a)
	{
		resize(a.size[0], a.size[1]);
		for (int i = 0; i < data_size; i++)
			data[i] = a.data[i];
	}

	void resize(int w, int h)
	{
		clear();

		data_size = w*h;
		size[0] = w;
		size[1] = h;

		data = new T[data_size];
	}

	T &operator()(int x, int y)
	{
		return data[y*size[0] + x];
	}

	void clear()
	{
		size[0] = size[1] = 0;
		data_size = 0;
		delete[] data;
		data = 0;
	}
};
