using System.IO;

namespace Custom.Math
{
    public struct box3d
    {
        public double xl, yl, zl, xh, yh, zh;

        public static int[,] cube_edge2vert = { { 0, 1 }, { 0, 2 }, { 1, 3 }, { 2, 3 }, { 0, 4 }, { 1, 5 }, { 2, 6 }, { 3, 7 }, { 4, 5 }, { 4, 6 }, { 5, 7 }, { 6, 7 } };
        public static int[,] cube_face2vert = { { 0, 1, 3, 2 }, { 0, 4, 5, 1 }, { 0, 2, 6, 4 }, { 1, 5, 7, 3 }, { 2, 3, 7, 6 }, { 4, 6, 7, 5 } };
        public static int[,] cube_face2edge = { { 1, 0, 2, 3 }, { 0, 4, 8, 5 }, { 4, 1, 6, 9 }, { 2, 5, 10, 7 }, { 6, 3, 7, 11 }, { 8, 9, 11, 10 } };
        public static int[,] cube_orient2face = { { 2, 3 }, { 1, 4 }, { 0, 5 } };
        public static int[] cube_face2orient = { 2, 1, 0, 0, 1, 2 };
        public static int[] cube_edge2orient = { 0, 1, 1, 0, 2, 2, 2, 2, 0, 1, 1, 0 };
        public static int[,] cube_orient2edge = { { 0, 3, 8, 11 }, { 1, 2, 9, 10 }, { 4, 5, 6, 7 } };
        public static int[] cube_face2opposite = { 5, 4, 3, 2, 1, 0 };
        public static int[,] cube_faceuv2coord = { { -1, 1 }, { 1, 1 }, { 1, -1 }, { 1, 1 }, { -1, 1}, { 1, 1 } };
        public static int[,] cube_faceuv2dims = { { 1, 2 }, { 0, 2}, { 0, 1 } };

        public box3d(vect3d mine, vect3d maxe)
        {
            xl = mine.x;
            yl = mine.y;
            zl = mine.z;

            xh = maxe.x;
            yh = maxe.y;
            zh = maxe.z;
        }

        public void save(BinaryWriter f)
        {
            f.Write(xl);
            f.Write(yl);
            f.Write(zl);
            f.Write(xh);
            f.Write(yh);
            f.Write(zh);
        }

        public void load(BinaryReader f)
        {
            xl = f.ReadDouble();
            yl = f.ReadDouble();
            zl = f.ReadDouble();
            xh = f.ReadDouble();
            yh = f.ReadDouble();
            zh = f.ReadDouble();
        }

        public double this[int minmax, int dim]
        {
            get
            {
                int k = minmax * 3 + dim;
                switch (k)
                {
                    case 0:
                        return xl;
                    case 1:
                        return yl;
                    case 2:
                        return zl;
                    case 3:
                        return xh;
                    case 4:
                        return yh;
                    default:
                        return zh;
                }
            }
            set
            {
                int k = minmax * 3 + dim;
                switch (k)
                {
                    case 0:
                        xl = value;
                        break;
                    case 1:
                        yl = value;
                        break;
                    case 2:
                        zl = value;
                        break;
                    case 3:
                        xh = value;
                        break;
                    case 4:
                        yh = value;
                        break;
                    case 5:
                        zh = value;
                        break;
                }
            }
        }

        public vect3d this[int minmax]
        {
            get
            {
                switch (minmax)
                {
                    case 0:
                        return new vect3d(xl, yl, zl);
                    default:
                        return new vect3d(xh, yh, zh);
                }
            }
            set
            {
                switch (minmax)
                {
                    case 0:
                        xl = value.x;
                        yl = value.y;
                        zl = value.z;
                        break;
                    case 1:
                        xh = value.x;
                        yh = value.y;
                        zh = value.z;
                        break;
                }
            }
        }
        public void fix()
        {
            if (xh < xl)
            {
                double xt = xh;
                xh = xl;
                xl = xt;
            }
            if (yh < yl)
            {
                double yt = yh;
                yh = yl;
                yl = yt;
            }
            if (zh < zl)
            {
                double zt = zh;
                zh = zl;
                zl = zt;
            }
        }

        public bool hasSameVerts(box3d b)
        {
            return xl == b.xl && xh == b.xh && yl == b.yl && yh == b.yh && zl == b.zl &&  zh == b.zh;
        }

        public double volume()
        {
            return (xh - xl) * (yh - yl) * (zh - zl);
        }

        public vect3d vertPos(int i)
        {
            vect3d p = this[0];
            if ((i & 1) != 0)
                p[0] = this[1, 0];
            if ((i & 2) != 0)
                p[1] = this[1, 1];
            if ((i & 4) != 0)
                p[2] = this[1, 2];
            return p;
        }

        public double area()
        {
            double x = xh - xl;
            double y = yh - yl;
            double z = zh - zl;

            return 2 * x * y + 2 * x * z + 2 * y * z;
        }
    }
}
