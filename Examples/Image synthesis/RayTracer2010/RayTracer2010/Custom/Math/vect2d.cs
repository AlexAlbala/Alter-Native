using System.IO;

namespace Custom.Math
{
    public struct vect2d
    {
        public double x, y;

        public vect2d(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
        public vect2d(vect3d v)
        {
            x = v.x;
            y = v.y;
        }
        public vect2d(vect3i v)
        {
            x = v.x;
            y = v.y;
        }
        public vect2d(double v)
        {
            x = v;
            y = v;
        }

        public void save(BinaryWriter f)
        {
            f.Write(x);
            f.Write(y);
        }

        public void load(BinaryReader f)
        {
            x = f.ReadDouble();
            y = f.ReadDouble();
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
                }
            }
        }
        public static vect2d operator +(vect2d a, vect2d b)
        {
            vect2d r;
            r.x = a.x + b.x;
            r.y = a.y + b.y;
            return r;
        }

        public static vect2d operator -(vect2d a, vect2d b)
        {
            vect2d r;
            r.x = a.x - b.x;
            r.y = a.y - b.y;
            return r;
        }
        public static vect2d operator -(vect2d b)
        {
            vect2d r;
            r.x = -b.x;
            r.y = -b.y;
            return r;
        }

        public static vect2d operator *(vect2d a, double b)
        {
            vect2d r;
            r.x = a.x * b;
            r.y = a.y * b;
            return r;
        }
        public static vect2d operator *(double a, vect2d b)
        {
            vect2d r;
            r.x = a * b.x;
            r.y = a * b.y;
            return r;
        }
        public static double operator *(vect2d a, vect2d b)
        {
            return a.x * b.x + a.y * b.y;
        }

        public static vect2d operator /(vect2d a, double b)
        {
            vect2d r;
            r.x = a.x / b;
            r.y = a.y / b;
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
        public static double operator !(vect2d a)
        {
            return a.length();
        }
        public static vect2d operator ~(vect2d a)
        {
            return a / a.length();
        }

        public static bool operator <(vect2d a, vect2d b)
        {
            if (a.x < b.x)
                return true;
            else if (a.x > b.x)
                return false;

            if (a.y < b.y)
                return true;
            else if (a.y > b.y)
                return false;

            return false;
        }
        public static bool operator >(vect2d a, vect2d b)
        {
            if (a.x > b.x)
                return true;
            else if (a.x < b.x)
                return false;

            if (a.y > b.y)
                return true;
            else if (a.y < b.y)
                return false;

            return false;
        }

        public override int GetHashCode()
        {
            long h = (long)x ^ (long)y;
            return (int)(h & 0xffffffff) ^ (int)(h >> 32);
        }
        public override bool Equals(object p)
        {
            return Equals(p);
        }
        public bool Equals(vect2d b)
        {
            return x == b.x && y == b.y;
        }
        public static bool operator ==(vect2d a, vect2d b)
        {
            return a.x == b.x && a.y == b.y;
        }
        public static bool operator !=(vect2d a, vect2d b)
        {
            return a.x != b.x || a.y != b.y;
        }
        public override string ToString()
        {
            return "(" + x + ", " + y + ")";
        }
    }
}
