using System;
using System.Collections.Generic;
using System.Text;

namespace USAL
{
    /// <summary>[PENDING] The Vector3 class is a 3 columns object</summary>
    /// <remarks>
    /// <para>USAL/Vector3. AutoNAV4D 4.1 Revisión 93</para>
    /// <para>©Joshua M. Tristancho.</para>
    /// <para>Esta clase está protegida por la licencia BSD de código abierto.</para>
    /// <para>0.Puedes ejecutar el programa con cualquier propósito.</para>
    /// <para>1.Puedes estudiar y/o modificar el programa.</para>
    /// <para>2.Puedes copiar el programa de manera que puedas ayudar a tu vecino.</para>
    /// <para>3.Puedes mejorar el programa, y hacer públicas las mejoras, de forma que se beneficie toda la comunidad.</para>
    /// </remarks>
    [Serializable]
    public class Vector3
    {
        public double x;
        public double y;
        public double z;

        #region Constructors

        public Vector3()
        {
            this.x = 0;
            this.y = 0;
            this.z = 0;
        }

        public Vector3(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector3(Vector3 v)
        {
            this.x = v.x;
            this.y = v.y;
            this.z = v.z;
        }

        #endregion

        #region Indexer and operators

        public double this[int i]
        {
            get {
                if (i == 0) return x;
                else if (i == 1) return y;
                else if (i == 2) return z;
                else throw new IndexOutOfRangeException();
            }
            set
            {
                if (i == 0) x = value;
                else if (i == 1) y = value;
                else if (i == 2) z = value;
                else throw new IndexOutOfRangeException();
            }
        }

        public static Vector3 operator +(Vector3 A, Vector3 B)
        {
            return new Vector3(A.x + B.x, A.y + B.y, A.z + B.z);
        }

        public static Vector3 operator -(Vector3 A, Vector3 B)
        {
            return new Vector3(A.x - B.x, A.y - B.y, A.z - B.z);
        }

        public static double operator *(Vector3 A, Vector3 B)
        {
            return (A.x * B.x + A.y * B.y + A.z * B.z);
        }

        #endregion

        #region Public methods
        
        public void Normalize()
        {
            double mag2 = x * x + y * y + z * z;
            if (mag2 > 0.00001)// TODO: Revisar este límite
            {
                double mag = Math.Sqrt(mag2);
                x /= mag;
                y /= mag;
                z /= mag;
            }
        }

        #endregion

    }
}
