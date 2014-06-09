using System.IO;

namespace Custom.Math
{
    public struct vect3d
    {
        public double x, y, z;

        public vect3d(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public vect3d(vect3d v)
        {
            x = v.x;
            y = v.y;
            z = v.z;
        }
        public vect3d(vect3i v)
        {
            x = v.x;
            y = v.y;
            z = v.z;
        }
        public vect3d(double v)
        {
            x = v;
            y = v;
            z = v;
        }

        public void save(BinaryWriter f)
        {
            f.Write(x);
            f.Write(y);
            f.Write(z);
        }

        public void load(BinaryReader f)
        {
            x = f.ReadDouble();
            y = f.ReadDouble();
            z = f.ReadDouble();
        }

        public double this[int i]
        {
            get
            {
                switch (i)
                {
                    case 0:
                        return x;
                    case 1:
                        return y;
                    case 2:
                        return z;
                }
                return 0;
            }
            set
            {
                switch (i)
                {
                    case 0:
                        x = value;
                        break;
                    case 1:
                        y = value;
                        break;
                    case 2:
                        z = value;
                        break;
                }
            }
        }

        public static vect3d operator +(vect3d a, vect3d b)
        {
            vect3d r;
            r.x = a.x + b.x;
            r.y = a.y + b.y;
            r.z = a.z + b.z;
            return r;
        }

        public static vect3d operator +(vect3d a, double b)
        {
            vect3d r;
            r.x = a.x + b;
            r.y = a.y + b;
            r.z = a.z + b;
            return r;
        }

        public static vect3d operator -(vect3d a, vect3d b)
        {
            vect3d r;
            r.x = a.x - b.x;
            r.y = a.y - b.y;
            r.z = a.z - b.z;
            return r;
        }
        public static vect3d operator -(vect3d b)
        {
            vect3d r;
            r.x = -b.x;
            r.y = -b.y;
            r.z = -b.z;
            return r;
        }

        public static vect3d operator *(vect3d a, double b)
        {
            vect3d r;
            r.x = a.x * b;
            r.y = a.y * b;
            r.z = a.z * b;
            return r;
        }
        public static vect3d operator *(double a, vect3d b)
        {
            vect3d r;
            r.x = a * b.x;
            r.y = a * b.y;
            r.z = a * b.z;
            return r;
        }
        public static double operator *(vect3d a, vect3d b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z;
        }

        public static vect3d operator /(vect3d a, double b)
        {
            vect3d r;
            r.x = a.x / b;
            r.y = a.y / b;
            r.z = a.z / b;
            return r;
        }

        public static vect3d operator %(vect3d a, vect3d b)
        {
            vect3d r;
            r.x = a.y * b.z - a.z * b.y;
            r.y = a.z * b.x - a.x * b.z;
            r.z = a.x * b.y - a.y * b.x;
            return r;
        }

        public vect3d times(vect3d a)
        {
            vect3d r;
            r.x = x * a.x;
            r.y = y * a.y;
            r.z = z * a.z;
            return r;
        }

        public double length2()
        {
            return this * this;
        }
        public double length()
        {
            return System.Math.Sqrt(length2());
        }
        public static double operator !(vect3d a)
        {
            return a.length();
        }
        public static vect3d operator ~(vect3d a)
        {
            return a / a.length();
        }
        public void normalize()
	    {
    		double f = 1.0 / length();
            x *= f;
            y *= f;
            z *= f;
	    }
        public static bool operator <(vect3d a, vect3d b)
        {
            if (a.x < b.x)
                return true;
            else if (a.x > b.x)
                return false;

            if (a.y < b.y)
                return true;
            else if (a.y > b.y)
                return false;

            if (a.z < b.z)
                return true;
            else if (a.z > b.z)
                return false;

            return false;
        }
        public static bool operator >(vect3d a, vect3d b)
        {
            if (a.x > b.x)
                return true;
            else if (a.x < b.x)
                return false;

            if (a.y > b.y)
                return true;
            else if (a.y < b.y)
                return false;

            if (a.z > b.z)
                return true;
            else if (a.z < b.z)
                return false;

            return false;
        }

        public override int GetHashCode()
        {
            long h = (long)x ^ (long)y ^ (long)z;
            return (int)(h & 0xffffffff) ^ (int)(h >> 32);
        }
        public override bool Equals(object p)
        {
            return Equals(p);
        }
        public bool Equals(vect3d b)
        {
            return x == b.x && y == b.y && z == b.z;
        }
        public static bool operator ==(vect3d a, vect3d b)
        {
            return a.x == b.x && a.y == b.y && a.z == b.z;
        }
        public static bool operator !=(vect3d a, vect3d b)
        {
            return a.x != b.x || a.y != b.y || a.z != b.z;
        }

        public override string ToString()
        {
            return "(" + x + ", " + y + ", " + z + ")";
        }
    }
}
