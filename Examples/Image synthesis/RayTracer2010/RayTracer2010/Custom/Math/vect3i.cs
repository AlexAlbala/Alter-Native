namespace Custom.Math
{
    public struct vect3i
    {
        public int x, y, z;

        public vect3i(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public vect3i(vect3d v)
        {
            x = (int)System.Math.Floor(v.x);
            y = (int)System.Math.Floor(v.y);
            z = (int)System.Math.Floor(v.z);
        }
        public vect3i(vect3i v)
        {
            x = v.x;
            y = v.y;
            z = v.z;
        }
        public vect3i(int v)
        {
            x = v;
            y = v;
            z = v;
        }

        public int this[int i]
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
        public static vect3i operator +(vect3i a, vect3i b)
        {
            vect3i r;
            r.x = a.x + b.x;
            r.y = a.y + b.y;
            r.z = a.z + b.z;
            return r;
        }

        public static vect3i operator -(vect3i a, vect3i b)
        {
            vect3i r;
            r.x = a.x - b.x;
            r.y = a.y - b.y;
            r.z = a.z - b.z;
            return r;
        }
        public static vect3i operator -(vect3i b)
        {
            vect3i r;
            r.x = -b.x;
            r.y = -b.y;
            r.z = -b.z;
            return r;
        }

        public static vect3i operator *(vect3i a, int b)
        {
            vect3i r;
            r.x = a.x * b;
            r.y = a.y * b;
            r.z = a.z * b;
            return r;
        }
        public static vect3i operator *(int a, vect3i b)
        {
            vect3i r;
            r.x = a * b.x;
            r.y = a * b.y;
            r.z = a * b.z;
            return r;
        }
        public static int operator *(vect3i a, vect3i b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z;
        }

        public static vect3i operator /(vect3i a, int b)
        {
            vect3i r;
            r.x = a.x / b;
            r.y = a.y / b;
            r.z = a.z / b;
            return r;
        }

        public static vect3i operator %(vect3i a, vect3i b)
        {
            vect3i r;
            r.x = a.y * b.z - a.z * b.y;
            r.y = a.z * b.x - a.x * b.z;
            r.z = a.x * b.y - a.y * b.x;
            return r;
        }

        public int length2()
        {
            return this * this;
        }
        public double length()
        {
            return System.Math.Sqrt((double)length2());
        }
        public static double operator !(vect3i a)
        {
            return a.length();
        }
        public static vect3d operator ~(vect3i a)
        {
            return new vect3d(a) / a.length();
        }

        public static bool operator <(vect3i a, vect3i b)
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
        public static bool operator >(vect3i a, vect3i b)
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
            return x ^ y ^ z;
        }
        public override bool Equals(object p)
        {
            return Equals(p);
        }
        public bool Equals(vect3i b)
        {
            return x == b.x && y == b.y && z == b.z;
        }
        public static bool operator ==(vect3i a, vect3i b)
        {
            return a.x == b.x && a.y == b.y && a.z == b.z;
        }
        public static bool operator !=(vect3i a, vect3i b)
        {
            return a.x != b.x || a.y != b.y || a.z != b.z;
        }

        public override string ToString()
        {
            return "(" + x + ", " + y + ", " + z + ")";
        }
    }
}
