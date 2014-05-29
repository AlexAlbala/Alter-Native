/** \file
\brief Matrices
\author Josiah Manson

None of the functions in the matrices actually modify the current matrix,
except for the identity function. An example of how they work follows.

<code>
matrix3f A, B ,C;
vect3d p, q;

A = A.rotx(pi / 3); // these 2 lines are equivalent
A = B.rotx(pi / 3); // the matrix that calls the function is unaffected

C = A.mult(B); // read as "C = B x A". neither A or B is affected

q = C.mult(p); // read as "q = c x p"
</code>

With matrices, the order is from right to left, for example:

rotate x translate x scale

would read as do a scaling operation, then a translation, then apply a rotation

when applying a matrix to a point the following is the order

matrix x point

2x2
|0 1|
|2 3|

3x3
|0 1 2|
|3 4 5|
|6 7 8|

4x4
|0  1   2  3|
|4  5   6  7|
|8  9  10 11|
|12 13 14 15|
**/

#ifndef __matrix_h_
#define __matrix_h_

#include "vect.h"

//**********************************************************//
//******************definition of classes*******************//
//**********************************************************//

/// 2x2 matrix
class matrix2f
{
  public:
    float m[4];

    // constructors
    matrix2f() {}
    ~matrix2f() {}

    matrix2f(const matrix2f& mat);

    // functions
    static matrix2f identity();

    // 2d
    static matrix2f rotz(float rad);
    static matrix2f rotate(float rad) {return rotz(rad);}

    matrix2f transpose() const;
    matrix2f inverse() const;

    matrix2f mult(const matrix2f& mat) const;
    vect2f mult(const vect2f& vect) const;

	matrix2f operator*(const matrix2f& mat) const {return mult(mat);}
	vect2f operator*(const vect2f& vec) const {return mult(vec);}
	float &operator()(int r, int c) {return m[(r<<2) + c];}
};

//=========================================================//

/// 3x3 matrix
class matrix3f
{
  public:
    float m[9];

    // constructors
    matrix3f() {}
    ~matrix3f() {}

    matrix3f(const matrix2f& mat);
    matrix3f(const matrix3f& mat);

    // functions
    static matrix3f identity();
    static matrix3f zero();

    // 2d
    static matrix3f rotate(float rad) {return rotz(rad);}
    static matrix3f scale(float x, float y);
    static matrix3f translate(float x, float y);
    static matrix3f shear(float x, float y);

    static matrix3f rotateAround(float rad, float x, float y);
    static matrix3f scaleAround(float sx, float sy, float px, float py);

    static matrix3f scale(const vect2f& v) {return scale(v.v[0], v.v[1]);}
    static matrix3f translate(const vect2f& v) {return translate(v.v[0], v.v[1]);}
    static matrix3f shear(const vect2f& v) {return shear(v.v[0], v.v[1]);}
    static matrix3f rotateAround(float rad, vect2f v) {return rotateAround(rad, v.v[0], v.v[1]);}

    // 3d
    static matrix3f rotx(float rad);
    static matrix3f roty(float rad);
    static matrix3f rotz(float rad);
    static matrix3f rotxyz(const vect3d& euler);
	void rotzyz(const vect3d& euler);
	void rotzyz_t(const vect3d& euler);
    static matrix3f rotaxis(const vect3d& axis, float rad);

    matrix3f transpose() const;
    float determinant() const;
    matrix3f inverse() const;

    matrix3f mult(const matrix3f& mat) const;
    vect3d mult(const vect3d& vect) const;
    vect2f mult_pos(const vect2f& vect) const;

	matrix3f operator*(const matrix3f& mat) const {return mult(mat);}
	vect3d operator*(const vect3d& vec) const {return mult(vec);}
	float &operator()(int r, int c) {return m[r*3 + c];}

	void operator+=(const matrix3f& mat);
};

//=========================================================//

/// 4x4 matrix
class matrix4f
{
  public:
    float m[16];

    // constructors
    matrix4f() {}
    ~matrix4f() {}

    matrix4f(const matrix4f& mat);

    // functions
    static matrix4f identity();

    // 3d
    static matrix4f rotx(float rad);
    static matrix4f roty(float rad);
    static matrix4f rotz(float rad);
    static matrix4f rotxyz(const vect3d& euler);
	void rotzyz(const vect3d& euler);
	void rotzyz_t(const vect3d& euler);
    static matrix4f rotaxis(const vect3d& axis, float rad);

	static matrix4f scale(float s) { return scale(s,s,s); }
    static matrix4f scale(float x, float y, float z);
    static matrix4f translate(float x, float y, float z);
    static matrix4f shear(float x, float y, float z);

	static matrix4f scale(const vect3d &v) {return scale(v.v[0], v.v[1], v.v[2]);}
    static matrix4f translate(const vect3d &v) {return translate(v.v[0], v.v[1], v.v[2]);}
    static matrix4f shear(const vect3d &v) {return shear(v.v[0], v.v[1], v.v[2]);}

    matrix4f transpose() const;
    float determinant() const;
    matrix4f inverse() const;

    matrix4f mult(const matrix4f& mat) const;
    vect4f mult(const vect4f& vect) const;
    vect3d mult_pos(const vect3d& vect) const;
    vect3d mult_vec(const vect3d& vect) const;

	matrix4f operator*(const matrix4f& mat) const {return mult(mat);}
	vect4f operator*(const vect4f& vec) const {return mult(vec);}
	float &operator()(int r, int c) {return m[(r<<2) + c];}
};
#endif                                            // __matrix_h_
