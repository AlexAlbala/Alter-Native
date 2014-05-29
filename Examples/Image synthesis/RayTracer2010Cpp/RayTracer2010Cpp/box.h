#pragma once

#include "vect.h"

template <class T>
struct box3
{
	vect<3, T> v[2];

	box3(){}
	box3(vect<3, T> mine, vect<3, T> maxe)
	{
		v[0] = mine;
		v[1] = maxe;
	}

	vect<3, T> &operator[](int i) {return v[i];}

	void fix()
	{
		for (int i = 0; i < 3; i++)
		{
			if (v[1][i] < v[0][i])
				std::swap(v[0][i], v[1][i])
		}
	}

	bool hasSameVerts(box3<T> b)
	{
		return v[0] == v[1];
	}

	double volume()
	{
		vect<3, T> d = v[1] - v[0];
		return d[0]*d[1]*d[2];
	}

	vect<3, T> vertPos(int i)
	{
		vect<3, T> p = v[0];
		if ((i & 1) != 0)
			p[0] = v[1][0];
		if ((i & 2) != 0)
			p[1] = v[1][1];
		if ((i & 4) != 0)
			p[2] = v[1][2];
		return p;
	}

	double area()
	{
		vect<3, T> d = v[1] - v[0];
		return 2*d[0]*d[1] + 2*d[0]*d[2] + 2*d[1]*d[2];
	}
};

typedef box3<double> box3d;
typedef box3<float> box3f;
