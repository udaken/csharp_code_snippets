using System;
using System.Collections.Generic;

namespace Collections
{
    public class RingBuffer<T> : ICollection<T>, IEnumerable<T>
    {
        private T[] backBuffer;

        private int head = 0;
        private int tail = 0;
        private int count = 0;

        public event Action<T> ElementDischarged;

        public RingBuffer(int capacity)
        {
            if (capacity <= 0)
                throw new ArgumentOutOfRangeException();

            this.backBuffer = new T[capacity];
        }

        public RingBuffer(int capacity, IEnumerable<T> initial)
            : this(capacity)
        {
            if (initial == null)
                throw new ArgumentNullException();

            foreach (var elem in initial)
            {
                Add(elem);
            }
        }

        public T this[int index]
        {
            get
            {
                if (index < 0)
                    throw new ArgumentOutOfRangeException();

                if (index >= Count)
                    throw new ArgumentOutOfRangeException();

                return backBuffer[(head + index) % Capacity];
            }
        }
        
        public void Add(T item)
        {
            if (Count == Capacity && ElementDischarged != null)
                ElementDischarged(backBuffer[tail]);

            backBuffer[tail] = item;
            tail = (tail + 1) % Capacity;

            if (Count == Capacity)
                head = tail;
            if (Count < Capacity)
                count++;
        }

        public int Capacity
        {
            get { return backBuffer.Length; }
        }

        public void Clear()
        {
            Array.Clear(backBuffer, 0, backBuffer.Length);
            this.count = 0;
        }

        public bool Contains(T item)
        {
            return (Array.IndexOf<T>(backBuffer, item) != -1);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException();

            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException();

            if (array.Length - arrayIndex < Count)
                throw new ArgumentException();

            int mid = Count - head;
            Array.Copy(backBuffer, head, array, arrayIndex, mid);

            Array.Copy(backBuffer, 0, array, arrayIndex + mid, Count - mid);
        }

        public int Count
        {
            get { return count; }
        }

        public bool IsReadOnly
        {
            get { return backBuffer.IsReadOnly; }
        }

        public T Pop()
        {
            if (Count == 0)
                throw new InvalidOperationException();

            var n = (tail - 1) % Capacity;

            T element = backBuffer[n];
            tail = n;
            count--;

            return element;
        }
        public T PopFirst()
        {
            if (Count == 0)
                throw new InvalidOperationException();

            T element = backBuffer[head];
            head--;
            count--;

            return element;
        }
        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            int mid = Count;
            for (int i = head; i < Capacity && 0 < mid; i++, mid--)
            {
                yield return backBuffer[i];
            }

            for (int i = 0; i < head && 0 < mid; i++, mid--)
            {
                yield return backBuffer[i];
            }
        }
        public void Resize(int newCapacity)
        {
            if (newCapacity < this.Capacity)
            {
                throw new ArgumentOutOfRangeException();
            }

            var newBuffer = new T[newCapacity];

            CopyTo(newBuffer, 0);
            backBuffer = newBuffer;
            head = 0;
            tail = Count;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}