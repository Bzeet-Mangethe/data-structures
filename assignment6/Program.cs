using System;
using System.Collections.Generic;

public class HashTable<TKey, TValue>
{
    private class Entry
    {
        public TKey Key { get; }
        public TValue Value { get; }

        public Entry(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }

    private const int DefaultCapacity = 16;
    private const float DefaultLoadFactor = 0.75f;

    private LinkedList<Entry>[] buckets;
    private int count;
    private float loadFactor;
    private int threshold;

    public HashTable()
        : this(DefaultCapacity, DefaultLoadFactor)
    {
    }

    public HashTable(int capacity)
        : this(capacity, DefaultLoadFactor)
    {
    }

    public HashTable(int capacity, float loadFactor)
    {
        if (capacity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be a positive integer.");
        }
        if (loadFactor <= 0 || loadFactor > 1)
        {
            throw new ArgumentOutOfRangeException(nameof(loadFactor), "Load factor must be a value between 0 and 1.");
        }

        buckets = new LinkedList<Entry>[capacity];
        for (int i = 0; i < capacity; i++)
        {
            buckets[i] = new LinkedList<Entry>();
        }
        count = 0;
        this.loadFactor = loadFactor;
        threshold = (int)(capacity * loadFactor);
    }

    public void Insert(TKey key, TValue value)
    {
        if (Contains(key))
        {
            throw new ArgumentException("An element with the same key already exists.");
        }

        int bucketIndex = GetBucketIndex(key);
        buckets[bucketIndex].AddLast(new Entry(key, value));
        count++;

        if (count > threshold)
        {
            Resize();
        }
    }

    public bool Delete(TKey key)
    {
        int bucketIndex = GetBucketIndex(key);
        LinkedList<Entry> bucket = buckets[bucketIndex];
        foreach (Entry entry in bucket)
        {
            if (EqualityComparer<TKey>.Default.Equals(entry.Key, key))
            {
                bucket.Remove(entry);
                count--;
                return true;
            }
        }
        return false;
    }

    public bool Contains(TKey key)
    {
        int bucketIndex = GetBucketIndex(key);
        LinkedList<Entry> bucket = buckets[bucketIndex];
        foreach (Entry entry in bucket)
        {
            if (EqualityComparer<TKey>.Default.Equals(entry.Key, key))
            {
                return true;
            }
        }
        return false;
    }

    public TValue GetValue(TKey key)
    {
        int bucketIndex = GetBucketIndex(key);
        LinkedList<Entry> bucket = buckets[bucketIndex];
        foreach (Entry entry in bucket)
        {
            if (EqualityComparer<TKey>.Default.Equals(entry.Key, key))
            {
                return entry.Value;
            }
        }
        throw new KeyNotFoundException("The specified key was not found.");
    }

    public int Size()
    {
        return count;
    }

    public Iterator GetIterator()
    {
        return new Iterator(this);
    }

    public void Traverse()
    {
        foreach (LinkedList<Entry> bucket in buckets)
        {
            foreach (Entry entry in bucket)
            {
                Console.WriteLine($"Key: {entry.Key}, Value: {entry.Value}");
            }
        }
    }

    private int GetBucketIndex(TKey key)
    {
        int hashCode = key.GetHashCode();
        int bucketIndex = hashCode % buckets.Length;
        if (bucketIndex < 0)
        {
            bucketIndex += buckets.Length;
        }
        return bucketIndex;
    }

    private void Resize()
    {
        int newCapacity = buckets.Length * 2;
        LinkedList<Entry>[] newBuckets = new LinkedList<Entry>[newCapacity];
        for (int i = 0; i < newCapacity; i++)
        {
            newBuckets[i] = new LinkedList<Entry>();
        }

        foreach (LinkedList<Entry> bucket in buckets)
        {
            foreach (Entry entry in bucket)
            {
                int newBucketIndex = entry.Key.GetHashCode() % newCapacity;
                if (newBucketIndex < 0)
                {
                    newBucketIndex += newCapacity;
                }
                newBuckets[newBucketIndex].AddLast(entry);
            }
        }

        buckets = newBuckets;
        threshold = (int)(newCapacity * loadFactor);
    }

    public class Iterator
    {
        private HashTable<TKey, TValue> hashTable;
        private int bucketIndex;
        private LinkedListNode<Entry> currentEntry;

        public Iterator(HashTable<TKey, TValue> hashTable)
        {
            this.hashTable = hashTable;
            bucketIndex = 0;
            currentEntry = null;
        }

        public bool HasNext()
        {
            if (currentEntry != null && currentEntry.Next != null)
            {
                currentEntry = currentEntry.Next;
                return true;
            }

            while (bucketIndex < hashTable.buckets.Length - 1)
            {
                bucketIndex++;
                LinkedList<Entry> bucket = hashTable.buckets[bucketIndex];
                if (bucket.Count > 0)
                {
                    currentEntry = bucket.First;
                    return true;
                }
            }
            return false;
        }

        public KeyValuePair<TKey, TValue> Next()
        {
            if (!HasNext())
            {
                throw new InvalidOperationException("No next element");
            }
            Entry entry = currentEntry.Value;
            return new KeyValuePair<TKey, TValue>(entry.Key, entry.Value);
        }
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            HashTable<string, int> myHashTable = new HashTable<string, int>();
            myHashTable.Insert("one", 1);
            myHashTable.Insert("two", 2);
            myHashTable.Insert("three", 3);
            myHashTable.Insert("four", 4);

            myHashTable.Traverse();
            // Output:
            // Key: one, Value: 1
            // Key: two, Value: 2
            // Key: three, Value: 3
            // Key: four, Value: 4

            Console.WriteLine(myHashTable.Contains("two"));  // Output: True
            Console.WriteLine(myHashTable.GetValue("three"));  // Output: 3
            Console.WriteLine(myHashTable.Size());  // Output: 4

            myHashTable.Delete("two");
            myHashTable.Traverse();
            // Output:
            // Key: one, Value: 1
            // Key: three, Value: 3
            // Key: four, Value: 4

            HashTable<string, int>.Iterator iterator = myHashTable.GetIterator();
            while (iterator.HasNext())
            {
                KeyValuePair<string, int> pair = iterator.Next();
                Console.WriteLine($"Key: {pair.Key}, Value: {pair.Value}");
            }
            // Output:
            // Key: one, Value: 1
            // Key: three, Value: 3
            // Key: four, Value: 4
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
