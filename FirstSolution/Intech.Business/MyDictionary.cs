using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intech.Business
{
    public class MyDictionary<TKey, TValue>
    {
        int _count;
        Bucket[] _buckets;
        int _fillFactor = 5;
        int[] _primaryNumbers = new int[] { 11, 23, 47, 97, 211, 421};

        class Bucket
        {
            public readonly TKey Key;
            public TValue Value;
            public Bucket Next;

            public Bucket(TKey key, TValue value, Bucket next)
            {
                this.Key = key;
                this.Value = value;
                this.Next = next;
            }
        }

        public MyDictionary()
        {
            _buckets = new Bucket[11];
        }

        public int Count
        {
            get { return _count; }
        }
        public void Add(TKey key, TValue value)
        {
            AddOrReplace(key, value, true);
        }

        public bool Remove (TKey key)
        {
            int h = key.GetHashCode();
            int slot = Math.Abs(h % _buckets.Length);
            Bucket b = _buckets[slot];
            Bucket prev = null;
            while(b != null)
            {
                if (EqualityComparer<TKey>.Default.Equals(b.Key, key))
                {
                    _count--;
                    if (prev == null)
                    {
                        _buckets[slot] = b.Next;
                        return true;
                    }
                    prev.Next = b.Next;
                    return true;
                }
                prev = b;
                b = b.Next;
            }
            return false;
        }

        private void AddOrReplace(TKey key, TValue value, bool add)
        {
            int h = key.GetHashCode();
            int slot = Math.Abs(h % _buckets.Length);
            Bucket b = _buckets[slot];
            if (b == null)
            {
                _buckets[slot] = new Bucket(key, value, null);
                _count++;
            }
            else
            {
                do
                {
                    if (EqualityComparer<TKey>.Default.Equals(b.Key, key))
                    {
                        if (add) throw new InvalidOperationException();
                        b.Value = value;
                        return;
                    }
                    b = b.Next;
                }
                while (b != null);
                _count++;
                b = new Bucket(key, value, _buckets[slot]);
                _buckets[slot] = b;
                if (_count / _buckets.Length >= _fillFactor)
                    Grow();
            }
        }

        void Grow()
        {
            int nextLength = _primaryNumbers[Array.IndexOf(_primaryNumbers, _buckets.Length) + 1];
            var newBuckets = new Bucket[23];
            for (int i = 0; i < _buckets.Length; i++)
            {
                Bucket b = _buckets[i];
                while(b != null)
                {
                    int h = b.Key.GetHashCode();
                    int slot = Math.Abs(h % newBuckets.Length);
                    var oldNext = b.Next; 
                    b.Next = newBuckets[slot];
                    newBuckets[slot] = b;
                    b = oldNext;
                }
            }
            _buckets = newBuckets;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            int h = key.GetHashCode();
            int slot = Math.Abs(h % _buckets.Length);
            Bucket b = _buckets[slot];

            while(b != null)
            {
                if (EqualityComparer<TKey>.Default.Equals(b.Key, key))
                {
                    value = b.Value;
                    return true;
                }
                b = b.Next;
            }
            value = default(TValue);
            return false;
        }

        public TValue this[TKey key]
        {
            get 
            {
                TValue value;
                if(!TryGetValue(key, out value))
                {
                    throw new KeyNotFoundException();
                }
                return value;
            }
            set
            {
                AddOrReplace(key, value, false);
            }
        }
    }
}
