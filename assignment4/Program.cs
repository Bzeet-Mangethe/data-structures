using System;
using System.Collections.Generic;

public class PriorityQueue<T>
{
    private List<T> elements;
    private IComparer<T> comparer;

    public PriorityQueue()
    {
        elements = new List<T>();
        comparer = Comparer<T>.Default;
    }

    public PriorityQueue(IComparer<T> customComparer)
    {
        elements = new List<T>();
        comparer = customComparer;
    }

    public void Enqueue(T item)
    {
        elements.Add(item);
        elements.Sort(comparer);
    }

    public T Dequeue()
    {
        if (elements.Count == 0)
        {
            throw new InvalidOperationException("Priority queue is empty");
        }
        T item = elements[0];
        elements.RemoveAt(0);
        return item;
    }

    public T Peek()
    {
        if (elements.Count == 0)
        {
            throw new InvalidOperationException("Priority queue is empty");
        }
        return elements[0];
    }

    public bool Contains(T item)
    {
        return elements.Contains(item);
    }

    public int Size()
    {
        return elements.Count;
    }

    public void Reverse()
    {
        elements.Reverse();
    }

    public T Center()
    {
        if (elements.Count == 0)
        {
            throw new InvalidOperationException("Priority queue is empty");
        }
        int middleIndex = elements.Count / 2;
        return elements[middleIndex];
    }

    public Iterator GetIterator()
    {
        return new Iterator(this);
    }

    public void Traverse()
    {
        foreach (T item in elements)
        {
            Console.Write(item + " ");
        }
        Console.WriteLine();
    }

    public class Iterator
    {
        private PriorityQueue<T> priorityQueue;
        private int currentIndex;

        public Iterator(PriorityQueue<T> priorityQueue)
        {
            this.priorityQueue = priorityQueue;
            currentIndex = 0;
        }

        public bool HasNext()
        {
            return currentIndex < priorityQueue.Size();
        }

        public T Next()
        {
            if (!HasNext())
            {
                throw new InvalidOperationException("No next element");
            }
            T item = priorityQueue.elements[currentIndex];
            currentIndex++;
            return item;
        }
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        // Example usage for PriorityQueue<int>
        PriorityQueue<int> myPriorityQueue = new PriorityQueue<int>();
        myPriorityQueue.Enqueue(5);
        myPriorityQueue.Enqueue(10);
        myPriorityQueue.Enqueue(3);
        myPriorityQueue.Enqueue(7);
        myPriorityQueue.Traverse();  // Output: 3 5 7 10
        Console.WriteLine(myPriorityQueue.Dequeue());  // Output: 3
        Console.WriteLine(myPriorityQueue.Peek());  // Output: 5
        Console.WriteLine(myPriorityQueue.Contains(10));  // Output: True
        Console.WriteLine(myPriorityQueue.Size());  // Output: 3
        myPriorityQueue.Reverse();
        myPriorityQueue.Traverse();  // Output: 10 7 5

        // Example usage for PriorityQueue<string> with custom comparer
        PriorityQueue<string> stringPriorityQueue = new PriorityQueue<string>(StringComparer.OrdinalIgnoreCase);
        stringPriorityQueue.Enqueue("Apple");
        stringPriorityQueue.Enqueue("banana");
        stringPriorityQueue.Enqueue("cherry");
        stringPriorityQueue.Traverse();  // Output: Apple banana cherry
        Console.WriteLine(stringPriorityQueue.Dequeue());  // Output: Apple
        Console.WriteLine(stringPriorityQueue.Peek());  // Output: banana

        PriorityQueue<int>.Iterator iterator = myPriorityQueue.GetIterator();
        while (iterator.HasNext())
        {
            Console.Write(iterator.Next() + " ");  // Output: 10 7 5
        }
        Console.WriteLine();
    }
}
