using System;
using System.Collections.Generic;
using System.Text;

namespace Custom.Math
{
    public class matrix4d
    {
        public double[] m;

        /// <summary>
        /// Creates an identity matrix by default.
        /// </summary>
        public matrix4d()
        {
            m = new double[16];
            m[0] = m[5] = m[10] = m[15] = 1;
        }
        public matrix4d(double diag)
        {
            m = new double[16];
            m[0] = m[5] = m[10] = m[15] = diag;
        }
        public matrix4d(matrix4d a)
        {
            m = new double[16];
            for (int i = 0; i < 16; i++)
                m[i] = a.m[i];
        }

        public static matrix4d rotaxis(ref vect3d axis, double rad)
        {
            double c = System.Math.Cos(rad);
            double s = System.Math.Sin(rad);
            double t = 1 - c;

            double x = axis.x;
            double y = axis.y;
            double z = axis.z;

            matrix4d ret = new matrix4d();

            ret.m[0] = t * x * x + c;
            ret.m[1] = t * x * y - s * z;
            ret.m[2] = t * x * z + s * y;

            ret.m[4] = t * x * y + s * z;
            ret.m[5] = t * y * y + c;
            ret.m[6] = t * y * z - s * x;

            ret.m[8] = t * x * z - s * y;
            ret.m[9] = t * y * z + s * x;
            ret.m[10] = t * z * z + c;

            return ret;
        }

        public static matrix4d scale(double x, double y, double z)
        {
            matrix4d ret = new matrix4d();

            ret.m[0] = x;
            ret.m[5] = y;
            ret.m[10] = z;

            return ret;
        }
        public static matrix4d scale(ref vect3d s)
        {
            matrix4d ret = new matrix4d();

            ret.m[0] = s.x;
            ret.m[5] = s.y;
            ret.m[10] = s.z;

            return ret;
        }
        public static matrix4d scale(double s)
        {
            matrix4d ret = new matrix4d();

            ret.m[0] = s;
            ret.m[5] = s;
            ret.m[10] = s;

            return ret;
        }


        matrix4d translate(double x, double y, double z)
        {
            matrix4d ret = new matrix4d();

            ret.m[3] = x;
            ret.m[7] = y;
            ret.m[11] = z;

            return ret;
        }
        matrix4d translate(ref vect3d t)
        {
            matrix4d ret = new matrix4d();

            ret.m[3] = t.x;
            ret.m[7] = t.y;
            ret.m[11] = t.z;

            return ret;
        }

        public double determinant()
        {
            return m[0] * m[5] * m[10] * m[15] + m[0] * m[6] * m[11] * m[13] + m[0] * m[7] * m[9] * m[14]
                 + m[1] * m[4] * m[11] * m[14] + m[1] * m[6] * m[8] * m[15] + m[1] * m[7] * m[10] * m[12]
                 + m[2] * m[4] * m[9] * m[15] + m[2] * m[5] * m[11] * m[12] + m[2] * m[7] * m[8] * m[13]
                 + m[3] * m[4] * m[10] * m[13] + m[3] * m[5] * m[8] * m[14] + m[3] * m[6] * m[9] * m[12]
                 - m[0] * m[5] * m[11] * m[14] - m[0] * m[6] * m[9] * m[15] - m[0] * m[7] * m[10] * m[13]
                 - m[1] * m[4] * m[10] * m[15] - m[1] * m[6] * m[11] * m[12] - m[1] * m[7] * m[8] * m[14]
                 - m[2] * m[4] * m[11] * m[13] - m[2] * m[5] * m[8] * m[15] - m[2] * m[7] * m[9] * m[12]
                 - m[3] * m[4] * m[9] * m[14] - m[3] * m[5] * m[10] * m[12] - m[3] * m[6] * m[8] * m[13];
        }

        public matrix4d inverse()
        {
            matrix4d ret = new matrix4d();

            ret.m[0] = m[5] * m[10] * m[15] + m[6] * m[11] * m[13] + m[7] * m[9] * m[14] - m[5] * m[11] * m[14] - m[6] * m[9] * m[15] - m[7] * m[10] * m[13];
            ret.m[1] = m[1] * m[11] * m[14] + m[2] * m[9] * m[15] + m[3] * m[10] * m[13] - m[1] * m[10] * m[15] - m[2] * m[11] * m[13] - m[3] * m[9] * m[14];
            ret.m[2] = m[1] * m[6] * m[15] + m[2] * m[7] * m[13] + m[3] * m[5] * m[14] - m[1] * m[7] * m[14] - m[2] * m[5] * m[15] - m[3] * m[6] * m[13];
            ret.m[3] = m[1] * m[7] * m[10] + m[2] * m[5] * m[11] + m[3] * m[6] * m[9] - m[1] * m[6] * m[11] - m[2] * m[7] * m[9] - m[3] * m[5] * m[10];

            ret.m[4] = m[4] * m[11] * m[14] + m[6] * m[8] * m[15] + m[7] * m[10] * m[12] - m[4] * m[10] * m[15] - m[6] * m[11] * m[12] - m[7] * m[8] * m[14];
            ret.m[5] = m[0] * m[10] * m[15] + m[2] * m[11] * m[12] + m[3] * m[8] * m[14] - m[0] * m[11] * m[14] - m[2] * m[8] * m[15] - m[3] * m[10] * m[12];
            ret.m[6] = m[0] * m[7] * m[14] + m[2] * m[4] * m[15] + m[3] * m[6] * m[12] - m[0] * m[6] * m[15] - m[2] * m[7] * m[12] - m[3] * m[4] * m[14];
            ret.m[7] = m[0] * m[6] * m[11] + m[2] * m[7] * m[8] + m[3] * m[4] * m[10] - m[0] * m[7] * m[10] - m[2] * m[4] * m[11] - m[3] * m[6] * m[8];

            ret.m[8] = m[4] * m[9] * m[15] + m[5] * m[11] * m[12] + m[7] * m[8] * m[13] - m[4] * m[11] * m[13] - m[5] * m[8] * m[15] - m[7] * m[9] * m[12];
            ret.m[9] = m[0] * m[11] * m[13] + m[1] * m[8] * m[15] + m[3] * m[9] * m[12] - m[0] * m[9] * m[15] - m[1] * m[11] * m[12] - m[3] * m[8] * m[13];
            ret.m[10] = m[0] * m[5] * m[15] + m[1] * m[7] * m[12] + m[3] * m[4] * m[13] - m[0] * m[7] * m[13] - m[1] * m[4] * m[15] - m[3] * m[5] * m[12];
            ret.m[11] = m[0] * m[7] * m[9] + m[1] * m[4] * m[11] + m[3] * m[5] * m[8] - m[0] * m[5] * m[11] - m[1] * m[7] * m[8] - m[3] * m[4] * m[9];

            ret.m[12] = m[4] * m[10] * m[13] + m[5] * m[8] * m[14] + m[6] * m[9] * m[12] - m[4] * m[9] * m[14] - m[5] * m[10] * m[12] - m[6] * m[8] * m[13];
            ret.m[13] = m[0] * m[9] * m[14] + m[1] * m[10] * m[12] + m[2] * m[8] * m[13] - m[0] * m[10] * m[13] - m[1] * m[8] * m[14] - m[2] * m[9] * m[12];
            ret.m[14] = m[0] * m[6] * m[13] + m[1] * m[4] * m[14] + m[2] * m[5] * m[12] - m[0] * m[5] * m[14] - m[1] * m[6] * m[12] - m[2] * m[4] * m[13];
            ret.m[15] = m[0] * m[5] * m[10] + m[1] * m[6] * m[8] + m[2] * m[4] * m[9] - m[0] * m[6] * m[9] - m[1] * m[4] * m[10] - m[2] * m[5] * m[8];

            double det_inv = 1.0 / determinant();
            for (int i = 0; i < 16; i++)
                ret.m[i] *= det_inv;

            return ret;
        }


        public matrix4d transpose()
        {
            matrix4d ret = new matrix4d();

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

        public matrix4d mult(matrix4d mat)
        {
            matrix4d ret = new matrix4d();

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


        public vect3d mult_pos(ref vect3d src)
        {
            vect3d ret = new vect3d();

            double div = 1.0 / (m[12] * src.x + m[13] * src.y + m[14] * src.z + m[15]);

            ret.x = (m[0] * src.x + m[1] * src.y + m[2] * src.z + m[3]) * div;
            ret.y = (m[4] * src.x + m[5] * src.y + m[6] * src.z + m[7]) * div;
            ret.z = (m[8] * src.x + m[9] * src.y + m[10] * src.z + m[11]) * div;

            return ret;
        }

        public vect3d mult_vec(ref vect3d src)
        {
            vect3d ret = new vect3d();

            ret.x = m[0] * src.x + m[1] * src.y + m[2] * src.z;
            ret.y = m[4] * src.x + m[5] * src.y + m[6] * src.z;
            ret.z = m[8] * src.x + m[9] * src.y + m[10] * src.z;

            return ret;
        }

        public static matrix4d operator *(matrix4d a, matrix4d b)
        {
            return a.mult(b);
        }
    }
}
