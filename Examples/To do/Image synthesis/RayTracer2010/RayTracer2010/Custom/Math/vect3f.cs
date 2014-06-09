namespace Custom.Math
{
    public struct vect3f
    {
        public float x, y, z;

        public vect3f(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public vect3f(vect3f v)
        {
            x = v.x;
            y = v.y;
            z = v.z;
        }
        public vect3f(vect3d v)
        {
            x = (float)v.x;
            y = (float)v.y;
            z = (float)v.z;
        }
        public vect3f(vect3i v)
        {
            x = v.x;
            y = v.y;
            z = v.z;
        }
        public vect3f(float v)
        {
            x = v;
            y = v;
            z = v;
        }

        public float this[int i]
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
        public static vect3f operator +(vect3f a, vect3f b)
        {
            vect3f r;
            r.x = a.x + b.x;
            r.y = a.y + b.y;
            r.z = a.z + b.z;
            return r;
        }

        public static vect3f operator -(vect3f a, vect3f b)
        {
            vect3f r;
            r.x = a.x - b.x;
            r.y = a.y - b.y;
            r.z = a.z - b.z;
            return r;
        }
        public static vect3f operator -(vect3f b)
        {
            vect3f r;
            r.x = -b.x;
            r.y = -b.y;
            r.z = -b.z;
            return r;
        }

        public static vect3f operator *(vect3f a, float b)
        {
            vect3f r;
            r.x = a.x * b;
            r.y = a.y * b;
            r.z = a.z * b;
            return r;
        }
        public static vect3f operator *(float a, vect3f b)
        {
            vect3f r;
            r.x = a * b.x;
            r.y = a * b.y;
            r.z = a * b.z;
            return r;
        }
        public static float operator *(vect3f a, vect3f b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z;
        }

        public static vect3f operator /(vect3f a, float b)
        {
            vect3f r;
            r.x = a.x / b;
            r.y = a.y / b;
            r.z = a.z / b;
            return r;
        }

        public static vect3f operator %(vect3f a, vect3f b)
        {
            vect3f r;
            r.x = a.y * b.z - a.z * b.y;
            r.y = a.z * b.x - a.x * b.z;
            r.z = a.x * b.y - a.y * b.x;
            return r;
        }

        public vect3f times(vect3f a)
        {
            vect3f r;
            r.x = x * a.x;
            r.y = y * a.y;
            r.z = z * a.z;
            return r;
        }


        public float length2()
        {
            return this * this;
        }
        public float length()
        {
            return (float)System.Math.Sqrt(length2());
        }
        public static float operator !(vect3f a)
        {
            return a.length();
        }
        public static vect3f operator ~(vect3f a)
        {
            return a / a.length();
        }

        public static bool operator <(vect3f a, vect3f b)
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
        public static bool operator >(vect3f a, vect3f b)
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
        public bool Equals(vect3f b)
        {
            return x == b.x && y == b.y && z == b.z;
        }
        public static bool operator ==(vect3f a, vect3f b)
        {
            return a.x == b.x && a.y == b.y && a.z == b.z;
        }
        public static bool operator !=(vect3f a, vect3f b)
        {
            return a.x != b.x || a.y != b.y || a.z != b.z;
        }

        public override string ToString()
        {
            return "(" + x + ", " + y + ", " + z + ")";
        }
    }
}
