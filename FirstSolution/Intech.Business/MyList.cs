using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intech.Business
{
    public class MyList<T> : IEnumerable<T>
    {
        T[] _array;
        int _count;

        public MyList()
        {
            _array = new T[8];
        }

        public int Count
        {
            get { return _count; }
        }

        public void Add(T value)
        {
            if (_array.Length == _count)
            {
                Grow();
            }
            _array[_count++] = value;
        }

        public T this[int i]
        {
            get
            {
                if (i >= _count)
                    throw new IndexOutOfRangeException();
                return _array[i];
            }
            set
            {
                if (i >= _count)
                    throw new IndexOutOfRangeException();
                _array[i] = value;
            }
        }

        public void RemoveAt(int i)
        {
            if (i >= _count)
                throw new IndexOutOfRangeException();

            Array.Copy(_array, i + 1, _array, i, _count - (i + 1));

            _array[--_count] = default(T);
        }

        public void InsertAt(int i, T item)
        {
            if (i > _count)
                throw new IndexOutOfRangeException();

            if (_array.Length == _count)
            {
                Grow();
            }

            Array.Copy(_array, i, _array, i+1, _count++ - i);

            _array[i] = item;
        }

        public int IndexOf(T item)
        {
            for (int i = 0; i < _count; i++)
            {
                if (EqualityComparer<T>.Default.Equals(item, _array[i]))
                    return i;
            }

            return -1;
        }

        void Grow()
        {
            var newOne = new T[_array.Length * 2];
            Array.Copy(_array, newOne, _array.Length);
            _array = newOne;
        }

        // Nested type
        class E : IEnumerator<T>
        {
            readonly MyList<T> _list;
            int _currentPosition;

            public E(MyList<T> l)
            {
                _list = l;
                _currentPosition = -1;
            }

            public T Current
            {
                get 
                {
                    if (_currentPosition < 0) throw new InvalidOperationException("MoveNext() must be called first");
                    if (_currentPosition >= _list._count) throw new InvalidOperationException("Current must not be used if MoveNext() returned false");
                    return _list._array[_currentPosition]; 
                }
            }

            public void Dispose()
            {
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            public bool MoveNext()
            {
                return (++_currentPosition >= _list._count);
            }

            public void Reset()
            {
                throw new NotSupportedException();
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new E(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
