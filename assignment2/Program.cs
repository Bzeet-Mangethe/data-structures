using System;
using System.Collections.Generic;

public class Stack<T>
{
    private List<T> elements;

    public Stack()
    {
        elements = new List<T>();
    }

    public void Push(T item)
    {
        elements.Add(item);
    }

    public T Pop()
    {
        if (elements.Count == 0)
        {
            throw new InvalidOperationException("Stack is empty");
        }
        int lastIndex = elements.Count - 1;
        T item = elements[lastIndex];
        elements.RemoveAt(lastIndex);
        return item;
    }

    public T Peek()
    {
        if (elements.Count == 0)
        {
            throw new InvalidOperationException("Stack is empty");
        }
        return elements[elements.Count - 1];
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
            throw new InvalidOperationException("Stack is empty");
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
        private Stack<T> stack;
        private int currentIndex;

        public Iterator(Stack<T> stack)
        {
            this.stack = stack;
            currentIndex = stack.Size() - 1;
        }

        public bool HasNext()
        {
            return currentIndex >= 0;
        }

        public T Next()
        {
            if (!HasNext())
            {
                throw new InvalidOperationException("No next element");
            }
            T item = stack.elements[currentIndex];
            currentIndex--;
            return item;
        }
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        Stack<int> myStack = new Stack<int>();
        myStack.Push(5);
        myStack.Push(10);
        myStack.Push(3);
        myStack.Push(7);
        myStack.Traverse();  // Output: 5 10 3 7
        Console.WriteLine(myStack.Pop());  // Output: 7
        Console.WriteLine(myStack.Peek());  // Output: 3
        Console.WriteLine(myStack.Contains(10));  // Output: True
        Console.WriteLine(myStack.Size());  // Output: 3
        Console.WriteLine(myStack.Center());  // Output: 10
        myStack.Sort();
        myStack.Traverse();  // Output: 3 5 10
        myStack.Reverse();
        myStack.Traverse();  // Output: 10 5 3

        Stack<int>.Iterator iterator = myStack.GetIterator();
        while (iterator.HasNext())
        {
            Console.Write(iterator.Next() + " ");  // Output: 10 5 3
        }
        Console.WriteLine();
    }
}

