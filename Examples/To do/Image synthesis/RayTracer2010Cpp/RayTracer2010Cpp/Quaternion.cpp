#include "Quaternion.h"
//#include "MathLib.h"

Quaternion::Quaternion ( void )
{
	s = 1;
	v(0,0,0);
}

Quaternion::Quaternion ( double s, double x, double y, double z )
{
	this->s = s;

	v(x,y,z);
}

Quaternion Quaternion::makeIdentity ( void )
{
	return Quaternion ( );
}

Quaternion Quaternion::makeRotation ( double angle, double x, double y, double z )
{
	Quaternion q;
	double sine = (double)sin ( angle / 2 );

	q.s = (double)cos ( angle / 2 );
	q.v(x * sine, y * sine, z * sine);

	return q;
}

void Quaternion::operator = ( Quaternion q )
{
	s = q.s;
	v = q.v;
}

Quaternion Quaternion::operator * ( const Quaternion &q )
{
	Quaternion rvalue;

	rvalue.s = s * q.s - ( q.v * v );
	rvalue.v = ( q.v * s ) + ( v * q.s ) +  ( v % q.v );

	return rvalue;
}

Quaternion& Quaternion::operator *= ( const Quaternion &q )
{
	double newS;

	newS = q.s * s - ( q.v * v );
	v = ( q.v * s ) + ( v * q.s ) + ( v % q.v );

	s = newS;

	return *this;
}

double Quaternion::magnitudeSq ( void )
{
	return s * s + v.length2( );
}

Quaternion Quaternion::conjugate ( void )
{
	return Quaternion ( s, -v[0], -v[1], -v[2] );
}

void Quaternion::transformPoint ( double p[], double rvalue[] )
{
	vect3d vecTemp ( p [ 0 ], p [ 1 ], p [ 2 ] );
	double mag = -( vecTemp * v );

	vecTemp = ( ( vecTemp * s ) + ( ( v % vecTemp ) ) );
	Quaternion temp ( mag, vecTemp[0], vecTemp[1], vecTemp[2] );

	temp *= conjugate ( );

	rvalue [ 0 ] = temp.v[0];
	rvalue [ 1 ] = temp.v[1];
	rvalue [ 2 ] = temp.v[2];
}

void Quaternion::quat2Matrix ( matrix4f &mat )
{
	double wx, wy, wz, xx, xy, xz, yy, yz, zz;
	
	wx = 2 * v[0] * s;
	wy = 2 * v[1] * s;
	wz = 2 * v[2] * s;
	xx = 2 * v[0] * v[0];
	xy = 2 * v[0] * v[1];
	xz = 2 * v[0] * v[2];
	yy = 2 * v[1] * v[1];
	yz = 2 * v[1] * v[2];
	zz = 2 * v[2] * v[2];

	mat.m [ 0 *4+ 0 ] = 1.0f - ( yy + zz );
	mat.m [ 0 *4+ 1 ] = xy - wz;
	mat.m [ 0 *4+ 2 ] = xz + wy;
	mat.m [ 0 *4+ 3 ] = 0.0f;

	mat.m [ 1 *4+ 0 ] = xy + wz;
	mat.m [ 1 *4+ 1 ] = 1.0f - ( xx + zz );
	mat.m [ 1 *4+ 2 ] = yz - wx;
	mat.m [ 1 *4+ 3 ] = 0.0f;

	mat.m [ 2 *4+ 0 ] = xz - wy;
	mat.m [ 2 *4+ 1 ] = yz + wx;
	mat.m [ 2 *4+ 2 ] = 1.0f - ( xx + yy );
	mat.m [ 2 *4+ 3 ] = 0.0f;

	mat.m [ 3 *4+ 0 ] = 0.0f;
	mat.m [ 3 *4+ 1 ] = 0.0f;
	mat.m [ 3 *4+ 2 ] = 0.0f;
	mat.m [ 3 *4+ 3 ] = 1.0f;
}

Quaternion Quaternion::mat2Quat ( vect3d &x, vect3d &y, vect3d &z )
{
	matrix4f mat;
	mat.m[0] = x[0];
	mat.m[1] = x[1];
	mat.m[2] = x[2];
	mat.m[3] = 0;
	mat.m[4] = y[0];
	mat.m[5] = y[1];
	mat.m[6] = y[2];
	mat.m[7] = 0;
	mat.m[8] = z[0];
	mat.m[9] = z[1];
	mat.m[10] = z[2];
	mat.m[11] = 0;
	mat.m[12] = 0;
	mat.m[13] = 0;
	mat.m[14] = 0;
	mat.m[15] = 1;

	return mat2Quat(mat.transpose());
}

Quaternion Quaternion::mat2Quat ( matrix4f &mat )
{
	double tr, s;
	Quaternion q;
	int i = 0;
	double max;

	tr = mat.m [ 0 *4+ 0 ] + mat.m [ 1 *4+ 1 ] + mat.m [ 2 *4+ 2 ];

	max = tr;
	if ( mat.m [ 0 *4+ 0 ] > max )
	{
		max = mat.m [ 0 *4+ 0 ];
		i = 1;
	}
	if ( mat.m [ 1 *4+ 1 ] > max )
	{
		max = mat.m [ 1 *4+ 1 ];
		i = 2;
	}
	if ( mat.m [ 2 *4+ 2 ] > max )
	{
		i = 3;
	}

	switch ( i )
	{
	case 0:
		q.s = 0.5f * (double)sqrt ( tr + 1 );
		s = 1.0f / ( 4.0f * q.s );

		q.v[0] = ( mat.m [ 2 *4+ 1 ] - mat.m [ 1 *4+ 2 ] ) * s;
		q.v[1] = ( mat.m [ 0 *4+ 2 ] - mat.m [ 2 *4+ 0 ] ) * s;
		q.v[2] = ( mat.m [ 1 *4+ 0 ] - mat.m [ 0 *4+ 1 ] ) * s;
		break;
	case 1:
		q.v[0] = .5f * (double)sqrt ( 2 * mat.m [ 0 *4+ 0 ] - tr + 1 );
		s = 1.0f / ( 4.0f * q.v[0] );

		q.s = ( mat.m [ 2 *4+ 1 ] - mat.m [ 1 *4+ 2 ] ) * s;
		q.v[1] = ( mat.m [ 1 *4+ 0 ] + mat.m [ 0 *4+ 1 ] ) * s;
		q.v[2] = ( mat.m [ 0 *4+ 2 ] + mat.m [ 2 *4+ 0 ] ) * s;
		break;
	case 2:
		q.v[1] = .5f * (double)sqrt ( 2 * mat.m [ 1 *4+ 1 ] - tr + 1 );
		s = 1.0f / ( 4.0f * q.v[1] );

		q.s = ( mat.m [ 0 *4+ 2 ] - mat.m [ 2 *4+ 0 ] ) * s;
		q.v[0] = ( mat.m [ 1 *4+ 0 ] + mat.m [ 0 *4+ 1 ] ) * s;
		q.v[2] = ( mat.m [ 2 *4+ 1 ] + mat.m [ 1 *4+ 2 ] ) * s;
		break;
	case 3:
		q.v[2] = .5f * (double)sqrt ( 2 * mat.m [ 2 *4+ 2 ] - tr + 1 );
		s = 1.0f / ( 4.0f * q.v[2] );

		q.s = ( mat.m [ 1 *4+ 0 ] - mat.m [ 0 *4+ 1 ] ) * s;
		q.v[0] = ( mat.m [ 0 *4+ 2 ] + mat.m [ 2 *4+ 0 ] ) * s;
		q.v[1] = ( mat.m [ 2 *4+ 1 ] + mat.m [ 1 *4+ 2 ] ) * s;
		break;
	}	 

	return q;
}

Quaternion Quaternion::slerp ( Quaternion &q1, Quaternion &q2, double t )
{
	double temp [ 4 ];
	double omega, cosom, sinom, scale0, scale1;
	Quaternion rvalue;

	// calc cosine
	cosom = q1.s * q2.s + q1.v[0] * q2.v[0] + q1.v[1] * q2.v[1] + q1.v[2] * q2.v[2];


	// adjust signs (if necessary)
	if ( cosom < 0.0f )
	{ 
		cosom = -1.0f * cosom; 
		temp [ 0 ] = -1.0f * q2.s;
		temp [ 1 ] = -1.0f * q2.v[0];
		temp [ 2 ] = -1.0f * q2.v[1];
		temp [ 3 ] = -1.0f * q2.v[2];
	} 
	else  
	{
		temp [ 0 ] = q2.s;
		temp [ 1 ] = q2.v[0];
		temp [ 2 ] = q2.v[1];
		temp [ 3 ] = q2.v[2];
	}


	// calculate coefficients
	if ( ( 1.0f - cosom) > 0.001f ) 
	{
		// standard case (slerp)
		omega = (double)acos ( cosom );
		sinom = (double)sin ( omega );
		scale0 = (double)sin ( ( 1.0f - t ) * omega ) / sinom;
		scale1 = (double)sin ( t * omega ) / sinom;
	} 
	else 
	{        
		// q1 and q2 are about 1 degree apart so use linear
		// interpolation to avoid division by very small numbers
		scale0 = 1.0f - t;
		scale1 = t;
	}

	// do the interpolation
	rvalue.s = scale0 * q1.s + scale1 * temp [ 0 ];
	rvalue.v[0] = scale0 * q1.v[0] + scale1 * temp [ 1 ];
	rvalue.v[1] = scale0 * q1.v[1] + scale1 * temp [ 2 ];
	rvalue.v[2] = scale0 * q1.v[2] + scale1 * temp [ 3 ];

	return rvalue;
}

Quaternion Quaternion::lerp ( Quaternion &q1, Quaternion &q2, double t )
{
	double temp [ 4 ];
	double cosom, scale0, scale1;
	Quaternion rvalue;

	// calc cosine
	cosom = q1.s * q2.s + q1.v[0] * q2.v[0] + q1.v[1] * q2.v[1] + q1.v[2] * q2.v[2];


	// adjust signs (if necessary)
	if ( cosom < 0.0f )
	{ 
		cosom = -1.0f * cosom; 
		temp [ 0 ] = -1.0f * q2.s;
		temp [ 1 ] = -1.0f * q2.v[0];
		temp [ 2 ] = -1.0f * q2.v[1];
		temp [ 3 ] = -1.0f * q2.v[2];
	} 
	else  
	{
		temp [ 0 ] = q2.s;
		temp [ 1 ] = q2.v[0];
		temp [ 2 ] = q2.v[1];
		temp [ 3 ] = q2.v[2];
	}


	// q1 and q2 are about 1 degree apart so use linear
	// interpolation to avoid division by very small numbers
	scale0 = 1.0f - t;
	scale1 = t;

	// do the interpolation
	rvalue.s = scale0 * q1.s + scale1 * temp [ 0 ];
	rvalue.v[0] = scale0 * q1.v[0] + scale1 * temp [ 1 ];
	rvalue.v[1] = scale0 * q1.v[1] + scale1 * temp [ 2 ];
	rvalue.v[2] = scale0 * q1.v[2] + scale1 * temp [ 3 ];

	return rvalue;
}

void Quaternion::print ( void )
{
	printf ( "<%f, %f, %f, %f>\n", s, v[0], v[1], v[2] );
}

double Quaternion::getS ( void )
{
	return s;
}

double Quaternion::getX ( void )
{
	return v[0];
}

double Quaternion::getY ( void )
{
	return v[1];
}

double Quaternion::getZ ( void )
{
	return v[2];
}

void Quaternion::normalize ( void )
{
	double mag = magnitudeSq ( );

	mag = (double)sqrt ( mag );

	s /= mag;
	v /= mag;
}