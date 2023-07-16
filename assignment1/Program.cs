using System;

public class Node
{
    public int Data { get; set; }
    public Node Next { get; set; }

    public Node(int data)
    {
        Data = data;
        Next = null;
    }
}

public class LinkedList
{
    private Node head;

    public LinkedList()
    {
        head = null;
    }

    public void Insert(int data)
    {
        Node newNode = new Node(data);
        if (head == null)
        {
            head = newNode;
        }
        else
        {
            Node current = head;
            while (current.Next != null)
            {
                current = current.Next;
            }
            current.Next = newNode;
        }
    }

    public void InsertAtPosition(int data, int position)
    {
        Node newNode = new Node(data);
        if (position < 0)
        {
            throw new IndexOutOfRangeException("Invalid position");
        }
        if (position == 0)
        {
            newNode.Next = head;
            head = newNode;
        }
        else
        {
            Node current = head;
            Node previous = null;
            int count = 0;
            while (current != null && count < position)
            {
                previous = current;
                current = current.Next;
                count++;
            }
            if (count < position)
            {
                throw new IndexOutOfRangeException("Position out of range");
            }
            newNode.Next = current;
            previous.Next = newNode;
        }
    }

    public void Delete(int data)
    {
        if (head == null)
        {
            return;
        }
        if (head.Data == data)
        {
            head = head.Next;
            return;
        }
        Node current = head;
        Node previous = null;
        while (current != null && current.Data != data)
        {
            previous = current;
            current = current.Next;
        }
        if (current != null)
        {
            previous.Next = current.Next;
        }
    }

    public void DeleteAtPosition(int position)
    {
        if (position < 0)
        {
            throw new IndexOutOfRangeException("Invalid position");
        }
        if (position == 0)
        {
            if (head != null)
            {
                head = head.Next;
            }
            else
            {
                throw new IndexOutOfRangeException("Position out of range");
            }
        }
        else
        {
            Node current = head;
            Node previous = null;
            int count = 0;
            while (current != null && count < position)
            {
                previous = current;
                current = current.Next;
                count++;
            }
            if (count < position)
            {
                throw new IndexOutOfRangeException("Position out of range");
            }
            if (current != null)
            {
                previous.Next = current.Next;
            }
        }
    }

    public int Center()
    {
        if (head == null)
        {
            throw new InvalidOperationException("List is empty");
        }
        Node slow = head;
        Node fast = head;
        while (fast != null && fast.Next != null)
        {
            slow = slow.Next;
            fast = fast.Next.Next;
        }
        return slow.Data;
    }

    public void Sort()
    {
        head = MergeSort(head);
    }

    private Node MergeSort(Node node)
    {
        if (node == null || node.Next == null)
        {
            return node;
        }
        Node middle = GetMiddle(node);
        Node nextToMiddle = middle.Next;
        middle.Next = null;
        Node left = MergeSort(node);
        Node right = MergeSort(nextToMiddle);
        return Merge(left, right);
    }

    private Node GetMiddle(Node node)
    {
        if (node == null)
        {
            return node;
        }
        Node slow = node;
        Node fast = node.Next;
        while (fast != null)
        {
            fast = fast.Next;
            if (fast != null)
            {
                slow = slow.Next;
                fast = fast.Next;
            }
        }
        return slow;
    }

    private Node Merge(Node left, Node right)
    {
        if (left == null)
        {
            return right;
        }
        if (right == null)
        {
            return left;
        }
        Node result = null;
        if (left.Data <= right.Data)
        {
            result = left;
            result.Next = Merge(left.Next, right);
        }
        else
        {
            result = right;
            result.Next = Merge(left, right.Next);
        }
        return result;
    }

    public void Reverse()
    {
        Node previous = null;
        Node current = head;
        while (current != null)
        {
            Node next = current.Next;
            current.Next = previous;
            previous = current;
            current = next;
        }
        head = previous;
    }

    public int Size()
    {
        int count = 0;
        Node current = head;
        while (current != null)
        {
            count++;
            current = current.Next;
        }
        return count;
    }

    public void Traverse()
    {
        Node current = head;
        while (current != null)
        {
            Console.Write(current.Data + " ");
            current = current.Next;
        }
        Console.WriteLine();
    }

    public Iterator GetIterator()
    {
        return new Iterator(this);
    }

    public class Iterator
    {
        private Node current;

        public Iterator(LinkedList list)
        {
            current = list.head;
        }

        public bool HasNext()
        {
            return current != null;
        }

        public int Next()
        {
            if (current == null)
            {
                throw new InvalidOperationException("No next element");
            }
            int data = current.Data;
            current = current.Next;
            return data;
        }
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        LinkedList myLinkedList = new LinkedList();
        myLinkedList.Insert(5);
        myLinkedList.Insert(10);
        myLinkedList.Insert(3);
        myLinkedList.Insert(7);
        myLinkedList.Traverse();  // Output: 5 10 3 7
        myLinkedList.InsertAtPosition(15, 2);
        myLinkedList.Traverse();  // Output: 5 10 15 3 7
        myLinkedList.Delete(10);
        myLinkedList.Traverse();  // Output: 5 15 3 7
        Console.WriteLine(myLinkedList.Center());  // Output: 15
        myLinkedList.Sort();
        myLinkedList.Traverse();  // Output: 3 5 7 15
        myLinkedList.Reverse();
        myLinkedList.Traverse();  // Output: 15 7 5 3
        Console.WriteLine(myLinkedList.Size());  // Output: 4

        LinkedList.Iterator iterator = myLinkedList.GetIterator();
        while (iterator.HasNext())
        {
            Console.Write(iterator.Next() + " ");  // Output: 15 7 5 3
        }
        Console.WriteLine();
    }
}


