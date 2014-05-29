/** \file
\brief Tree View of scene
\author Josiah Manson

Modified: May 23, 2006
changed constructors to compile with gcc 4.0

Modified: July 07, 2005
added constructor to vect4<T>

Modified: May 04, 2009
rewrote to be templatized by length
**/

#pragma once

#ifndef __vect_h_
#define __vect_h_

//#pragma warning (disable: 4786 4244 4305)

#include <assert.h>
#include <cmath>
#include <algorithm>

// n d vector
template <int N, class T>
struct vect
{
	T v[N];
	
	// constructors
	~vect() {}
	vect() {}

	vect(const T v0)
	{
		for (int i = 0; i < N; i++)
			v[i] = v0;
	}

	vect(const T v0, const T v1)
	{
		if (N >= 1)
			v[0] = v0;
		if (N >= 2)
			v[1] = v1;
	}

	vect(const T v0, const T v1, const T v2)
	{
		if (N >= 1)
			v[0] = v0;
		if (N >= 2)
			v[1] = v1;
		if (N >= 3)
			v[2] = v2;
	}

	vect(const T v0, const T v1, const T v2, const T v3)
	{
		if (N >= 1)
			v[0] = v0;
		if (N >= 2)
			v[1] = v1;
		if (N >= 3)
			v[2] = v2;
		if (N >= 4)
			v[3] = v3;
	}

	vect(const T v0, const T v1, const T v2, const T v3, const T v4)
	{
		if (N >= 1)
			v[0] = v0;
		if (N >= 2)
			v[1] = v1;
		if (N >= 3)
			v[2] = v2;
		if (N >= 4)
			v[3] = v3;
		if (N >= 5)
			v[4] = v4;
	}

	template <int M, class S>
	vect(const vect<M, S> &a) 
	{
		int n;
		if (N < M)
			n = N;
		else
			n = M;

		for (int i = 0; i < n; i++)
			v[i] = a.v[i];
	}


	void setitem(int i, T val)
	{
		assert(i >= 0 && i < N);
		v[i] = val;
	}

	T getitem(int i)
	{
		assert(i >= 0 && i < N);
		return v[i];
	}

	
	void operator()(const T v0)
	{
		for (int i = 0; i < N; i++)
			v[i] = v0;
	}

	void operator()(const T v0, const T v1)
	{
		if (N >= 1)
			v[0] = v0;
		if (N >= 2)
			v[1] = v1;
	}

	void operator()(const T v0, const T v1, const T v2)
	{
		if (N >= 1)
			v[0] = v0;
		if (N >= 2)
			v[1] = v1;
		if (N >= 3)
			v[2] = v2;
	}

	void operator()(const T v0, const T v1, const T v2, const T v3)
	{
		if (N >= 1)
			v[0] = v0;
		if (N >= 2)
			v[1] = v1;
		if (N >= 3)
			v[2] = v2;
		if (N >= 4)
			v[3] = v3;
	}

	void operator()(const T v0, const T v1, const T v2, const T v3, const T v4)
	{
		if (N >= 1)
			v[0] = v0;
		if (N >= 2)
			v[1] = v1;
		if (N >= 3)
			v[2] = v2;
		if (N >= 4)
			v[3] = v3;
		if (N >= 5)
			v[4] = v4;
	}

	template <int M, class S>
	void operator=(const vect<M, S> &a) 
	{
		int n;
		if (N < M)
			n = N;
		else
			n = M;

		for (int i = 0; i < n; i++)
			v[i] = a.v[i];
	}

	template <class T>
	void operator=(const T a) 
	{
		for (int i = 0; i < N; i++)
			v[i] = a;
	}

	// accessors
	T &operator[](const int i)
	{
		assert(i >= 0 && i < N);
		return v[i];
	}

	// scalar functions
	void add(const T a)
	{
		for (int i = 0; i < N; i++)
			v[i] += a;
	}

	void subtract(const T a)
	{
		for (int i = 0; i < N; i++)
			v[i] -= a;
	}
	
	vect<N, T> multiply(const vect<N, T> &a)
	{
		vect<N, T> r;
		for (int i = 0; i < N; i++)
			r[i] = v[i] * a.v[i];
		return r;
	}

	void multiply(const T a)
	{
		for (int i = 0; i < N; i++)
			v[i] *= a;
	}
	
	void divide(const T a)
	{
		for (int i = 0; i < N; i++)
			v[i] /= a;
	}

	// vector functions
	template <int M, class S>
	void add(const vect<M, S> &a)
	{
		int n;
		if (N < M)
			n = N;
		else
			n = M;

		for (int i = 0; i < n; i++)
			v[i] += a.v[i];
	}

	template <int M, class S>
	void subtract(const vect<M, S> &a)
	{
		int n;
		if (N < M)
			n = N;
		else
			n = M;

		for (int i = 0; i < n; i++)
			v[i] -= a.v[i];
	}

	template <int M, class S>
	T dot(const vect<M, S> &a) const
	{
		int n;
		if (N < M)
			n = N;
		else
			n = M;

		T r = 0;
		for (int i = 0; i < n; i++)
			r += v[i]*a.v[i];
		return r;
	}
	
	template <int M, class S>
	vect<3, T> cross(const vect<M, S> &a) const
	{
		vect<3, T> r;

		if (M == 3 && N == 3)
		{
			r.v[0] = v[1] * a.v[2] - v[2] * a.v[1];
			r.v[1] = v[2] * a.v[0] - v[0] * a.v[2];
			r.v[2] = v[0] * a.v[1] - v[1] * a.v[0];
		}
		else if (M == 2 && N == 2)
		{
			r.v[0] = 0;
			r.v[1] = 0;
			r.v[2] = v[0] * a.v[1] - v[1] * a.v[0];
		}
		else if (M == 3 && N == 2)
		{
			r.v[0] = -v[2] * a.v[1];
			r.v[1] = v[2] * a.v[0];
			r.v[2] = v[0] * a.v[1] - v[1] * a.v[0];
		}
		else if (M == 2 && N == 3)
		{
			r.v[0] = v[1] * a.v[2];
			r.v[1] = -v[0] * a.v[2];
			r.v[2] = v[0] * a.v[1] - v[1] * a.v[0];
		}

		return r;
	}

	// unary functions
	void negate()
	{
		for (int i = 0; i < N; i++)
			v[i] = -v[i];
	}

	T length() const
	{
		return sqrt(length2());
	}
	T length2() const
	{
		return dot(*this);
	}
	void normalize()
	{
		const T f = 1.0 / length();
		for (int i = 0; i < N; i++)
			v[i] *= f;
	}

	// vector operators
	template <int M, class S> 
	vect<3, T> operator%(const vect<M, S> &a) const 
	{
		return cross(a);
	}

	template <int M, class S> 
	vect<N, T> operator+(const vect<M, S> &a) const 
	{
		int n;
		if (N < M)
			n = N;
		else
			n = M;

		vect<N, T> r; 
		for (int i = 0; i < N; i++)
			r.v[i] = v[i] + a.v[i];
		return r;
	}
	
	template <int M, class S> 
	vect<N, T> operator-(const vect<M, S> &a) const 
	{
		int n;
		if (N < M)
			n = N;
		else
			n = M;

		vect<N, T> r; 
		for (int i = 0; i < N; i++)
			r.v[i] = v[i] - a.v[i];
		return r;
	}
	
	template <int M, class S> 
	vect<N, T> operator/(const vect<M, S> &a) const 
	{
		int n;
		if (N < M)
			n = N;
		else
			n = M;

		vect<N, T> r; 
		for (int i = 0; i < N; i++)
			r.v[i] = v[i] / a.v[i];
		return r;
	}
	
	template <int M, class S> 
	T operator*(const vect<M, S> &a) const 
	{
		return dot(a);
	}
	
	// scalar operators
	template <class S> 
	friend vect<N, T> operator*(const S a, vect<N, T>);

	template <class S> 
	vect<N, T> operator*(const S a) const 
	{
		vect<N, T> r; 
		for (int i = 0; i < N; i++)
			r.v[i] = v[i] * a;
		return r;
	}
	template <class S> 
	vect<N, T> operator/(const S a) const 
	{
		vect<N, T> r; 
		for (int i = 0; i < N; i++)
			r.v[i] = v[i] / a;
		return r;
	}
	vect<N, T> operator+(const T a) const 
	{
		vect<N, T> r; 
		for (int i = 0; i < N; i++)
			r.v[i] = v[i] + a;
		return r;
	}
	vect<N, T> operator-(const T a) const 
	{
		vect<N, T> r; 
		for (int i = 0; i < N; i++)
			r.v[i] = v[i] - a;
		return r;
	}

	// unary operators
	vect<N, T> operator-() const 
	{
		vect<N, T> r; 
		for (int i = 0; i < N; i++)
			r.v[i] = -v[i];
		return r;
	}
	
	T operator!() const 
	{
		return length();
	}
	
	vect<N, T> operator~() const 
	{
		double f = 1.0 / length();

		vect<N, T> r; 
		for (int i = 0; i < N; i++)
			r.v[i] = v[i] * f;
		return r;
	}

	// update operators
	template <int M, class S> 
	void operator+=(const vect<M, S> &a)
	{
		add(a);
	}
	template <int M, class S> 
	void operator-=(const vect<M, S> &a)
	{
		subtract(a);
	}

	template <class S> 
	void operator+=(const S a)
	{
		add(a);
	}
	template <class S> 
	void operator-=(const S a)
	{
		subtract(a);
	}
	template <class S> 
	void operator*=(const S a)
	{
		multiply(a);
	}
	template <class S> 
	void operator/=(const S a)
	{
		divide(a);
	}

	// comparison functions
	bool operator==(const vect<N, T> &a) const 
	{
		for (int i = 0; i < N; i++)
			if (v[i] != a.v[i])
				return false;
		return true;
	}
	bool operator!=(const vect<N, T> &a) const 
	{
		return !(*this == a);
	}
	bool operator<(const vect<N, T> &a) const 
	{
		return lexicographical_compare(v, v+N, a.v, a.v+N);
	}
};

template <int N, class S, class T> 
vect<N, T> operator*(const S a, vect<N, T>);

#ifndef WIN32
#define __int8 char
#define __int16 short
#define __int32 int
#define __int64 long long
#endif

typedef vect<2, float> vect2f;
typedef vect<2, double> vect2d;
typedef vect<2, __int8> vect2b;
typedef vect<2, __int16> vect2s;
typedef vect<2, __int32> vect2i;
typedef vect<2, __int64> vect2l;
typedef vect<2, unsigned __int8> vect2ub;
typedef vect<2, unsigned __int16> vect2us;
typedef vect<2, unsigned __int32> vect2ui;
typedef vect<2, unsigned __int64> vect2ul;

typedef vect<3, float> vect3f;
typedef vect<3, double> vect3d;
typedef vect<3, __int8> vect3b;
typedef vect<3, __int16> vect3s;
typedef vect<3, __int32> vect3i;
typedef vect<3, __int64> vect3l;
typedef vect<3, unsigned __int8> vect3ub;
typedef vect<3, unsigned __int16> vect3us;
typedef vect<3, unsigned __int32> vect3ui;
typedef vect<3, unsigned __int64> vect3ul;

typedef vect<4, float> vect4f;
typedef vect<4, double> vect4d;
typedef vect<4, __int8> vect4b;
typedef vect<4, __int16> vect4s;
typedef vect<4, __int32> vect4i;
typedef vect<4, __int64> vect4l;
typedef vect<4, unsigned __int8> vect4ub;
typedef vect<4, unsigned __int16> vect4us;
typedef vect<4, unsigned __int32> vect4ui;
typedef vect<4, unsigned __int64> vect4ul;

typedef vect<5, float> vect5f;
typedef vect<5, double> vect5d;
typedef vect<5, __int8> vect5b;
typedef vect<5, __int16> vect5s;
typedef vect<5, __int32> vect5i;
typedef vect<5, __int64> vect5l;
typedef vect<5, unsigned __int8> vect5ub;
typedef vect<5, unsigned __int16> vect5us;
typedef vect<5, unsigned __int32> vect5ui;
typedef vect<5, unsigned __int64> vect5ul;

#endif // __vect_h_
