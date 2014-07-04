using System;
using System.Collections.Generic;
using System.Text;

namespace USAL
{
    public class Matrix3
    {
        public double m00, m01, m02;
        public double m10, m11, m12;
        public double m20, m21, m22;

        #region Constructors
        
        public Matrix3()
        {
        }

        public Matrix3(double m00, double m01, double m02,
                       double m10, double m11, double m12,
                       double m20, double m21, double m22)
        {
            this.m00 = m00; this.m01 = m01; this.m02 = m02;
            this.m10 = m10; this.m11 = m11; this.m12 = m12;
            this.m20 = m20; this.m21 = m21; this.m22 = m22;
        }

        public Matrix3(Matrix3 m)
        {
            this.m00 = m.m00; this.m01 = m.m01; this.m02 = m.m02;
            this.m10 = m.m10; this.m11 = m.m11; this.m12 = m.m12;
            this.m20 = m.m20; this.m21 = m.m21; this.m22 = m.m22;
        }

        #endregion

        #region Indexer and operators

        public double this[int i, int j]
        {
            get
            {
                switch (i * 3 + j)
                {
                    case 0: return m00;
                    case 1: return m01;
                    case 2: return m02;
                    case 3: return m10;
                    case 4: return m11;
                    case 5: return m12;
                    case 6: return m20;
                    case 7: return m21;
                    case 8: return m22;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
            set
            {
                switch (i * 3 + j)
                {
                    case 0: m00 = value; break;
                    case 1: m01 = value; break;
                    case 2: m02 = value; break;
                    case 3: m10 = value; break;
                    case 4: m11 = value; break;
                    case 5: m12 = value; break;
                    case 6: m20 = value; break;
                    case 7: m21 = value; break;
                    case 8: m22 = value; break;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }

        public static Matrix3 operator +(Matrix3 A, Matrix3 B)
        {
            return new Matrix3(A.m00 + B.m00, A.m01 + B.m01, A.m02 + B.m02,
                               A.m10 + B.m10, A.m11 + B.m11, A.m12 + B.m12,
                               A.m20 + B.m20, A.m21 + B.m21, A.m22 + B.m22);
        }

        public static Matrix3 operator -(Matrix3 A, Matrix3 B)
        {
            return new Matrix3(A.m00 - B.m00, A.m01 - B.m01, A.m02 - B.m02,
                               A.m10 - B.m10, A.m11 - B.m11, A.m12 - B.m12,
                               A.m20 - B.m20, A.m21 - B.m21, A.m22 - B.m22);
        }

        public static Matrix3 operator *(Matrix3 A, Matrix3 B)
        {
            return new Matrix3(A.m00 * B.m00 + A.m01 * B.m10 + A.m02 * B.m20,
                               A.m00 * B.m01 + A.m01 * B.m11 + A.m02 * B.m21,
                               A.m00 * B.m02 + A.m01 * B.m12 + A.m02 * B.m22,
                               A.m10 * B.m00 + A.m11 * B.m10 + A.m12 * B.m20,
                               A.m10 * B.m01 + A.m11 * B.m11 + A.m12 * B.m21,
                               A.m10 * B.m02 + A.m11 * B.m12 + A.m12 * B.m22,
                               A.m20 * B.m00 + A.m21 * B.m10 + A.m22 * B.m20,
                               A.m20 * B.m01 + A.m21 * B.m11 + A.m22 * B.m21,
                               A.m20 * B.m02 + A.m21 * B.m12 + A.m22 * B.m22);
        }

        public static Vector3 operator *(Matrix3 m, Vector3 v)
        {
            return new Vector3(m.m00 * v.x + m.m01 * v.y + m.m02 * v.z,
                               m.m10 * v.x + m.m11 * v.y + m.m12 * v.z,
                               m.m20 * v.x + m.m21 * v.y + m.m22 * v.z);
        }

        public static Matrix3 operator *(Matrix3 m, double s)
        {
            return new Matrix3(m.m00 * s, m.m01 * s, m.m02 * s,
                               m.m10 * s, m.m11 * s, m.m12 * s,
                               m.m20 * s, m.m21 * s, m.m22 * s);
        }

        #endregion

        #region Public methods

        public Matrix3 Transpose()
        {
            return new Matrix3(m00, m10, m20,
                               m01, m11, m21,
                               m02, m12, m22);
        }

        public double Determinant()
        {
            return (m00 * (m11 * m22 - m12 * m21)
                  + m01 * (m12 * m20 - m10 * m22) 
                  + m02 * (m10 * m21 - m11 * m20));
        }

        public Matrix3 Inverse()
        {
            double det = this.Determinant();

            if (det == 0) return null;

            return new Matrix3((m11 * m22 - m12 * m21) / det,
                               (m02 * m21 - m01 * m22) / det,
                               (m01 * m12 - m02 * m11) / det,
                               (m12 * m20 - m10 * m22) / det,
                               (m00 * m22 - m02 * m20) / det,
                               (m02 * m10 - m00 * m12) / det,
                               (m10 * m21 - m11 * m20) / det,
                               (m01 * m20 - m00 * m21) / det,
                               (m00 * m11 - m01 * m10) / det);

        }

        #endregion

    }
}
