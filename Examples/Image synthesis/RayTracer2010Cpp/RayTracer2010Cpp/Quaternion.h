#ifndef QUATERNION_H
#define QUATERNION_H

#include "vect.h"
#include "matrix.h"

/**
 * Quaternion class for doing euler angle independent rotations.
 *  Only unit quaternions should
 * be constructed.  A quaternion is of the form <s,x,y,z>.
 * Modified somewhat by Josiah Manson.
 *
 * @author Scott Schaefer
 */

struct Quaternion
{
	double s;
	vect3d v;

	/**
	 * Constructor.  Makes an identity quaternion
	 */
	Quaternion ( void );

	/**
	 * Constructor.  Makes a quaternion with the specified components
	 */
	Quaternion ( double s, double x, double y, double z );

	double &operator [] ( int ind )
	{
		assert ( ind >= 0 && ind < 4 );
		return (&s) [ ind ];
	}

	/**
	 * @return an identity quaternion corresponding to no rotation
	 */
	static Quaternion makeIdentity ( void );

	/**
	 * Makes a quaterion representing a rotation of an angle about a vector.
	 * Note that this must be a unit vector!
	 *
	 * @param angle the angle in radians to rotate
	 * @param x the x component of the vector to rotate about
	 * @param y the y component of the vector to rotate about
	 * @param z the z component of the vector to rotate about
	 */
	static Quaternion makeRotation ( double angle, double x, double y, double z );
	static Quaternion makeRotation ( double angle, vect3d axis ){return makeRotation(angle, axis[0], axis[1], axis[2]);}

	/** 
	 * Takes a rotation matrix (no translations or scalings) and returns
	 * a quaterion representing that rotation matrix
	 *
	 * @param mat the rotation matrix to convert to a quaternion
	 */
	static Quaternion mat2Quat ( matrix4f &mat );
	static Quaternion mat2Quat ( vect3d &x, vect3d &y, vect3d &z );

	/**
	 * Performs spherical linear interpolation between two quaternions.  If the
	 * quaternions are separated by 1 degree or less, then linear interpolation
	 * is used instead because it is more numerically stable for small angles.
	 *
	 * @param q1 the first quaternion to interpolate between
	 * @param q2 the second quaternion to interpolate between
	 * @param alpha the amount to rotate so far
	 *
	 * @return a quaternion representing the interpolated rotation
	 */
	static Quaternion slerp ( Quaternion &q1, Quaternion &q2, double t );

	/**
	 * Performs linear interpolation between two quaternions.  This is faster 
	 * than slerp, but worse in that the rotation speeds up as it approaches
	 * the half angle between the two vectors.
	 *
	 * @param q1 the first quaternion to interpolate between
	 * @param q2 the second quaternion to interpolate between
	 * @param alpha the amount to rotate so far
	 *
	 * @return a quaternion representing the interpolated rotation
	 */
	static Quaternion lerp ( Quaternion &q1, Quaternion &q2, double t );

	/**
	 * Equals operator
	 */
	void operator = ( Quaternion q );

	/**
	 * Multiplication operator.  Note that quaternion multiplication 
	 * is NOT communative.
	 */
	Quaternion operator * ( const Quaternion &q );

	/**
	 * Times Equal operator
	 */
	Quaternion& operator *= ( const Quaternion &q );

	/** 
	 * @return the magnitude squared of this quaternion
	 */
	double magnitudeSq ( void );

	void normalize ( void );

	/** 
	 * @return the conjugate of this UNIT quaternion.  In the case
	 * of unit quaternions conjugate(<s,v>) is <s,-v>
	 */
	Quaternion conjugate ( void );

	/**
	 * Transforms a point by the rotation represented by this quaternion
	 *
	 * @param p the point to transform
	 * @param rvalue where to put the transformed point
	 */
	void transformPoint ( double p[], double rvalue[] );
	vect3d transformPoint ( vect3d p ){vect3d r; transformPoint(p.v, r.v); return r;}

	/**
	 * Transforms this quaternion into a rotation matrix
	 *
	 * @param mat the matrix to store the rotation in
	 */
	void quat2Matrix ( matrix4f &mat );

	/**
	 * Prints this quaternion
	 */
	void print ( void );

	/**
	 * @return the "s" component of this quaternion
	 */
	double getS ( void );

	/**
	 * @return the "x" component of this quaternion
	 */
	double getX ( void );

	/**
	 * @return the "y" component of this quaternion
	 */
	double getY ( void );

	/**
	 * @return the "z" component of this quaternion
	 */
	double getZ ( void );
};

#endif