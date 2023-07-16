using System;
using System.Collections.Generic;

public class Queue<T>
{
    private List<T> elements;

    public Queue()
    {
        elements = new List<T>();
    }

    public void Enqueue(T item)
    {
        elements.Add(item);
    }

    public T Dequeue()
    {
        if (elements.Count == 0)
        {
            throw new InvalidOperationException("Queue is empty");
        }
        T item = elements[0];
        elements.RemoveAt(0);
        return item;
    }

    public T Peek()
    {
        if (elements.Count == 0)
        {
            throw new InvalidOperationException("Queue is empty");
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

    public T Center()
    {
        if (elements.Count == 0)
        {
            throw new InvalidOperationException("Queue is empty");
        }
        int middleIndex = elements.Count / 2;
        return elements[middleIndex];
    }

    public void Sort()
    {
        elements.Sort();
    }

    public void Reverse()
    {
        elements.Reverse();
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
        private Queue<T> queue;
        private int currentIndex;

        public Iterator(Queue<T> queue)
        {
            this.queue = queue;
            currentIndex = 0;
        }

        public bool HasNext()
        {
            return currentIndex < queue.Size();
        }

        public T Next()
        {
            if (!HasNext())
            {
                throw new InvalidOperationException("No next element");
            }
            T item = queue.elements[currentIndex];
            currentIndex++;
            return item;
        }
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        Queue<int> myQueue = new Queue<int>();
        myQueue.Enqueue(5);
        myQueue.Enqueue(10);
        myQueue.Enqueue(3);
        myQueue.Enqueue(7);
        myQueue.Traverse();  // Output: 5 10 3 7
        Console.WriteLine(myQueue.Dequeue());  // Output: 5
        Console.WriteLine(myQueue.Peek());  // Output: 10
        Console.WriteLine(myQueue.Contains(10));  // Output: True
        Console.WriteLine(myQueue.Size());  // Output: 3
        Console.WriteLine(myQueue.Center());  // Output: 10
        myQueue.Sort();
        myQueue.Traverse();  // Output: 3 7 10
        myQueue.Reverse();
        myQueue.Traverse();  // Output: 10 7 3

        Queue<int>.Iterator iterator = myQueue.GetIterator();
        while (iterator.HasNext())
        {
            Console.Write(iterator.Next() + " ");  // Output: 10 7 3
        }
        Console.WriteLine();
    }
}
