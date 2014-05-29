/** \file
\brief Matrices
\author Josiah Manson

**/

#include "matrix.h"

//**********************************************************//
//*********************matrix2f functions*******************//
//**********************************************************//

inline matrix2f::matrix2f(const matrix2f& mat)
{
	m[0] = mat.m[0];
	m[1] = mat.m[1];
	m[2] = mat.m[2];
	m[3] = mat.m[3];
}


//---------------------------------------------------------//

matrix2f matrix2f::identity()
{
	matrix2f ret;

	ret.m[0] = 1;
	ret.m[1] = 0;
	ret.m[2] = 0;
	ret.m[3] = 1;

	return ret;
}


//---------------------------------------------------------//

matrix2f matrix2f::rotz(float rad)
{
	matrix2f ret;
	float c = (float)cos(rad);
	float s = (float)sin(rad);

	ret.m[0] = c;
	ret.m[1] = -s;
	ret.m[2] = s;
	ret.m[3] = c;

	return ret;
}


//---------------------------------------------------------//

matrix2f matrix2f::mult(const matrix2f& mat) const
{
	matrix2f ret;

	ret.m[0] = mat.m[0] * m[0] + mat.m[1] * m[2];
	ret.m[1] = mat.m[0] * m[1] + mat.m[1] * m[3];
	ret.m[2] = mat.m[2] * m[0] + mat.m[3] * m[2];
	ret.m[3] = mat.m[2] * m[1] + mat.m[3] * m[3];

	return ret;
}


//---------------------------------------------------------//

vect2f matrix2f::mult(const vect2f& vect) const
{
	vect2f ret;

	ret.v[0] = m[0] * vect.v[0] + m[1] * vect.v[1];
	ret.v[1] = m[2] * vect.v[0] + m[3] * vect.v[1];

	return ret;
}


//**********************************************************//
//*********************matrix3f functions*******************//
//**********************************************************//

matrix3f::matrix3f(const matrix2f& mat)
{
	m[0] = mat.m[0];
	m[1] = mat.m[1];
	m[2] = 0;
	m[3] = mat.m[2];
	m[4] = mat.m[3];
	m[5] = 0;
	m[6] = 0;
	m[7] = 0;
	m[8] = 1;
}


matrix3f::matrix3f(const matrix3f& mat)
{
	m[0] = mat.m[0];
	m[1] = mat.m[1];
	m[2] = mat.m[2];
	m[3] = mat.m[3];
	m[4] = mat.m[4];
	m[5] = mat.m[5];
	m[6] = mat.m[6];
	m[7] = mat.m[7];
	m[8] = mat.m[8];
}


matrix3f matrix3f::identity()
{
	matrix3f ret;

	ret.m[0] = 1;
	ret.m[1] = 0;
	ret.m[2] = 0;
	ret.m[3] = 0;
	ret.m[4] = 1;
	ret.m[5] = 0;
	ret.m[6] = 0;
	ret.m[7] = 0;
	ret.m[8] = 1;

	return ret;
}

void matrix3f::operator+=(const matrix3f& mat)
{
	for (int i = 0; i < 9; i++)
		m[i] += mat.m[i];
}

matrix3f matrix3f::zero()
{
	matrix3f ret;

	ret.m[0] = 0;
	ret.m[1] = 0;
	ret.m[2] = 0;
	ret.m[3] = 0;
	ret.m[4] = 0;
	ret.m[5] = 0;
	ret.m[6] = 0;
	ret.m[7] = 0;
	ret.m[8] = 0;

	return ret;
}

matrix3f matrix3f::scale(float x, float y)
{
	matrix3f ret;

	ret.m[0] = x;
	ret.m[1] = 0;
	ret.m[2] = 0;
	ret.m[3] = 0;
	ret.m[4] = y;
	ret.m[5] = 0;
	ret.m[6] = 0;
	ret.m[7] = 0;
	ret.m[8] = 1;

	return ret;
}


matrix3f matrix3f::translate(float x, float y)
{
	matrix3f ret;

	ret.m[0] = 1;
	ret.m[1] = 0;
	ret.m[2] = x;
	ret.m[3] = 0;
	ret.m[4] = 1;
	ret.m[5] = y;
	ret.m[6] = 0;
	ret.m[7] = 0;
	ret.m[8] = 1;

	return ret;
}


matrix3f matrix3f::shear(float x, float y)
{
	matrix3f ret;

	ret.m[0] = 1;
	ret.m[1] = x;
	ret.m[2] = 0;
	ret.m[3] = y;
	ret.m[4] = 1;
	ret.m[5] = 0;
	ret.m[6] = 0;
	ret.m[7] = 0;
	ret.m[8] = 1;

	return ret;
}


matrix3f matrix3f::rotateAround(float rad, float x, float y)
{
	matrix3f mat;

	mat = mat.translate(-x, -y);
	mat = mat.mult( mat.rotz(rad) );
	mat = mat.mult( mat.translate(x, y) );

	return mat;
}


matrix3f matrix3f::scaleAround(float sx, float sy, float x, float y)
{
	matrix3f mat;

	mat = mat.translate(-x, -y);
	mat = mat.mult( mat.scale(sx, sy) );
	mat = mat.mult( mat.translate(x, y) );

	return mat;
}


matrix3f matrix3f::rotx(float rad)
{
	matrix3f ret;
	float c = (float)cos(rad);
	float s = (float)sin(rad);

	ret.m[0] = 1;
	ret.m[1] = 0;
	ret.m[2] = 0;
	ret.m[3] = 0;
	ret.m[4] = c;
	ret.m[5] = s;
	ret.m[6] = 0;
	ret.m[7] = -s;
	ret.m[8] = c;

	return ret;
}


matrix3f matrix3f::roty(float rad)
{
	matrix3f ret;
	float c = (float)cos(rad);
	float s = (float)sin(rad);

	ret.m[0] = c;
	ret.m[1] = 0;
	ret.m[2] = -s;
	ret.m[3] = 0;
	ret.m[4] = 1;
	ret.m[5] = 0;
	ret.m[6] = s;
	ret.m[7] = 0;
	ret.m[8] = c;

	return ret;
}


matrix3f matrix3f::rotz(float rad)
{
	matrix3f ret;
	float c = (float)cos(rad);
	float s = (float)sin(rad);

	ret.m[0] = c;
	ret.m[1] = s;
	ret.m[2] = 0;
	ret.m[3] = -s;
	ret.m[4] = c;
	ret.m[5] = 0;
	ret.m[6] = 0;
	ret.m[7] = 0;
	ret.m[8] = 1;

	return ret;
}


matrix3f matrix3f::rotxyz(const vect3d& euler)
{
	matrix3f ret;
	float c1 = (float)cos(euler.v[0]);
	float s1 = (float)sin(euler.v[0]);
	float c2 = (float)cos(euler.v[1]);
	float s2 = (float)sin(euler.v[1]);
	float c3 = (float)cos(euler.v[2]);
	float s3 = (float)sin(euler.v[2]);

	ret.m[0] =  c2*c3;
	ret.m[1] =  c2*s3;
	ret.m[2] =  -s2;
	ret.m[3] = s1*s2*c3-c1*s3;
	ret.m[4] = s1*s2*s3+c1*c3;
	ret.m[5] = s1*c2;
	ret.m[6] = c1*s2*c3+s1*s3;
	ret.m[7] = c1*s2*s3-s1*c3;
	ret.m[8] = c1*c2;

	return ret;
}
void matrix3f::rotzyz_t(const vect3d& euler)
{
	float c1 = (float)cos(euler.v[0]);
	float s1 = (float)sin(euler.v[0]);
	float c2 = (float)cos(euler.v[1]);
	float s2 = (float)sin(euler.v[1]);
	float c3 = (float)cos(euler.v[2]);
	float s3 = (float)sin(euler.v[2]);

	m[0]=c1*c2*c3-s1*s3;
	m[1]=c3*s1+c1*c2*s3;
	m[2]=-c1*s2;

	m[3]=-c2*c3*s1-c1*s3;
	m[4]=c1*c3-c2*s1*s3;
	m[5]=s1*s2;

	m[6]=c3*s2;
	m[7]=s2*s3;
	m[8]=c2;
}

void matrix3f::rotzyz(const vect3d& euler)
{

	float c1 = (float)cos(euler.v[0]);
	float s1 = (float)sin(euler.v[0]);
	float c2 = (float)cos(euler.v[1]);
	float s2 = (float)sin(euler.v[1]);
	float c3 = (float)cos(euler.v[2]);
	float s3 = (float)sin(euler.v[2]);

	m[0]=c1*c2*c3-s1*s3;
	m[3]=c3*s1+c1*c2*s3;
	m[6]=-c1*s2;

	m[1]=-c2*c3*s1-c1*s3;
	m[4]=c1*c3-c2*s1*s3;
	m[7]=s1*s2;

	m[2]=c3*s2;
	m[5]=s2*s3;
	m[9]=c2;
}

matrix3f matrix3f::rotaxis(const vect3d& axis, float rad)
{
	matrix3f ret;
	float c = cos(rad);
	float s = sin(rad);
	float t = 1 - c;

	float x = axis.v[0];
	float y = axis.v[1];
	float z = axis.v[2];

	ret.m[0] = t*x*x + c;
	ret.m[1] = t*x*y - s*z;
	ret.m[2] = t*x*z + s*y;
	ret.m[3] = t*x*y + s*z;
	ret.m[4] = t*y*y + c;
	ret.m[5] = t*y*z - s*x;
	ret.m[6] = t*x*z - s*y;
	ret.m[7] = t*y*z + s*x;
	ret.m[8] = t*z*z + c;

	return ret;
}

float matrix3f::determinant() const
{
	const float a11 = m[0], a12 = m[1], a13 = m[2];
	const float a21 = m[3], a22 = m[4], a23 = m[5];
	const float a31 = m[6], a32 = m[7], a33 = m[8];

	return a11*a22*a33 + a12*a23*a31 + a13*a21*a32 - a11*a23*a32 - a12*a21*a33 - a13*a22*a31;
}

matrix3f matrix3f::inverse() const
{
	const float det = determinant();
	const float det_inv = 1.0 / det;

	matrix3f ret;

	const float a11 = m[0], a12 = m[1], a13 = m[2];
	const float a21 = m[3], a22 = m[4], a23 = m[5];
	const float a31 = m[6], a32 = m[7], a33 = m[8];

	ret.m[0] = a33*a22-a32*a23;
	ret.m[1] = -(a33*a12-a32*a13);
	ret.m[2] = a23*a12-a22*a13;
	ret.m[3] = -(a33*a21-a31*a23);
	ret.m[4] = a33*a11-a31*a13;
	ret.m[5] = -(a23*a11-a21*a13);
	ret.m[6] = a32*a21-a31*a22;
	ret.m[7] = -(a32*a11-a31*a12);
	ret.m[8] = a22*a11-a21*a12;

	for (int i = 0; i < 9; i++)
		ret.m[i] *= det_inv;

	return ret;
}

matrix3f matrix3f::transpose() const
{
	matrix3f ret;

	ret.m[0] = m[0];
	ret.m[1] = m[3];
	ret.m[2] = m[6];
	ret.m[3] = m[1];
	ret.m[4] = m[4];
	ret.m[5] = m[7];
	ret.m[6] = m[2];
	ret.m[7] = m[5];
	ret.m[8] = m[8];

	return ret;
}


matrix3f matrix3f::mult(const matrix3f& mat) const
{
	matrix3f ret;

	ret.m[0] = mat.m[0] * m[0] + mat.m[1] * m[3] + mat.m[2] * m[6];
	ret.m[1] = mat.m[0] * m[1] + mat.m[1] * m[4] + mat.m[2] * m[7];
	ret.m[2] = mat.m[0] * m[2] + mat.m[1] * m[5] + mat.m[2] * m[8];
	ret.m[3] = mat.m[3] * m[0] + mat.m[4] * m[3] + mat.m[5] * m[6];
	ret.m[4] = mat.m[3] * m[1] + mat.m[4] * m[4] + mat.m[5] * m[7];
	ret.m[5] = mat.m[3] * m[2] + mat.m[4] * m[5] + mat.m[5] * m[8];
	ret.m[6] = mat.m[6] * m[0] + mat.m[7] * m[3] + mat.m[8] * m[6];
	ret.m[7] = mat.m[6] * m[1] + mat.m[7] * m[4] + mat.m[8] * m[7];
	ret.m[8] = mat.m[6] * m[2] + mat.m[7] * m[5] + mat.m[8] * m[8];

	return ret;
}


vect2f matrix3f::mult_pos(const vect2f& src) const
{
	vect2f ret;
	float div = m[6] * src.v[0] + m[7] * src.v[1] + m[8];

	ret.v[0] = (m[0] * src.v[0] + m[1] * src.v[1] + m[2]) / div;
	ret.v[1] = (m[3] * src.v[0] + m[4] * src.v[1] + m[5]) / div;

	return ret;
}


vect3d matrix3f::mult(const vect3d& src) const
{
	vect3d ret;

	ret.v[0] = m[0] * src.v[0] + m[1] * src.v[1] + m[2] * src.v[2];
	ret.v[1] = m[3] * src.v[0] + m[4] * src.v[1] + m[5] * src.v[2];
	ret.v[2] = m[6] * src.v[0] + m[7] * src.v[1] + m[8] * src.v[2];

	return ret;
}


//**********************************************************//
//*********************matrix4f functions*******************//
//**********************************************************//

matrix4f::matrix4f(const matrix4f& mat)
{
	m[0] = mat.m[0];
	m[1] = mat.m[1];
	m[2] = mat.m[2];
	m[3] = mat.m[3];
	m[4] = mat.m[4];
	m[5] = mat.m[5];
	m[6] = mat.m[6];
	m[7] = mat.m[7];
	m[8] = mat.m[8];
	m[9] = mat.m[9];
	m[10] = mat.m[10];
	m[11] = mat.m[11];
	m[12] = mat.m[12];
	m[13] = mat.m[13];
	m[14] = mat.m[14];
	m[15] = mat.m[15];
}


matrix4f matrix4f::identity()
{
	matrix4f ret;

	ret.m[0] = ret.m[5] = ret.m[10] = ret.m[15] = 1;
	ret.m[1] = ret.m[2] = ret.m[3] = ret.m[4] = ret.m[6] = ret.m[7] = ret.m[8] = ret.m[9] = ret.m[11] = ret.m[12] = ret.m[13] = ret.m[14] = 0;

	return ret;
}


matrix4f matrix4f::rotx(float rad)
{
	matrix4f ret;
	float c = (float)cos(rad);
	float s = (float)sin(rad);

	ret.m[0] = 1;
	ret.m[5] = c;
	ret.m[6] = s;
	ret.m[9] = -s;
	ret.m[10] = c;

	ret.m[1] = ret.m[2] = ret.m[3] = ret.m[4] = ret.m[7] = ret.m[8] = ret.m[11] = ret.m[12] = ret.m[13] = ret.m[14] = 0;
	ret.m[15] = 1;

	return ret;
}


matrix4f matrix4f::roty(float rad)
{
	matrix4f ret;
	float c = (float)cos(rad);
	float s = (float)sin(rad);

	ret.m[0] = c;
	ret.m[2] = -s;
	ret.m[5] = 1;
	ret.m[8] = s;
	ret.m[10] = c;

	ret.m[1] = ret.m[3] = ret.m[4] = ret.m[6] = ret.m[7] = ret.m[9] = ret.m[11] = ret.m[12] = ret.m[13] = ret.m[14] = 0;
	ret.m[15] = 1;

	return ret;
}


matrix4f matrix4f::rotz(float rad)
{
	matrix4f ret;
	float c = (float)cos(rad);
	float s = (float)sin(rad);

	ret.m[0] = c;
	ret.m[1] = s;
	ret.m[4] = -s;
	ret.m[5] = c;
	ret.m[10] = 1;

	ret.m[2] = ret.m[3] = ret.m[6] = ret.m[7] = ret.m[8] = ret.m[9] = ret.m[11] = ret.m[12] = ret.m[13] = ret.m[14] = 0;
	ret.m[15] = 1;

	return ret;
}


matrix4f matrix4f::rotxyz(const vect3d& euler)
{
	matrix4f ret;
	float c1 = (float)cos(euler.v[0]);
	float s1 = (float)sin(euler.v[0]);
	float c2 = (float)cos(euler.v[1]);
	float s2 = (float)sin(euler.v[1]);
	float c3 = (float)cos(euler.v[2]);
	float s3 = (float)sin(euler.v[2]);

	ret.m[0] =  c2*c3;
	ret.m[1] =  c2*s3;
	ret.m[2] =  -s2;
	ret.m[4] = s1*s2*c3-c1*s3;
	ret.m[5] = s1*s2*s3+c1*c3;
	ret.m[6] = s1*c2;
	ret.m[8] = c1*s2*c3+s1*s3;
	ret.m[9] = c1*s2*s3-s1*c3;
	ret.m[10] = c1*c2;

	ret.m[3] = ret.m[7] = ret.m[11] = ret.m[12] = ret.m[13] = ret.m[14] = 0;
	ret.m[15] = 1;

	return ret;
}

void matrix4f::rotzyz_t(const vect3d& euler)
{
	float c1 = (float)cos(euler.v[0]);
	float s1 = (float)sin(euler.v[0]);
	float c2 = (float)cos(euler.v[1]);
	float s2 = (float)sin(euler.v[1]);
	float c3 = (float)cos(euler.v[2]);
	float s3 = (float)sin(euler.v[2]);

	m[0]=c1*c2*c3-s1*s3;
	m[1]=c3*s1+c1*c2*s3;
	m[2]=-c1*s2;

	m[4]=-c2*c3*s1-c1*s3;
	m[5]=c1*c3-c2*s1*s3;
	m[6]=s1*s2;

	m[8]=c3*s2;
	m[9]=s2*s3;
	m[10]=c2;

	m[3] = m[7] = m[11] = m[12] = m[13] = m[14] = 0;
	m[15] = 1;
}

void matrix4f::rotzyz(const vect3d& euler)
{
	float c1 = (float)cos(euler.v[0]);
	float s1 = (float)sin(euler.v[0]);
	float c2 = (float)cos(euler.v[1]);
	float s2 = (float)sin(euler.v[1]);
	float c3 = (float)cos(euler.v[2]);
	float s3 = (float)sin(euler.v[2]);

	m[0]=c1*c2*c3-s1*s3;
	m[4]=c3*s1+c1*c2*s3;
	m[8]=-c1*s2;

	m[1]=-c2*c3*s1-c1*s3;
	m[5]=c1*c3-c2*s1*s3;
	m[9]=s1*s2;

	m[2]=c3*s2;
	m[6]=s2*s3;
	m[10]=c2;

	m[3] = m[7] = m[11] = m[12] = m[13] = m[14] = 0;
	m[15] = 1;
}

matrix4f matrix4f::rotaxis(const vect3d& axis, float rad)
{
	matrix4f ret;
	float c = cos(rad);
	float s = sin(rad);
	float t = 1 - c;

	float x = axis.v[0];
	float y = axis.v[1];
	float z = axis.v[2];

	ret.m[0] = t*x*x + c;
	ret.m[1] = t*x*y - s*z;
	ret.m[2] = t*x*z + s*y;
	ret.m[3] = 0;
	ret.m[4] = t*x*y + s*z;
	ret.m[5] = t*y*y + c;
	ret.m[6] = t*y*z - s*x;
	ret.m[7] = 0;
	ret.m[8] = t*x*z - s*y;
	ret.m[9] = t*y*z + s*x;
	ret.m[10] = t*z*z + c;
	ret.m[11] = 0;
	ret.m[12] = 0;
	ret.m[13] = 0;
	ret.m[14] = 0;
	ret.m[15] = 1;

	return ret;
}


matrix4f matrix4f::scale(float x, float y, float z)
{
	matrix4f ret;

	ret.m[0] = x;
	ret.m[1] = 0;
	ret.m[2] = 0;
	ret.m[3] = 0;
	ret.m[4] = 0;
	ret.m[5] = y;
	ret.m[6] = 0;
	ret.m[7] = 0;
	ret.m[8] = 0;
	ret.m[9] = 0;
	ret.m[10] = z;
	ret.m[11] = 0;
	ret.m[12] = 0;
	ret.m[13] = 0;
	ret.m[14] = 0;
	ret.m[15] = 1;

	return ret;
}


/*
4x4
|0  1   2  3|
|4  5   6  7|
|8  9  10 11|
|12 13 14 15|
*/

matrix4f matrix4f::translate(float x, float y, float z)
{
	matrix4f ret;

	ret.m[0] = 1;
	ret.m[1] = 0;
	ret.m[2] = 0;
	ret.m[3] = x;
	ret.m[4] = 0;
	ret.m[5] = 1;
	ret.m[6] = 0;
	ret.m[7] = y;
	ret.m[8] = 0;
	ret.m[9] = 0;
	ret.m[10] = 1;
	ret.m[11] = z;
	ret.m[12] = 0;
	ret.m[13] = 0;
	ret.m[14] = 0;
	ret.m[15] = 1;

	return ret;
}


matrix4f matrix4f::shear(float x, float y, float z)
{
	matrix4f ret;

	ret.m[0] = 1;
	ret.m[1] = 0;
	ret.m[2] = 0;
	ret.m[3] = 0;
	ret.m[4] = 0;
	ret.m[5] = 1;
	ret.m[6] = 0;
	ret.m[7] = 0;
	ret.m[8] = 0;
	ret.m[9] = 0;
	ret.m[10] = 1;
	ret.m[11] = 0;
	ret.m[12] = 0;
	ret.m[13] = 0;
	ret.m[14] = 0;
	ret.m[15] = 1;

	return ret;
}

// http://www.cvl.iis.u-tokyo.ac.jp/~miyazaki/tech/teche23.html
float matrix4f::determinant() const
{
	const float a11 = m[0], a12 = m[1], a13 = m[2], a14 = m[3];
	const float a21 = m[4], a22 = m[5], a23 = m[6], a24 = m[7];
	const float a31 = m[8], a32 = m[9], a33 = m[10], a34 = m[11];
	const float a41 = m[12], a42 = m[13], a43 = m[14], a44 = m[15];

	return a11*a22*a33*a44 + a11*a23*a34*a42 + a11*a24*a32*a43
		 + a12*a21*a34*a43 + a12*a23*a31*a44 + a12*a24*a33*a41
		 + a13*a21*a32*a44 + a13*a22*a34*a41 + a13*a24*a31*a42
		 + a14*a21*a33*a42 + a14*a22*a31*a43 + a14*a23*a32*a41
		 - a11*a22*a34*a43 - a11*a23*a32*a44 - a11*a24*a33*a42
		 - a12*a21*a33*a44 - a12*a23*a34*a41 - a12*a24*a31*a43
		 - a13*a21*a34*a42 - a13*a22*a31*a44 - a13*a24*a32*a41
		 - a14*a21*a32*a43 - a14*a22*a33*a41 - a14*a23*a31*a42;
}

// http://www.cvl.iis.u-tokyo.ac.jp/~miyazaki/tech/teche23.html
matrix4f matrix4f::inverse() const
{
	const float det = determinant();
	assert(det != 0);
	const float det_inv = 1.0 / det;

	matrix4f ret;

	const float a11 = m[0], a12 = m[1], a13 = m[2], a14 = m[3];
	const float a21 = m[4], a22 = m[5], a23 = m[6], a24 = m[7];
	const float a31 = m[8], a32 = m[9], a33 = m[10], a34 = m[11];
	const float a41 = m[12], a42 = m[13], a43 = m[14], a44 = m[15];

	ret.m[0] = a22*a33*a44 + a23*a34*a42 + a24*a32*a43 - a22*a34*a43 - a23*a32*a44 - a24*a33*a42;
	ret.m[1] = a12*a34*a43 + a13*a32*a44 + a14*a33*a42 - a12*a33*a44 - a13*a34*a42 - a14*a32*a43;
	ret.m[2] = a12*a23*a44 + a13*a24*a42 + a14*a22*a43 - a12*a24*a43 - a13*a22*a44 - a14*a23*a42;
	ret.m[3] = a12*a24*a33 + a13*a22*a34 + a14*a23*a32 - a12*a23*a34 - a13*a24*a32 - a14*a22*a33;

	ret.m[4] = a21*a34*a43 + a23*a31*a44 + a24*a33*a41 - a21*a33*a44 - a23*a34*a41 - a24*a31*a43;
	ret.m[5] = a11*a33*a44 + a13*a34*a41 + a14*a31*a43 - a11*a34*a43 - a13*a31*a44 - a14*a33*a41;
	ret.m[6] = a11*a24*a43 + a13*a21*a44 + a14*a23*a41 - a11*a23*a44 - a13*a24*a41 - a14*a21*a43;
	ret.m[7] = a11*a23*a34 + a13*a24*a31 + a14*a21*a33 - a11*a24*a33 - a13*a21*a34 - a14*a23*a31;
	
	ret.m[8] = a21*a32*a44 + a22*a34*a41 + a24*a31*a42 - a21*a34*a42 - a22*a31*a44 - a24*a32*a41;
	ret.m[9] = a11*a34*a42 + a12*a31*a44 + a14*a32*a41 - a11*a32*a44 - a12*a34*a41 - a14*a31*a42;
	ret.m[10] = a11*a22*a44 + a12*a24*a41 + a14*a21*a42 - a11*a24*a42 - a12*a21*a44 - a14*a22*a41;
	ret.m[11] = a11*a24*a32 + a12*a21*a34 + a14*a22*a31 - a11*a22*a34 - a12*a24*a31 - a14*a21*a32;
	
	ret.m[12] = a21*a33*a42 + a22*a31*a43 + a23*a32*a41 - a21*a32*a43 - a22*a33*a41 - a23*a31*a42;
	ret.m[13] = a11*a32*a43 + a12*a33*a41 + a13*a31*a42 - a11*a33*a42 - a12*a31*a43 - a13*a32*a41;
	ret.m[14] = a11*a23*a42 + a12*a21*a43 + a13*a22*a41 - a11*a22*a43 - a12*a23*a41 - a13*a21*a42;
	ret.m[15] = a11*a22*a33 + a12*a23*a31 + a13*a21*a32 - a11*a23*a32 - a12*a21*a33 - a13*a22*a31;

	for (int i = 0; i < 16; i++)
		ret.m[i] *= det_inv;

	return ret;
}

matrix4f matrix4f::transpose() const
{
	matrix4f ret;

	ret.m[0] = m[0];
	ret.m[5] = m[5];
	ret.m[10] = m[10];
	ret.m[15] = m[15];

	ret.m[1] = m[4];
	ret.m[6] = m[9];
	ret.m[11] = m[14];

	ret.m[2] = m[8];
	ret.m[7] = m[13];

	ret.m[3] = m[12];

	ret.m[4] = m[1];
	ret.m[9] = m[6];
	ret.m[14] = m[11];

	ret.m[8] = m[2];
	ret.m[13] = m[7];

	ret.m[12] = m[3];

	return ret;
}


matrix4f matrix4f::mult(const matrix4f& mat) const
{
	matrix4f ret;

	ret.m[0] = mat.m[0] * m[0] + mat.m[1] * m[4] + mat.m[2] * m[8] + mat.m[3] * m[12];
	ret.m[1] = mat.m[0] * m[1] + mat.m[1] * m[5] + mat.m[2] * m[9] + mat.m[3] * m[13];
	ret.m[2] = mat.m[0] * m[2] + mat.m[1] * m[6] + mat.m[2] * m[10] + mat.m[3] * m[14];
	ret.m[3] = mat.m[0] * m[3] + mat.m[1] * m[7] + mat.m[2] * m[11] + mat.m[3] * m[15];

	ret.m[4] = mat.m[4] * m[0] + mat.m[5] * m[4] + mat.m[6] * m[8] + mat.m[7] * m[12];
	ret.m[5] = mat.m[4] * m[1] + mat.m[5] * m[5] + mat.m[6] * m[9] + mat.m[7] * m[13];
	ret.m[6] = mat.m[4] * m[2] + mat.m[5] * m[6] + mat.m[6] * m[10] + mat.m[7] * m[14];
	ret.m[7] = mat.m[4] * m[3] + mat.m[5] * m[7] + mat.m[6] * m[11] + mat.m[7] * m[15];

	ret.m[8] = mat.m[8] * m[0] + mat.m[9] * m[4] + mat.m[10] * m[8] + mat.m[11] * m[12];
	ret.m[9] = mat.m[8] * m[1] + mat.m[9] * m[5] + mat.m[10] * m[9] + mat.m[11] * m[13];
	ret.m[10] = mat.m[8] * m[2] + mat.m[9] * m[6] + mat.m[10] * m[10] + mat.m[11] * m[14];
	ret.m[11] = mat.m[8] * m[3] + mat.m[9] * m[7] + mat.m[10] * m[11] + mat.m[11] * m[15];

	ret.m[12] = mat.m[12] * m[0] + mat.m[13] * m[4] + mat.m[14] * m[8] + mat.m[15] * m[12];
	ret.m[13] = mat.m[12] * m[1] + mat.m[13] * m[5] + mat.m[14] * m[9] + mat.m[15] * m[13];
	ret.m[14] = mat.m[12] * m[2] + mat.m[13] * m[6] + mat.m[14] * m[10] + mat.m[15] * m[14];
	ret.m[15] = mat.m[12] * m[3] + mat.m[13] * m[7] + mat.m[14] * m[11] + mat.m[15] * m[15];

	return ret;
}


vect4f matrix4f::mult(const vect4f& src) const
{
	vect4f ret;

	ret.v[0] = m[0] * src.v[0] + m[1] * src.v[1] + m[2] * src.v[2] + m[3] * src.v[3];
	ret.v[1] = m[4] * src.v[0] + m[5] * src.v[1] + m[6] * src.v[2] + m[7] * src.v[3];
	ret.v[2] = m[8] * src.v[0] + m[9] * src.v[1] + m[10] * src.v[2] + m[11] * src.v[3];
	ret.v[3] = m[12] * src.v[0] + m[13] * src.v[1] + m[14] * src.v[2] + m[15] * src.v[3];

	return ret;
}


vect3d matrix4f::mult_pos(const vect3d& src) const
{
	vect3d ret;

	float div = 1.0 / (m[12] * src.v[0] + m[13] * src.v[1] + m[14] * src.v[2] + m[15]);

	ret.v[0] = (m[0] * src.v[0] + m[1] * src.v[1] + m[2] * src.v[2] + m[3]) * div;
	ret.v[1] = (m[4] * src.v[0] + m[5] * src.v[1] + m[6] * src.v[2] + m[7]) * div;
	ret.v[2] = (m[8] * src.v[0] + m[9] * src.v[1] + m[10] * src.v[2] + m[11]) * div;

	return ret;
}

vect3d matrix4f::mult_vec(const vect3d& src) const
{
	vect3d ret;

	ret.v[0] = m[0] * src.v[0] + m[1] * src.v[1] + m[2] * src.v[2];
	ret.v[1] = m[4] * src.v[0] + m[5] * src.v[1] + m[6] * src.v[2];
	ret.v[2] = m[8] * src.v[0] + m[9] * src.v[1] + m[10] * src.v[2];

	return ret;
}