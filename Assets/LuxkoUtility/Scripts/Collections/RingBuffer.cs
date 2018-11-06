using System;
using System.Collections;
using System.Collections.Generic;

namespace Luxko.Collections
{
    public class RingBuffer<T> : IEnumerable<T>
    {
        public int Count { get; private set; }
        public int Capacity
        {
            get { return _buffer.Length; }
            set
            {
                if (value <= Count) return;
                //Array.Resize(ref _buffer, value);
                var newBuffer = new T[value];
                for (int i = 0; i < Count; ++i)
                {
                    newBuffer[i] = this[i];
                }
                _tail = Count;
                _buffer = newBuffer;
            }
        }
        T[] _buffer;
        int _tail;

        public RingBuffer() : this(8) { }
        public RingBuffer(int capacity)
        {
            _buffer = new T[capacity];
            _tail = 0;
            Count = 0;
        }

        [System.Runtime.CompilerServices.MethodImpl(256)]
        int AcutalIndex(int index)
        {
            var i = (_tail - Count + index) % _buffer.Length;
            if (i < 0) i += _buffer.Length;
            return i;
        }

        public void PushBack(T v)
        {
            _buffer[_tail] = v;
            _tail = (_tail + 1) % _buffer.Length;
            if (Count < _buffer.Length) Count = Count + 1;
        }

        public T PopBack()
        {
            if (Count <= 0) throw new ArgumentOutOfRangeException();
            return _buffer[AcutalIndex(--Count)];
        }

        public void PushFront(T v)
        {
            _buffer[AcutalIndex(-1)] = v;
            if (Count < _buffer.Length) Count = Count + 1;
        }

        public T PopFront()
        {
            if (Count <= 0) throw new ArgumentOutOfRangeException();
            var front = _buffer[AcutalIndex(0)];
            Count = Count - 1;
            return front;
        }

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= Count) throw new ArgumentOutOfRangeException("index");
                return _buffer[AcutalIndex(index)];
            }
            set
            {
                if (index < 0 || index >= Count) throw new ArgumentOutOfRangeException("index");
                _buffer[AcutalIndex(index)] = value;
            }
        }

        public void Clear()
        {
            if (_tail - Count >= 0)
            {
                System.Array.Clear(_buffer, _tail - Count, Count);
            }
            else
            {
                var from = AcutalIndex(0);
                var length = _buffer.Length - from;
                System.Array.Clear(_buffer, from, length);
                length = Count - length;
                System.Array.Clear(_buffer, _tail, length);
            }
            Count = 0;
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            for (int i = 0; i < Count; ++i)
            {
                yield return this[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (this as IEnumerable<T>).GetEnumerator();
        }
    }
}
