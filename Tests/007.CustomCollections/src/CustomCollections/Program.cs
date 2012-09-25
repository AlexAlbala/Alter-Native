using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomCollections
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> l = new List<int>();
            l.Add(12);
            l.Add(25);
            l.Add(6);

            MyList<int> myList = new MyList<int>(l);
            foreach (int n in myList)
                Console.WriteLine(n);
        }
    }

    class MyList<T> : IEnumerable<T>
    {
        private List<T> mylist;

        public MyList(List<T> values)
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
        private List<T> values;
        private int currentIndex;

        public MyEnumerator(List<T> values)
        {
            this.values = new List<T>(values);
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
            return currentIndex < values.Count;
        }

        public void Reset()
        {
            currentIndex = -1;
        }
    }
}
