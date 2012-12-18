using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomCollections
{
    class A
    {
        public A()
        {
            value = 35;
        }
        public int value;
    }
    class Program
    {
        static void Main(string[] args)
        {
            int[] l = new int[3];
            l[0] = 2;
            l[1] = 3;
            l[2] = 4;

            MyList<int> myList = new MyList<int>(l);

            foreach (int n in myList)
                Console.WriteLine(n);

            A[] al = new A[3];
            al[0] = new A();
            al[1] = new A();
            al[2] = new A();

            MyList<A> myListA = new MyList<A>(al);

            foreach (A na in myListA)
                Console.WriteLine(na.value);
        }
    }

    class MyList<T> : IEnumerable<T>
    {
        private T[] mylist;

        public MyList(T[] values)
        {
            this.mylist = values;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new MyEnumerator<T>(mylist);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class MyEnumerator<T> : IEnumerator<T>
    {
        private T[] values;
        private int currentIndex;

        public MyEnumerator(T[] values)
        {
            this.values = values;
            Reset();
        }
        public T Current
        {
            get { return values[currentIndex]; }
        }

        public void Dispose()
        {

        }

        object System.Collections.IEnumerator.Current
        {
            get { return Current; }
        }

        public bool MoveNext()
        {
            currentIndex++;
            return currentIndex < values.Length;
        }

        public void Reset()
        {
            currentIndex = -1;
        }
    }
}
