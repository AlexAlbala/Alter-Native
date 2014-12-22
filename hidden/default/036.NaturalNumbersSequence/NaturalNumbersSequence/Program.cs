using System;
using System.Collections;
using System.Collections.Generic;

public class Program {

    public static void Main() {
    NaturalNumbersSequence numbers = new NaturalNumbersSequence();

    foreach(var n in numbers) {
        Console.Write(n + " ");
    }
    Console.WriteLine();
    }
}

public class NaturalNumbersSequence: IEnumerable<int>
{
    public class NaturalEnumerator: IEnumerator<int>
    {
        private int current = 1;
        private bool atStart = true;

    // interface members
        public int Current
        {
            get {
                return current;
            }
        }
        object IEnumerator.Current
        {
            get {
                return current;
            }
        }
        public void Reset()
        {
            atStart = true;
            current = 1;
        }
        public bool MoveNext()
        {
            if (atStart)
            {
                atStart = false;
                return true;
            }
            else
            {
                if (current < 1000)
                {
                    current++;
                    return true;
                }
                else
                    return false;
            }
        }
        public void Dispose()
        {
            // do nothing
        }
    }

    public IEnumerator<int> GetEnumerator()
    {
        return new NaturalEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return new NaturalEnumerator();
    }
}

