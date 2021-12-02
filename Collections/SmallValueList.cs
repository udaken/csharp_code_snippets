using System;
using System.Collections;
using System.Collections.Generic;

namespace MarkdownWpfApp
{
    struct SmallValueList<T> : IList<T>
    {
        ValueTuple<T, T, T, T, T, T, T> _tuple;
        const int N = 7;
        byte _count;

        public T this[int index]
        {
            get
            {
                if (index >= _count)
                    throw new ArgumentOutOfRangeException();

                switch (index)
                {
                    case 0:
                        return _tuple.Item1;
                    case 1:
                        return _tuple.Item2;
                    case 2:
                        return _tuple.Item3;
                    case 3:
                        return _tuple.Item4;
                    case 4:
                        return _tuple.Item5;
                    case 5:
                        return _tuple.Item6;
                    case 6:
                        return _tuple.Item7;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            set
            {
                if (index >= _count)
                    throw new ArgumentOutOfRangeException();

                switch (index)
                {
                    case 0:
                        _tuple.Item1 = value;
                        break;
                    case 1:
                        _tuple.Item2 = value;
                        break;
                    case 2:
                        _tuple.Item3 = value;
                        break;
                    case 3:
                        _tuple.Item4 = value;
                        break;
                    case 4:
                        _tuple.Item5 = value;
                        break;
                    case 5:
                        _tuple.Item6 = value;
                        break;
                    case 6:
                        _tuple.Item7 = value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public int Count => _count;

        public bool IsReadOnly => false;

        public void Add(T item)
        {
            if (N <= Count)
                throw new InvalidOperationException();
            _count++;
            this[Count] = item;
        }
        public void Clear()
        {
            _count = 0;
            _tuple.Item1 = default(T);
            _tuple.Item2 = default(T);
            _tuple.Item3 = default(T);
            _tuple.Item4 = default(T);
            _tuple.Item5 = default(T);
            _tuple.Item6 = default(T);
            _tuple.Item7 = default(T);
        }
        public bool Contains(T item) => IndexOf(item) >= 0;
        public void CopyTo(T[] array, int arrayIndex) => throw new NotImplementedException();

        public IEnumerator<T> GetEnumerator()
        {
            var i = 0;
            if (i >= Count)
                yield break;
            yield return this[i++];
            if (i >= Count)
                yield break;
            yield return this[i++];
            if (i >= Count)
                yield break;
            yield return this[i++];
            if (i >= Count)
                yield break;
            yield return this[i++];
            if (i >= Count)
                yield break;
            yield return this[i++];
            if (i >= Count)
                yield break;
            yield return this[i++];
            if (i >= Count)
                yield break;
            yield return this[i++];
        }
        public int IndexOf(T item)
        {
            var comparer = EqualityComparer<T>.Default;
            var i = 0;
            return
                i++ < Count && comparer.Equals(item, _tuple.Item1) ? i - 1 :
                i++ < Count && comparer.Equals(item, _tuple.Item2) ? i - 1 :
                i++ < Count && comparer.Equals(item, _tuple.Item3) ? i - 1 :
                i++ < Count && comparer.Equals(item, _tuple.Item4) ? i - 1 :
                i++ < Count && comparer.Equals(item, _tuple.Item5) ? i - 1 :
                i++ < Count && comparer.Equals(item, _tuple.Item6) ? i - 1 :
                i++ < Count && comparer.Equals(item, _tuple.Item7) ? i - 1 :
                -1;
        }
        public void Insert(int index, T item) => throw new NotImplementedException();
        public bool Remove(T item) => throw new NotImplementedException();
        public void RemoveAt(int index) => throw new NotImplementedException();
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}