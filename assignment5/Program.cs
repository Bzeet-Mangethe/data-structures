using System;
using System.Collections.Generic;

public class HashTable<TKey, TValue>
{
    private const int DefaultCapacity = 16;
    private const float DefaultLoadFactor = 0.75f;

    private LinkedList<KeyValuePair<TKey, TValue>>[] buckets;
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

        buckets = new LinkedList<KeyValuePair<TKey, TValue>>[capacity];
        for (int i = 0; i < capacity; i++)
        {
            buckets[i] = new LinkedList<KeyValuePair<TKey, TValue>>();
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
        buckets[bucketIndex].AddLast(new KeyValuePair<TKey, TValue>(key, value));
        count++;

        if (count > threshold)
        {
            Resize();
        }
    }

    public bool Delete(TKey key)
    {
        int bucketIndex = GetBucketIndex(key);
        LinkedList<KeyValuePair<TKey, TValue>> bucket = buckets[bucketIndex];
        foreach (KeyValuePair<TKey, TValue> pair in bucket)
        {
            if (EqualityComparer<TKey>.Default.Equals(pair.Key, key))
            {
                bucket.Remove(pair);
                count--;
                return true;
            }
        }
        return false;
    }

    public bool Contains(TKey key)
    {
        int bucketIndex = GetBucketIndex(key);
        LinkedList<KeyValuePair<TKey, TValue>> bucket = buckets[bucketIndex];
        foreach (KeyValuePair<TKey, TValue> pair in bucket)
        {
            if (EqualityComparer<TKey>.Default.Equals(pair.Key, key))
            {
                return true;
            }
        }
        return false;
    }

    public TValue GetValue(TKey key)
    {
        int bucketIndex = GetBucketIndex(key);
        LinkedList<KeyValuePair<TKey, TValue>> bucket = buckets[bucketIndex];
        foreach (KeyValuePair<TKey, TValue> pair in bucket)
        {
            if (EqualityComparer<TKey>.Default.Equals(pair.Key, key))
            {
                return pair.Value;
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
        foreach (LinkedList<KeyValuePair<TKey, TValue>> bucket in buckets)
        {
            foreach (KeyValuePair<TKey, TValue> pair in bucket)
            {
                Console.WriteLine($"Key: {pair.Key}, Value: {pair.Value}");
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
        LinkedList<KeyValuePair<TKey, TValue>>[] newBuckets = new LinkedList<KeyValuePair<TKey, TValue>>[newCapacity];
        for (int i = 0; i < newCapacity; i++)
        {
            newBuckets[i] = new LinkedList<KeyValuePair<TKey, TValue>>();
        }

        foreach (LinkedList<KeyValuePair<TKey, TValue>> bucket in buckets)
        {
            foreach (KeyValuePair<TKey, TValue> pair in bucket)
            {
                int newBucketIndex = pair.Key.GetHashCode() % newCapacity;
                if (newBucketIndex < 0)
                {
                    newBucketIndex += newCapacity;
                }
                newBuckets[newBucketIndex].AddLast(pair);
            }
        }

        buckets = newBuckets;
        threshold = (int)(newCapacity * loadFactor);
    }

    public class Iterator
    {
        private HashTable<TKey, TValue> hashTable;
        private int bucketIndex;
        private IEnumerator<KeyValuePair<TKey, TValue>> enumerator;

        public Iterator(HashTable<TKey, TValue> hashTable)
        {
            this.hashTable = hashTable;
            bucketIndex = 0;
            enumerator = null;
        }

        public bool HasNext()
        {
            if (enumerator == null || !enumerator.MoveNext())
            {
                while (bucketIndex < hashTable.buckets.Length)
                {
                    LinkedList<KeyValuePair<TKey, TValue>> bucket = hashTable.buckets[bucketIndex];
                    if (bucket.Count > 0)
                    {
                        enumerator = bucket.GetEnumerator();
                        return enumerator.MoveNext();
                    }
                    bucketIndex++;
                }
                return false;
            }
            return true;
        }

        public KeyValuePair<TKey, TValue> Next()
        {
            if (!HasNext())
            {
                throw new InvalidOperationException("No next element");
            }
            return enumerator.Current;
        }
    }
}

public class Program
{
    public static void Main(string[] args)
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
}

