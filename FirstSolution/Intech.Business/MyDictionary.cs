using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intech.Business
{
    public class MyDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        IEqualityComparer<TKey> _strategy;
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
            _strategy = EqualityComparer<TKey>.Default;
        }
        public MyDictionary(IEqualityComparer<TKey> strat)
        {
            if (strat == null) throw new ArgumentException("null strat");
            _strategy = strat;
            _buckets = new Bucket[11];
        }
        public int Count
        {
            get { return _count; }
        }
        public IEnumerable<TValue> Values
        {
            get
            {
                return new EValue(this);
            }
        }
        public IEnumerable<TKey> Keys
        {
            get
            {
                return new EKey(this);
            }
        }
        public void Add(TKey key, TValue value)
        {
            AddOrReplace(key, value, true);
        }
        public bool Remove (TKey key)
        {
            int h = _strategy.GetHashCode(key);
            int slot = Math.Abs(h % _buckets.Length);
            Bucket b = _buckets[slot];
            Bucket prev = null;
            while(b != null)
            {
                if (_strategy.Equals(b.Key, key))
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
        void AddOrReplace(TKey key, TValue value, bool add)
        {
            int h = _strategy.GetHashCode(key);
            int slot = Math.Abs(h % _buckets.Length);
            Bucket b = _buckets[slot];
            if (b == null)
            {
                //AddNewBucket(slot, key, value);
                _buckets[slot] = new Bucket(key, value, null);
                _count++;
            }
            else
            {
                do
                {
                    if (_strategy.Equals(key, b.Key))
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
                    int h = _strategy.GetHashCode(b.Key);
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
            int h = _strategy.GetHashCode(key);
            int slot = Math.Abs(h % _buckets.Length);
            Bucket b = _buckets[slot];

            while(b != null)
            {
                if (_strategy.Equals(b.Key, key))
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
        class E
        {
            protected readonly MyDictionary<TKey, TValue> _dico;
            int _currentVerticalPosition;
            Bucket _currentBucket;
            public E(MyDictionary<TKey, TValue> d)
            {
                _dico = d;
                _currentVerticalPosition = -1;
            }
            protected Bucket CurrentBucket
            {
                get
                {
                    if (_currentVerticalPosition < 0) throw new InvalidOperationException("MoveNext() must be called first");
                    if (_currentVerticalPosition >= _dico._buckets.Length) throw new InvalidOperationException("Current must not be used if MoveNext() returned false");

                    return _currentBucket;
                }
            }
            public void Dispose()
            {
            }
            public bool MoveNext()
            {
                if (_currentBucket != null)
                    _currentBucket = _currentBucket.Next;

                while (_currentBucket == null)
                {
                    if (++_currentVerticalPosition >= _dico._buckets.Length)
                        return false;
                    _currentBucket = _dico._buckets[_currentVerticalPosition];
                }

                Debug.Assert(_currentBucket != null);
                
                return true;
            }
            public void Reset()
            {
                throw new NotSupportedException();
            }
        }
        class EPair : E, IEnumerator<KeyValuePair<TKey, TValue>>
        {
            public EPair(MyDictionary<TKey, TValue> dico) : base (dico)
            {

            }
            public KeyValuePair<TKey, TValue> Current
            {
                get
                {
                    return new KeyValuePair<TKey, TValue>(base.CurrentBucket.Key, base.CurrentBucket.Value);
                }
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }
        }
        class EKey : E, IEnumerator<TKey>, IEnumerable<TKey>
        {
            public EKey(MyDictionary<TKey, TValue> dico)
                : base(dico)
            {

            }
            public TKey Current
            {
                get
                {
                    return base.CurrentBucket.Key;
                }
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            public IEnumerator<TKey> GetEnumerator()
            {
                return new EKey(_dico);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
        class EValue : E, IEnumerator<TValue>, IEnumerable<TValue>
        {
            public EValue(MyDictionary<TKey, TValue> dico)
                : base(dico)
            {

            }
            public TValue Current
            {
                get
                {
                    return base.CurrentBucket.Value;
                }
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            public IEnumerator<TValue> GetEnumerator()
            {
                return new EValue(_dico);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return new EPair(this);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
