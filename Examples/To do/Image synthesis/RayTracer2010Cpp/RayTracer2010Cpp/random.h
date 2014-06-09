#pragma once

// Created: March 07, 2005
// Author: Josiah Manson
// Description:
// rand() from the C stdlib is called the number of times required to make the randomXX() bit range number.
// Thus, srand() will seed it, and it will change rand() calls and vice versa.
// ToDo: Implement seperate random number generator from the C standard one.

// Modified: March 25, 2005
// Nothing modified, just a bit more description. The problem this header addresses is that the cstdlib rand()
// function only returns 15 bits worth of random bits. Thus, 2 calls generate a 30 bit number, and if you
// want the full 32 bits, 3 calls must be made to rand().

// Modified: March 25, 2005
// Added Mersenne Twister code. I didn't like the function names, so I changed them, but it's the same otherwise.
// The website for the code is http://www.math.sci.hiroshima-u.ac.jp/~m-mat/MT/emt.html

// Modified: February 11, 2010
// Added sphere code

#include "vect.h"

//*** these use cstdlib rand()
int rand_cstdlib15();
int rand_cstdlib30();
unsigned int rand_cstdlib32();

//*** the following functions use the mersenne twister
/* initializes mt[N] with a seed */
void rand_mt_init(unsigned long s);

/* initialize by an array with array-length */
void rand_mt_init_array(unsigned long init_key[], int key_length);

/* generates a random number on [0,0xffffffff]-interval */
unsigned long rand_mt_int32(void);

/* generates a random number on [0,0x7fffffff]-interval */
long rand_mt_int31(void);

/* generates a random number on [0,1]-real-interval */
double rand_mt_real1(void);

/* generates a random number on [0,1)-real-interval */
double rand_mt_real2(void);

/* generates a random number on (0,1)-real-interval */
double rand_mt_real3(void);

/* generates a random number on [0,1) with 53-bit resolution*/
double rand_mt_res53(void);

template <int N, class T>
void rand_mt_in_sphere(vect<N, T> &v)
{
	do
	{
		for (int i = 0; i < N; i++)
			v[i] = rand_mt_real1() * 2 - 1;
	} while (v*v > 1);
}

template <int N>
vect<N, double> rand_mt_in_sphere()
{
	vect<N, double> v;
	rand_mt_in_sphere(v);
	return v;
}


template <int N, class T>
void rand_mt_on_sphere(vect<N, T> &v)
{
	do
	{
		rand_mt_in_sphere(v);
	} while (v[0] == 0 && v[1] == 0 && v[2] == 0);

	v.normalize();
}

template <int N>
vect<N, double> rand_mt_on_sphere()
{
	vect<N, double> v;
	rand_mt_on_sphere(v);
	return v;
}
