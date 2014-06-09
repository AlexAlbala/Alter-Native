using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Custom.Math
{
    public struct quaternion
    {
        public double s;
        public vect3d v;

        public quaternion(double s, double x, double y, double z)
        {
            this.s = s;
            v = new vect3d(x, y, z);
        }
        public quaternion(double s, vect3d v)
        {
            this.s = s;
            this.v = v;
        }

        public void save(BinaryWriter f)
        {
            f.Write(s);
            v.save(f);
        }
        public void load(BinaryReader f)
        {
            s = f.ReadDouble();
            v.load(f);
        }

        public override string ToString()
        {
            return "(" + s + ", " + v.x + ", " + v.y + ", " + v.z + ")";
        }

        public static quaternion makeIdentity()
        {
            return new quaternion(1, 0, 0, 0);
        }

        /// <summary>
        /// Makes a quaterion representing a rotation of an angle about a vector. Note that this must be a unit vector!
        /// </summary>
        /// <returns></returns>
        public static quaternion makeRotation(double angle, double x, double y, double z)
        {
            double sine = System.Math.Sin(angle / 2);
            return new quaternion(System.Math.Cos(angle / 2), x * sine, y * sine, z * sine);
        }

        /// <summary>
        /// Makes a quaterion representing a rotation of an angle about a vector. Note that this must be a unit vector!
        /// </summary>
        /// <returns></returns>
        public static quaternion makeRotation(double angle, vect3d axis)
        {
            return makeRotation(angle, axis.x, axis.y, axis.z);
        }

        public static quaternion operator *(quaternion a, quaternion b)
        {
            quaternion r = new quaternion();

            r.s = a.s * b.s - (a.v * b.v);
            r.v = (a.s * b.v) + (a.v * b.s) + (a.v % b.v);

            return r;
        }

        public double magnitudeSq()
        {
            return s * s + v.length2();
        }

        public quaternion conjugate()
        {
            return new quaternion(s, -v);
        }

        public vect3d transformPoint(ref vect3d p)
        {
            vect3d vecTemp = p;
            double mag = -(vecTemp * v);

            vecTemp = ((vecTemp * s) + ((v % vecTemp)));
            quaternion temp = new quaternion(mag, vecTemp);

            return (temp * conjugate()).v;
        }

        public void normalize()
        {
            double mag = 1.0 / System.Math.Sqrt(magnitudeSq());

            s *= mag;
            v *= mag;
        }

        quaternion slerp(ref quaternion q1, ref quaternion q2, double t)
        {
            double[] temp = new double[4];
            double omega, cosom, sinom, scale0, scale1;
            quaternion rvalue;

            // calc cosine
            cosom = q1.s * q2.s + q1.v.x * q2.v.x + q1.v.y * q2.v.y + q1.v.z * q2.v.z;

            // adjust signs (if necessary)
            if (cosom < 0.0)
            {
                cosom = -1.0 * cosom;
                temp[0] = -1.0 * q2.s;
                temp[1] = -1.0 * q2.v.x;
                temp[2] = -1.0 * q2.v.y;
                temp[3] = -1.0 * q2.v.z;
            }
            else
            {
                temp[0] = q2.s;
                temp[1] = q2.v.x;
                temp[2] = q2.v.y;
                temp[3] = q2.v.z;
            }

            // calculate coefficients
            if ((1.0 - cosom) > 0.001)
            {
                // standard case (slerp)
                omega = System.Math.Acos(cosom);
                sinom = System.Math.Sin(omega);
                scale0 = System.Math.Sin((1.0 - t) * omega) / sinom;
                scale1 = System.Math.Sin(t * omega) / sinom;
            }
            else
            {
                // q1 and q2 are about 1 degree apart so use linear
                // interpolation to avoid division by very small numbers
                scale0 = 1.0 - t;
                scale1 = t;
            }

            // do the interpolation
            rvalue.s = scale0 * q1.s + scale1 * temp[0];
            rvalue.v.x = scale0 * q1.v.x + scale1 * temp[1];
            rvalue.v.y = scale0 * q1.v.y + scale1 * temp[2];
            rvalue.v.z = scale0 * q1.v.z + scale1 * temp[3];

            return rvalue;
        }

        public matrix4d getMatrix()
        {
            double wx, wy, wz, xx, xy, xz, yy, yz, zz;

            wx = 2 * v.x * s;
            wy = 2 * v.y * s;
            wz = 2 * v.z * s;
            xx = 2 * v.x * v.x;
            xy = 2 * v.x * v.y;
            xz = 2 * v.x * v.z;
            yy = 2 * v.y * v.y;
            yz = 2 * v.y * v.z;
            zz = 2 * v.z * v.z;

            matrix4d mat = new matrix4d();

            mat.m[0 * 4 + 0] = 1.0 - (yy + zz);
            mat.m[0 * 4 + 1] = xy - wz;
            mat.m[0 * 4 + 2] = xz + wy;

            mat.m[1 * 4 + 0] = xy + wz;
            mat.m[1 * 4 + 1] = 1.0 - (xx + zz);
            mat.m[1 * 4 + 2] = yz - wx;

            mat.m[2 * 4 + 0] = xz - wy;
            mat.m[2 * 4 + 1] = yz + wx;
            mat.m[2 * 4 + 2] = 1.0 - (xx + yy);

            return mat;
        }
        public void setMatrix(matrix4d mat)
        {
            double tr, s;
            int i = 0;
            double max;

            tr = mat.m[0 * 4 + 0] + mat.m[1 * 4 + 1] + mat.m[2 * 4 + 2];

            max = tr;
            if (mat.m[0 * 4 + 0] > max)
            {
                max = mat.m[0 * 4 + 0];
                i = 1;
            }
            if (mat.m[1 * 4 + 1] > max)
            {
                max = mat.m[1 * 4 + 1];
                i = 2;
            }
            if (mat.m[2 * 4 + 2] > max)
            {
                i = 3;
            }

            switch (i)
            {
                case 0:
                    s = 0.5 * System.Math.Sqrt(tr + 1);
                    s = 1.0 / (4.0 * s);

                    v.x = (mat.m[2 * 4 + 1] - mat.m[1 * 4 + 2]) * s;
                    v.y = (mat.m[0 * 4 + 2] - mat.m[2 * 4 + 0]) * s;
                    v.z = (mat.m[1 * 4 + 0] - mat.m[0 * 4 + 1]) * s;
                    break;
                case 1:
                    v.x = .5 * System.Math.Sqrt(2 * mat.m[0 * 4 + 0] - tr + 1);
                    s = 1.0 / (4.0 * v.x);

                    s = (mat.m[2 * 4 + 1] - mat.m[1 * 4 + 2]) * s;
                    v.y = (mat.m[1 * 4 + 0] + mat.m[0 * 4 + 1]) * s;
                    v.z = (mat.m[0 * 4 + 2] + mat.m[2 * 4 + 0]) * s;
                    break;
                case 2:
                    v.y = .5 * System.Math.Sqrt(2 * mat.m[1 * 4 + 1] - tr + 1);
                    s = 1.0 / (4.0 * v.y);

                    s = (mat.m[0 * 4 + 2] - mat.m[2 * 4 + 0]) * s;
                    v.x = (mat.m[1 * 4 + 0] + mat.m[0 * 4 + 1]) * s;
                    v.z = (mat.m[2 * 4 + 1] + mat.m[1 * 4 + 2]) * s;
                    break;
                case 3:
                    v.z = .5 * System.Math.Sqrt(2 * mat.m[2 * 4 + 2] - tr + 1);
                    s = 1.0 / (4.0 * v.z);

                    s = (mat.m[1 * 4 + 0] - mat.m[0 * 4 + 1]) * s;
                    v.x = (mat.m[0 * 4 + 2] + mat.m[2 * 4 + 0]) * s;
                    v.y = (mat.m[2 * 4 + 1] + mat.m[1 * 4 + 2]) * s;
                    break;
            }
        }
    }
}
