using System;

namespace Operators
{

    class OperatedVector
    {
        public const int DimSize = 3;
        private float[] m_array = new float[DimSize];

        // allow callers to initialize
        public float this[int x]
        {
            get { return m_array[x]; }
            set { m_array[x] = value; }
        }

        // let user add matrices
        public static OperatedVector operator +(OperatedVector mat1, OperatedVector mat2)
        {
            OperatedVector newMatrix = new OperatedVector();

            for (int x = 0; x < DimSize; x++)
                newMatrix[x] = mat1[x] + mat2[x];

            return newMatrix;
        }
    }

    class OperatedVector2
    {
        public double x, y;
        public OperatedVector2(double x, double y)
        {
            this.x = x; this.y = y;
        }
        public OperatedVector2 norm()
        {
            return this * (1.0 / System.Math.Sqrt(x * x + y * y));
        }
        public static OperatedVector2 operator *(OperatedVector2 a, double b)
        {
            return new OperatedVector2(a.x * b, a.y * b);
        }
    }

    public class A
    {
        public A() { }
        public static A operator -(A a, A b) { return new A(); }
    }

    public class B
    {
        public A a;
        public A Foo(B b)
        {
            return a - b.a;
        }
    }

    class MatrixTest
    {
        // used in the InitMatrix method.
        public static Random m_rand = new Random();

        // test Matrix
        static void Main()
        {
            OperatedVector mat1 = new OperatedVector();
            OperatedVector mat2 = new OperatedVector();

            // init matrices with random values
            InitMatrix(mat1, 3.0f);
            InitMatrix(mat2, 5.2f);

            // print out matrices
            Console.WriteLine("Vector 1: ");
            PrintMatrix(mat1);

            Console.WriteLine("Vector 2: ");
            PrintMatrix(mat2);

            // perform operation and print out results
            OperatedVector mat3 = mat1 + mat2;

            Console.WriteLine();
            Console.WriteLine("Vector 1 + Vector 2 = ");
            PrintMatrix(mat3);


            OperatedVector2 vec1 = new OperatedVector2(3, 4);
            OperatedVector2 result = vec1 * 5.0;

            Console.Write("X:");
            Console.WriteLine(result.x);
            Console.Write("Y:");
            Console.WriteLine(result.y);
        }

        // initialize matrix with random values
        public static void InitMatrix(OperatedVector mat, float f)
        {
            for (int x = 0; x < OperatedVector.DimSize; x++)
                mat[x] = f;
        }

        // print matrix to console
        public static void PrintMatrix(OperatedVector mat)
        {
            Console.WriteLine();
            Console.Write("[ ");
            for (int x = 0; x < OperatedVector.DimSize; x++)
            {
                // format the output
                Console.Write(mat[x]);
                if ((x) < (OperatedVector.DimSize - 1))
                    Console.Write(", ");
            }
            Console.WriteLine(" ]");
            Console.WriteLine();
        }
    }
}