using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace TableCollections
{
    public class IndexedTable<TValue> : IEnumerable<TValue>
    {
        protected IList<IDictionary<object, ISet<int>>> _indices;
        protected IList<TValue> _data;
        protected IList<ISet<int>> _collapsedKeys;

        protected IndexedTable()
        {
            _indices = new List<IDictionary<object, ISet<int>>>();
            _data = new List<TValue>();
            _collapsedKeys = new List<ISet<int>>();
        }

        protected internal IndexedTable(IList<IDictionary<object, ISet<int>>> indices,
            IList<TValue> data, IList<ISet<int>> collapsedKeys)
        {
            _indices = indices;
            _data = data;
            _collapsedKeys = collapsedKeys;
        }

        public int Count => GetCollapsedIndexsetOrDefault()?.Count ?? _data.Count;

        /// <summary>
        /// Sets *all* values in the table (respectively the slice) to the given value
        /// </summary>
        /// <param name="value"></param>
        public void Set(TValue value)
        {
            var collapsedIndices = GetCollapsedIndexsetOrDefault();
            if (collapsedIndices == null)
            {
                // set all values
                for (var i = 0; i < _data.Count; i++)
                {
                    _data[i] = value;
                }
            }
            else
            {
                // set those values that are in the slice
                foreach (var index in collapsedIndices)
                    _data[index] = value;
            }
        }

        public bool Contains(params object[] keys)
        {
            ExceptionHandling.ThrowIfNull(keys, nameof(keys));
            if (keys.Length != _indices.Count)
                throw new ArgumentException($"Expected {nameof(keys)} to have {_indices.Count} elements");

            var indices = GetCollapsedIndexsetOrDefault();

            for (var i = 0; i < keys.Length; i++)
            {
                if (!_indices[i].TryGetValue(keys[i], out var ind))
                    return false;
                if (indices == null)
                {
                    indices = new HashSet<int>(ind);
                }
                else
                {
                    indices.IntersectWith(ind);
                }
                if (indices.Count == 0) return false;
            }
            return true;
        }
        
#if NET472 || NET481
        public ISet<int> GetCollapsedIndexsetOrDefault()
        {
            HashSet<int> collapsedIndices = null;
            if (_collapsedKeys.Count > 0)
            {
                collapsedIndices = new HashSet<int>(_collapsedKeys.First());
                foreach (var collapsedSet in _collapsedKeys.Skip(1))
                    collapsedIndices.IntersectWith(collapsedSet);
            }
            return collapsedIndices;
        }
#else
        public ISet<int>? GetCollapsedIndexsetOrDefault()
        {
            HashSet<int>? collapsedIndices = null;
            if (_collapsedKeys.Count > 0)
            {
                collapsedIndices = new HashSet<int>(_collapsedKeys.First());
                foreach (var collapsedSet in _collapsedKeys.Skip(1))
                    collapsedIndices.IntersectWith(collapsedSet);
            }
            return collapsedIndices;
        }
#endif
        
        public IEnumerable<TValue> EnumerateValues()
        {
            var collapsedIndices = GetCollapsedIndexsetOrDefault();
            if (collapsedIndices == null) return _data;
            return collapsedIndices.Select(f => _data[f]);
        }

        IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
        {
            return EnumerateValues().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return EnumerateValues().GetEnumerator();
        }
    }

    public class IndexedTable<T1, T2, T3, T4, T5, TValue> : IndexedTable<TValue>
#if !(NET472 || NET481)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
#endif
    {
        protected internal IndexedTable(IList<IDictionary<object, ISet<int>>> indices,
            IList<TValue> data, IList<ISet<int>> collapsedKeys) : base(indices, data, collapsedKeys) { }
        public IndexedTable() : this(new KeyValuePair<(T1, T2, T3, T4, T5), TValue>[0]) { }
        public IndexedTable(IEnumerable<KeyValuePair<(T1, T2, T3, T4, T5), TValue>> data)
        {
            for (var i = 0; i < 5; i++)
                _indices.Add(new Dictionary<object, ISet<int>>());

            var idx = -1;
            foreach (var d in data)
            {
                _data.Insert(++idx, d.Value);
                var key = (ITuple)d.Key;
                for (var i = 0; i < key.Length; i++)
                {
                    if (!_indices[i].TryGetValue(key[i], out var ind))
                    {
                        ind = new HashSet<int>();
                        _indices[i][key[i]] = ind;
                    }
                    ind.Add(idx);
                }
            }
        }
        public IndexedTable(IEnumerable<(T1, T2, T3, T4, T5, TValue)> data)
        {
            for (var i = 0; i < 5; i++)
                _indices.Add(new Dictionary<object, ISet<int>>());

            var idx = -1;
            foreach (var d in data)
            {
                _data.Insert(++idx, d.Item6);
                var key = (ITuple)d;
                for (var i = 0; i < key.Length - 1; i++)
                {
                    if (!_indices[i].TryGetValue(key[i], out var ind))
                    {
                        ind = new HashSet<int>();
                        _indices[i][key[i]] = ind;
                    }
                    ind.Add(idx);
                }
            }
        }

        public TValue this[T1 key1, T2 key2, T3 key3, T4 key4, T5 key5]
        {
            get
            {
                ExceptionHandling.ThrowIfNull(key1, nameof(key1));
                ExceptionHandling.ThrowIfNull(key2, nameof(key2));
                ExceptionHandling.ThrowIfNull(key3, nameof(key3));
                ExceptionHandling.ThrowIfNull(key4, nameof(key4));
                ExceptionHandling.ThrowIfNull(key5, nameof(key5));
                if (!_indices[0].TryGetValue(key1, out var indices1))
                    throw new ArgumentException($"Key not found {key1}");
                if (!_indices[1].TryGetValue(key2, out var indices2))
                    throw new ArgumentException($"Key not found {key2}");
                if (!_indices[2].TryGetValue(key3, out var indices3))
                    throw new ArgumentException($"Key not found {key3}");
                if (!_indices[3].TryGetValue(key4, out var indices4))
                    throw new ArgumentException($"Key not found {key4}");
                if (!_indices[4].TryGetValue(key5, out var indices5))
                    throw new ArgumentException($"Key not found {key5}");
                var index = -1;
                var indices = GetCollapsedIndexsetOrDefault();
                if (indices != null)
                {
                    indices.IntersectWith(indices1);
                    indices.IntersectWith(indices2);
                    indices.IntersectWith(indices3);
                    indices.IntersectWith(indices4);
                    indices.IntersectWith(indices5);
                    index = indices.Single();
                }
                else
                {
                    index = indices1.Intersect(indices2).Intersect(indices3).Intersect(indices4).Intersect(indices5).Single();
                }
                return _data[index];
            }
            set
            {
                ExceptionHandling.ThrowIfNull(key1, nameof(key1));
                ExceptionHandling.ThrowIfNull(key2, nameof(key2));
                ExceptionHandling.ThrowIfNull(key3, nameof(key3));
                ExceptionHandling.ThrowIfNull(key4, nameof(key4));
                ExceptionHandling.ThrowIfNull(key5, nameof(key5));
                var tuple = (ITuple)(key1, key2, key3, key4, key5);
                var index = -1;
                var indices = GetCollapsedIndexsetOrDefault();
                if (indices != null)
                {
                    for (var i = 0; i < tuple.Length; i++)
                    {
                        if (!_indices[i].TryGetValue(tuple[i], out var ind))
                        {
                            ind = new HashSet<int>();
                            _indices[i][tuple[i]] = ind;
                        }
                        indices.IntersectWith(ind);
                    }
                    if (indices.Count > 0)
                        index = indices.Single();
                }
                else
                {
                    IEnumerable<int> filtered = null;
                    for (var i = 0; i < tuple.Length; i++)
                    {
                        if (!_indices[i].TryGetValue(tuple[i], out var ind))
                        {
                            ind = new HashSet<int>();
                            _indices[i][tuple[i]] = ind;
                        }
                        filtered = filtered == null ? ind : filtered.Intersect(ind);
                    }
                    foreach (var i in filtered ?? Enumerable.Empty<int>())
                    {
                        if (index != -1) throw new InvalidOperationException(); // Single() for non-negative enumerable
                        index = i;
                    }
                }
                if (index < 0)
                {
                    index = _data.Count;
                    _data.Insert(index, value);
                    for (var i = 0; i < tuple.Length; i++)
                    {
                        _indices[i][tuple[i]].Add(index);
                    }
                    // the data needs to be added to all collapsed dimensions
                    foreach (var f in _collapsedKeys)
                    {
                        f.Add(index);
                    }
                }
                else
                {
                    _data[index] = value;
                }
            }
        }

        public TValue this[(T1 key1, T2 key2, T3 key3, T4 key4, T5 key5) keys]
        {
            get => this[keys.key1, keys.key2, keys.key3, keys.key4, keys.key5];
            set => this[keys.key1, keys.key2, keys.key3, keys.key4, keys.key5] = value;
        }

        public void Add(T1 key1, T2 key2, T3 key3, T4 key4, T5 key5, TValue value)
        {
            if (Contains(key1, key2, key3, key4, key5)) throw new ArgumentException($"Key already exists ({key1}, {key2}, {key3}, {key4}, {key5})");
            this[key1, key2, key3, key4, key5] = value;
        }

        public void Add((T1 key1, T2 key2, T3 key3, T4 key4, T5 key5) keys, TValue value) => Add(keys.key1, keys.key2, keys.key3, keys.key4, keys.key5, value);

        public bool TryGetValue(T1 key1, T2 key2, T3 key3, T4 key4, T5 key5, out TValue? value)
        {
            value = default;
            if (!_indices[0].TryGetValue(key1, out var indices1))
                return false;
            if (!_indices[1].TryGetValue(key2, out var indices2))
                return false;
            if (!_indices[2].TryGetValue(key3, out var indices3))
                return false;
            if (!_indices[3].TryGetValue(key4, out var indices4))
                return false;
            if (!_indices[4].TryGetValue(key5, out var indices5))
                return false;

            var indices = GetCollapsedIndexsetOrDefault();
            if (indices == null) indices = new HashSet<int>(indices1);
            else indices.IntersectWith(indices1);
            indices.IntersectWith(indices2);
            indices.IntersectWith(indices3);          
            indices.IntersectWith(indices4);
            indices.IntersectWith(indices5);
            if (indices.Count != 1) return false;
            value = _data[indices.Single()];
            return true;
        }

        public IndexedTable<T2, T3, T4, T5, TValue> Slice1(T1 key1)
        {
            ExceptionHandling.ThrowIfNull(key1, nameof(key1));
            if (!_indices[0].TryGetValue(key1, out var ind))
            {
                throw new ArgumentException($"Key not found {key1}");
            }
            return new IndexedTable<T2, T3, T4, T5, TValue>(
                new[] { _indices[1], _indices[2], _indices[3], _indices[4] },
                _data, _collapsedKeys.Concat(new[] { ind }).ToList());
        }

        public IndexedTable<T1, T3, T4, T5, TValue> Slice2(T2 key2)
        {
            ExceptionHandling.ThrowIfNull(key2, nameof(key2));
            if (!_indices[1].TryGetValue(key2, out var ind))
            {
                throw new ArgumentException($"Key not found {key2}");
            }
            return new IndexedTable<T1, T3, T4, T5, TValue>(
                new[] { _indices[0], _indices[2], _indices[3], _indices[4] },
                _data, _collapsedKeys.Concat(new[] { ind }).ToList());
        }

        public IndexedTable<T1, T2, T4, T5, TValue> Slice3(T3 key3)
        {
            ExceptionHandling.ThrowIfNull(key3, nameof(key3));
            if (!_indices[2].TryGetValue(key3, out var ind))
            {
                throw new ArgumentException($"Key not found {key3}");
            }
            return new IndexedTable<T1, T2, T4, T5, TValue>(
                new[] { _indices[0], _indices[1], _indices[3], _indices[4] },
                _data, _collapsedKeys.Concat(new[] { ind }).ToList());
        }

        public IndexedTable<T1, T2, T3, T5, TValue> Slice4(T4 key4)
        {
            ExceptionHandling.ThrowIfNull(key4, nameof(key4));
            if (!_indices[3].TryGetValue(key4, out var ind))
            {
                throw new ArgumentException($"Key not found {key4}");
            }
            return new IndexedTable<T1, T2, T3, T5, TValue>(
                new[] { _indices[0], _indices[1], _indices[2], _indices[4] },
                _data, _collapsedKeys.Concat(new[] { ind }).ToList());
        }

        public IndexedTable<T1, T2, T3, T4, TValue> Slice5(T5 key5)
        {
            ExceptionHandling.ThrowIfNull(key5, nameof(key5));
            if (!_indices[4].TryGetValue(key5, out var ind))
            {
                throw new ArgumentException($"Key not found {key5}");
            }
            return new IndexedTable<T1, T2, T3, T4, TValue>(
                new[] { _indices[0], _indices[1], _indices[2], _indices[3] },
                _data, _collapsedKeys.Concat(new[] { ind }).ToList());
        }

        public bool ContainsKey1(T1 key1)
        {
            ExceptionHandling.ThrowIfNull(key1, nameof(key1));
            if (!_indices[0].TryGetValue(key1, out var ind))
                return false;

            var indices = GetCollapsedIndexsetOrDefault();
            if (indices == null)
            {
                return true; // otherwise, we'd have returned false above
            }
            else
            {
                indices.IntersectWith(ind);
            }
            if (indices.Count == 0) return false;
            return true;
        }

        public bool ContainsKey2(T2 key2)
        {
            ExceptionHandling.ThrowIfNull(key2, nameof(key2));
            if (!_indices[1].TryGetValue(key2, out var ind))
                return false;

            var indices = GetCollapsedIndexsetOrDefault();
            if (indices == null)
            {
                return true; // otherwise, we'd have returned false above
            }
            else
            {
                indices.IntersectWith(ind);
            }
            if (indices.Count == 0) return false;
            return true;
        }

        public bool ContainsKey3(T3 key3)
        {
            ExceptionHandling.ThrowIfNull(key3, nameof(key3));
            if (!_indices[2].TryGetValue(key3, out var ind))
                return false;

            var indices = GetCollapsedIndexsetOrDefault();
            if (indices == null)
            {
                return true; // otherwise, we'd have returned false above
            }
            else
            {
                indices.IntersectWith(ind);
            }
            if (indices.Count == 0) return false;
            return true;
        }

        public bool ContainsKey4(T4 key4)
        {
            ExceptionHandling.ThrowIfNull(key4, nameof(key4));
            if (!_indices[3].TryGetValue(key4, out var ind))
                return false;

            var indices = GetCollapsedIndexsetOrDefault();
            if (indices == null)
            {
                return true; // otherwise, we'd have returned false above
            }
            else
            {
                indices.IntersectWith(ind);
            }
            if (indices.Count == 0) return false;
            return true;
        }

        public bool ContainsKey5(T5 key5)
        {
            ExceptionHandling.ThrowIfNull(key5, nameof(key5));
            if (!_indices[4].TryGetValue(key5, out var ind))
                return false;

            var indices = GetCollapsedIndexsetOrDefault();
            if (indices == null)
            {
                return true; // otherwise, we'd have returned false above
            }
            else
            {
                indices.IntersectWith(ind);
            }
            if (indices.Count == 0) return false;
            return true;
        }

        public bool TrySlice1(T1 key1, out IndexedTable<T2, T3, T4, T5, TValue> values)
        {
            ExceptionHandling.ThrowIfNull(key1, nameof(key1));
            if (_indices[0].TryGetValue(key1, out var indices))
            {
                values = new IndexedTable<T2, T3, T4, T5, TValue>(new[] { _indices[1], _indices[2], _indices[3], _indices[4] },
                _data, _collapsedKeys.Concat(new[] { indices }).ToList());
                return true;
            }
            values = new IndexedTable<T2, T3, T4, T5, TValue>();
            return false;
        }

        public bool TrySlice2(T2 key2, out IndexedTable<T1, T3, T4, T5, TValue> values)
        {
            ExceptionHandling.ThrowIfNull(key2, nameof(key2));
            if (_indices[1].TryGetValue(key2, out var indices))
            {
                values = new IndexedTable<T1, T3, T4, T5, TValue>(new[] { _indices[0], _indices[2], _indices[3], _indices[4] },
                _data, _collapsedKeys.Concat(new[] { indices }).ToList());
                return true;
            }
            values = new IndexedTable<T1, T3, T4, T5, TValue>();
            return false;
        }

        public bool TrySlice3(T3 key3, out IndexedTable<T1, T2, T4, T5, TValue> values)
        {
            ExceptionHandling.ThrowIfNull(key3, nameof(key3));
            if (_indices[2].TryGetValue(key3, out var indices))
            {
                values = new IndexedTable<T1, T2, T4, T5, TValue>(new[] { _indices[0], _indices[1], _indices[3], _indices[4] },
                _data, _collapsedKeys.Concat(new[] { indices }).ToList());
                return true;
            }
            values = new IndexedTable<T1, T2, T4, T5, TValue>();
            return false;
        }

        public bool TrySlice4(T4 key4, out IndexedTable<T1, T2, T3, T5, TValue> values)
        {
            ExceptionHandling.ThrowIfNull(key4, nameof(key4));
            if (_indices[3].TryGetValue(key4, out var indices))
            {
                values = new IndexedTable<T1, T2, T3, T5, TValue>(new[] { _indices[0], _indices[1], _indices[2], _indices[4] },
                _data, _collapsedKeys.Concat(new[] { indices }).ToList());
                return true;
            }
            values = new IndexedTable<T1, T2, T3, T5, TValue>();
            return false;
        }

        public bool TrySlice5(T5 key5, out IndexedTable<T1, T2, T3, T4, TValue> values)
        {
            ExceptionHandling.ThrowIfNull(key5, nameof(key5));
            if (_indices[4].TryGetValue(key5, out var indices))
            {
                values = new IndexedTable<T1, T2, T3, T4, TValue>(new[] { _indices[0], _indices[1], _indices[2], _indices[3] },
                _data, _collapsedKeys.Concat(new[] { indices }).ToList());
                return true;
            }
            values = new IndexedTable<T1, T2, T3, T4, TValue>();
            return false;
        }

        public IEnumerable<(T1, T2, T3, T4, T5, TValue)> Enumerate()
        {
            var collapsedIndices = GetCollapsedIndexsetOrDefault();
            IEnumerable<(T1, int)> result1 = (collapsedIndices != null
                ? _indices[0].Select(x => x.Value.Where(y => collapsedIndices.Contains(y)).Select(y => ((T1)x.Key, y))).SelectMany(x => x)
                : _indices[0].Select(x => x.Value.Select(y => ((T1)x.Key, y))).SelectMany(x => x)).OrderBy(x => x.Item2);
            IEnumerable<(T2, int)> result2 = (collapsedIndices != null
                ? _indices[1].Select(x => x.Value.Where(y => collapsedIndices.Contains(y)).Select(y => ((T2)x.Key, y))).SelectMany(x => x)
                : _indices[1].Select(x => x.Value.Select(y => ((T2)x.Key, y))).SelectMany(x => x)).OrderBy(x => x.Item2);
            IEnumerable<(T3, int)> result3 = (collapsedIndices != null
                ? _indices[2].Select(x => x.Value.Where(y => collapsedIndices.Contains(y)).Select(y => ((T3)x.Key, y))).SelectMany(x => x)
                : _indices[2].Select(x => x.Value.Select(y => ((T3)x.Key, y))).SelectMany(x => x)).OrderBy(x => x.Item2);
            IEnumerable<(T4, int)> result4 = (collapsedIndices != null
                ? _indices[3].Select(x => x.Value.Where(y => collapsedIndices.Contains(y)).Select(y => ((T4)x.Key, y))).SelectMany(x => x)
                : _indices[3].Select(x => x.Value.Select(y => ((T4)x.Key, y))).SelectMany(x => x)).OrderBy(x => x.Item2);
            IEnumerable<(T5, int)> result5 = (collapsedIndices != null
                ? _indices[4].Select(x => x.Value.Where(y => collapsedIndices.Contains(y)).Select(y => ((T5)x.Key, y))).SelectMany(x => x)
                : _indices[4].Select(x => x.Value.Select(y => ((T5)x.Key, y))).SelectMany(x => x)).OrderBy(x => x.Item2);
            var r1Iter = result1.GetEnumerator();
            var r2Iter = result2.GetEnumerator();
            var r3Iter = result3.GetEnumerator();
            var r4Iter = result4.GetEnumerator();
            var r5Iter = result5.GetEnumerator();
            while (r1Iter.MoveNext())
            {
#if DEBUG
                if (!r2Iter.MoveNext()) throw new InvalidOperationException();
                if (r2Iter.Current.Item2 != r1Iter.Current.Item2) throw new InvalidOperationException();
                if (!r3Iter.MoveNext()) throw new InvalidOperationException();
                if (r3Iter.Current.Item2 != r1Iter.Current.Item2) throw new InvalidOperationException();
                if (!r4Iter.MoveNext()) throw new InvalidOperationException();
                if (r4Iter.Current.Item2 != r1Iter.Current.Item2) throw new InvalidOperationException();
                if (!r5Iter.MoveNext()) throw new InvalidOperationException();
                if (r5Iter.Current.Item2 != r1Iter.Current.Item2) throw new InvalidOperationException();
#else
                r2Iter.MoveNext();
                r3Iter.MoveNext();
                r4Iter.MoveNext();
                r5Iter.MoveNext();
#endif
                yield return ((T1)r1Iter.Current.Item1, (T2)r2Iter.Current.Item1, (T3)r3Iter.Current.Item1, (T4)r4Iter.Current.Item1, (T5)r5Iter.Current.Item1, _data[r1Iter.Current.Item2]);
            }
        }
    }

    public class IndexedTable<T1, T2, T3, T4, TValue> : IndexedTable<TValue>
#if !(NET472 || NET481)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
#endif
    {
        protected internal IndexedTable(IList<IDictionary<object, ISet<int>>> indices,
            IList<TValue> data, IList<ISet<int>> collapsedKeys) : base(indices, data, collapsedKeys) { }
        public IndexedTable() : this(new KeyValuePair<(T1, T2, T3, T4), TValue>[0]) { }
        public IndexedTable(IEnumerable<KeyValuePair<(T1, T2, T3, T4), TValue>> data)
        {
            for (var i = 0; i < 4; i++)
                _indices.Add(new Dictionary<object, ISet<int>>());

            var idx = -1;
            foreach (var d in data)
            {
                _data.Insert(++idx, d.Value);
                var key = (ITuple)d.Key;
                for (var i = 0; i < key.Length; i++)
                {
                    if (!_indices[i].TryGetValue(key[i], out var ind))
                    {
                        ind = new HashSet<int>();
                        _indices[i][key[i]] = ind;
                    }
                    ind.Add(idx);
                }
            }
        }
        public IndexedTable(IEnumerable<(T1, T2, T3, T4, TValue)> data)
        {
            for (var i = 0; i < 4; i++)
                _indices.Add(new Dictionary<object, ISet<int>>());

            var idx = -1;
            foreach (var d in data)
            {
                _data.Insert(++idx, d.Item5);
                var key = (ITuple)d;
                for (var i = 0; i < key.Length - 1; i++)
                {
                    if (!_indices[i].TryGetValue(key[i], out var ind))
                    {
                        ind = new HashSet<int>();
                        _indices[i][key[i]] = ind;
                    }
                    ind.Add(idx);
                }
            }
        }

        public TValue this[T1 key1, T2 key2, T3 key3, T4 key4]
        {
            get
            {
                ExceptionHandling.ThrowIfNull(key1, nameof(key1));
                ExceptionHandling.ThrowIfNull(key2, nameof(key2));
                ExceptionHandling.ThrowIfNull(key3, nameof(key3));
                ExceptionHandling.ThrowIfNull(key4, nameof(key4));
                if (!_indices[0].TryGetValue(key1, out var indices1))
                    throw new ArgumentException($"Key not found {key1}");
                if (!_indices[1].TryGetValue(key2, out var indices2))
                    throw new ArgumentException($"Key not found {key2}");
                if (!_indices[2].TryGetValue(key3, out var indices3))
                    throw new ArgumentException($"Key not found {key3}");
                if (!_indices[3].TryGetValue(key4, out var indices4))
                    throw new ArgumentException($"Key not found {key4}");
                var index = -1;
                var indices = GetCollapsedIndexsetOrDefault();
                if (indices != null)
                {
                    indices.IntersectWith(indices1);
                    indices.IntersectWith(indices2);
                    indices.IntersectWith(indices3);
                    indices.IntersectWith(indices4);
                    index = indices.Single();
                }
                else
                {
                    index = indices1.Intersect(indices2).Intersect(indices3).Intersect(indices4).Single();
                }
                return _data[index];
            }
            set
            {
                ExceptionHandling.ThrowIfNull(key1, nameof(key1));
                ExceptionHandling.ThrowIfNull(key2, nameof(key2));
                ExceptionHandling.ThrowIfNull(key3, nameof(key3));
                ExceptionHandling.ThrowIfNull(key4, nameof(key4));
                var tuple = (ITuple)(key1, key2, key3, key4);
                var index = -1;
                var indices = GetCollapsedIndexsetOrDefault();
                if (indices != null)
                {
                    for (var i = 0; i < tuple.Length; i++)
                    {
                        if (!_indices[i].TryGetValue(tuple[i], out var ind))
                        {
                            ind = new HashSet<int>();
                            _indices[i][tuple[i]] = ind;
                        }
                        indices.IntersectWith(ind);
                    }
                    if (indices.Count > 0)
                        index = indices.Single();
                }
                else
                {
                    IEnumerable<int> filtered = null;
                    for (var i = 0; i < tuple.Length; i++)
                    {
                        if (!_indices[i].TryGetValue(tuple[i], out var ind))
                        {
                            ind = new HashSet<int>();
                            _indices[i][tuple[i]] = ind;
                        }
                        filtered = filtered == null ? ind : filtered.Intersect(ind);
                    }
                    foreach (var i in filtered ?? Enumerable.Empty<int>())
                    {
                        if (index != -1) throw new InvalidOperationException(); // Single() for non-negative enumerable
                        index = i;
                    }
                }
                if (index < 0)
                {
                    index = _data.Count;
                    _data.Insert(index, value);
                    for (var i = 0; i < tuple.Length; i++)
                    {
                        _indices[i][tuple[i]].Add(index);
                    }
                    // the data needs to be added to all collapsed dimensions
                    foreach (var f in _collapsedKeys)
                    {
                        f.Add(index);
                    }
                }
                else
                {
                    _data[index] = value;
                }
            }
        }

        public TValue this[(T1 key1, T2 key2, T3 key3, T4 key4) keys]
        {
            get => this[keys.key1, keys.key2, keys.key3, keys.key4];
            set => this[keys.key1, keys.key2, keys.key3, keys.key4] = value;
        }

        public void Add(T1 key1, T2 key2, T3 key3, T4 key4, TValue value)
        {
            if (Contains(key1, key2, key3, key4)) throw new ArgumentException($"Key already exists ({key1}, {key2}, {key3}, {key4})");
            this[key1, key2, key3, key4] = value;
        }

        public void Add((T1 key1, T2 key2, T3 key3, T4 key4) keys, TValue value) => Add(keys.key1, keys.key2, keys.key3, keys.key4, value);

        public bool TryGetValue(T1 key1, T2 key2, T3 key3, T4 key4, out TValue? value)
        {
            value = default;
            if (!_indices[0].TryGetValue(key1, out var indices1))
                return false;
            if (!_indices[1].TryGetValue(key2, out var indices2))
                return false;
            if (!_indices[2].TryGetValue(key3, out var indices3))
                return false;
            if (!_indices[3].TryGetValue(key4, out var indices4))
                return false;

            var indices = GetCollapsedIndexsetOrDefault();
            if (indices == null) indices = new HashSet<int>(indices1);
            else indices.IntersectWith(indices1);
            indices.IntersectWith(indices2);
            indices.IntersectWith(indices3);          
            indices.IntersectWith(indices4);
            if (indices.Count != 1) return false;
            value = _data[indices.Single()];
            return true;
        }

        public IndexedTable<T2, T3, T4, TValue> Slice1(T1 key1)
        {
            ExceptionHandling.ThrowIfNull(key1, nameof(key1));
            if (!_indices[0].TryGetValue(key1, out var ind))
            {
                throw new ArgumentException($"Key not found {key1}");
            }
            return new IndexedTable<T2, T3, T4, TValue>(
                new[] { _indices[1], _indices[2], _indices[3] },
                _data, _collapsedKeys.Concat(new[] { ind }).ToList());
        }

        public IndexedTable<T1, T3, T4, TValue> Slice2(T2 key2)
        {
            ExceptionHandling.ThrowIfNull(key2, nameof(key2));
            if (!_indices[1].TryGetValue(key2, out var ind))
            {
                throw new ArgumentException($"Key not found {key2}");
            }
            return new IndexedTable<T1, T3, T4, TValue>(
                new[] { _indices[0], _indices[2], _indices[3] },
                _data, _collapsedKeys.Concat(new[] { ind }).ToList());
        }

        public IndexedTable<T1, T2, T4, TValue> Slice3(T3 key3)
        {
            ExceptionHandling.ThrowIfNull(key3, nameof(key3));
            if (!_indices[2].TryGetValue(key3, out var ind))
            {
                throw new ArgumentException($"Key not found {key3}");
            }
            return new IndexedTable<T1, T2, T4, TValue>(
                new[] { _indices[0], _indices[1], _indices[3] },
                _data, _collapsedKeys.Concat(new[] { ind }).ToList());
        }

        public IndexedTable<T1, T2, T3, TValue> Slice4(T4 key4)
        {
            ExceptionHandling.ThrowIfNull(key4, nameof(key4));
            if (!_indices[3].TryGetValue(key4, out var ind))
            {
                throw new ArgumentException($"Key not found {key4}");
            }
            return new IndexedTable<T1, T2, T3, TValue>(
                new[] { _indices[0], _indices[1], _indices[2] },
                _data, _collapsedKeys.Concat(new[] { ind }).ToList());
        }

        public bool ContainsKey1(T1 key1)
        {
            ExceptionHandling.ThrowIfNull(key1, nameof(key1));
            if (!_indices[0].TryGetValue(key1, out var ind))
                return false;

            var indices = GetCollapsedIndexsetOrDefault();
            if (indices == null)
            {
                return true; // otherwise, we'd have returned false above
            }
            else
            {
                indices.IntersectWith(ind);
            }
            if (indices.Count == 0) return false;
            return true;
        }

        public bool ContainsKey2(T2 key2)
        {
            ExceptionHandling.ThrowIfNull(key2, nameof(key2));
            if (!_indices[1].TryGetValue(key2, out var ind))
                return false;

            var indices = GetCollapsedIndexsetOrDefault();
            if (indices == null)
            {
                return true; // otherwise, we'd have returned false above
            }
            else
            {
                indices.IntersectWith(ind);
            }
            if (indices.Count == 0) return false;
            return true;
        }

        public bool ContainsKey3(T3 key3)
        {
            ExceptionHandling.ThrowIfNull(key3, nameof(key3));
            if (!_indices[2].TryGetValue(key3, out var ind))
                return false;

            var indices = GetCollapsedIndexsetOrDefault();
            if (indices == null)
            {
                return true; // otherwise, we'd have returned false above
            }
            else
            {
                indices.IntersectWith(ind);
            }
            if (indices.Count == 0) return false;
            return true;
        }

        public bool ContainsKey4(T4 key4)
        {
            ExceptionHandling.ThrowIfNull(key4, nameof(key4));
            if (!_indices[3].TryGetValue(key4, out var ind))
                return false;

            var indices = GetCollapsedIndexsetOrDefault();
            if (indices == null)
            {
                return true; // otherwise, we'd have returned false above
            }
            else
            {
                indices.IntersectWith(ind);
            }
            if (indices.Count == 0) return false;
            return true;
        }

        public bool TrySlice1(T1 key1, out IndexedTable<T2, T3, T4, TValue> values)
        {
            ExceptionHandling.ThrowIfNull(key1, nameof(key1));
            if (_indices[0].TryGetValue(key1, out var indices))
            {
                values = new IndexedTable<T2, T3, T4, TValue>(new[] { _indices[1], _indices[2], _indices[3] },
                _data, _collapsedKeys.Concat(new[] { indices }).ToList());
                return true;
            }
            values = new IndexedTable<T2, T3, T4, TValue>();
            return false;
        }

        public bool TrySlice2(T2 key2, out IndexedTable<T1, T3, T4, TValue> values)
        {
            ExceptionHandling.ThrowIfNull(key2, nameof(key2));
            if (_indices[1].TryGetValue(key2, out var indices))
            {
                values = new IndexedTable<T1, T3, T4, TValue>(new[] { _indices[0], _indices[2], _indices[3] },
                _data, _collapsedKeys.Concat(new[] { indices }).ToList());
                return true;
            }
            values = new IndexedTable<T1, T3, T4, TValue>();
            return false;
        }

        public bool TrySlice3(T3 key3, out IndexedTable<T1, T2, T4, TValue> values)
        {
            ExceptionHandling.ThrowIfNull(key3, nameof(key3));
            if (_indices[2].TryGetValue(key3, out var indices))
            {
                values = new IndexedTable<T1, T2, T4, TValue>(new[] { _indices[0], _indices[1], _indices[3] },
                _data, _collapsedKeys.Concat(new[] { indices }).ToList());
                return true;
            }
            values = new IndexedTable<T1, T2, T4, TValue>();
            return false;
        }

        public bool TrySlice4(T4 key4, out IndexedTable<T1, T2, T3, TValue> values)
        {
            ExceptionHandling.ThrowIfNull(key4, nameof(key4));
            if (_indices[3].TryGetValue(key4, out var indices))
            {
                values = new IndexedTable<T1, T2, T3, TValue>(new[] { _indices[0], _indices[1], _indices[2] },
                _data, _collapsedKeys.Concat(new[] { indices }).ToList());
                return true;
            }
            values = new IndexedTable<T1, T2, T3, TValue>();
            return false;
        }

        public IEnumerable<(T1, T2, T3, T4, TValue)> Enumerate()
        {
            var collapsedIndices = GetCollapsedIndexsetOrDefault();
            IEnumerable<(T1, int)> result1 = (collapsedIndices != null
                ? _indices[0].Select(x => x.Value.Where(y => collapsedIndices.Contains(y)).Select(y => ((T1)x.Key, y))).SelectMany(x => x)
                : _indices[0].Select(x => x.Value.Select(y => ((T1)x.Key, y))).SelectMany(x => x)).OrderBy(x => x.Item2);
            IEnumerable<(T2, int)> result2 = (collapsedIndices != null
                ? _indices[1].Select(x => x.Value.Where(y => collapsedIndices.Contains(y)).Select(y => ((T2)x.Key, y))).SelectMany(x => x)
                : _indices[1].Select(x => x.Value.Select(y => ((T2)x.Key, y))).SelectMany(x => x)).OrderBy(x => x.Item2);
            IEnumerable<(T3, int)> result3 = (collapsedIndices != null
                ? _indices[2].Select(x => x.Value.Where(y => collapsedIndices.Contains(y)).Select(y => ((T3)x.Key, y))).SelectMany(x => x)
                : _indices[2].Select(x => x.Value.Select(y => ((T3)x.Key, y))).SelectMany(x => x)).OrderBy(x => x.Item2);
            IEnumerable<(T4, int)> result4 = (collapsedIndices != null
                ? _indices[3].Select(x => x.Value.Where(y => collapsedIndices.Contains(y)).Select(y => ((T4)x.Key, y))).SelectMany(x => x)
                : _indices[3].Select(x => x.Value.Select(y => ((T4)x.Key, y))).SelectMany(x => x)).OrderBy(x => x.Item2);
            var r1Iter = result1.GetEnumerator();
            var r2Iter = result2.GetEnumerator();
            var r3Iter = result3.GetEnumerator();
            var r4Iter = result4.GetEnumerator();
            while (r1Iter.MoveNext())
            {
#if DEBUG
                if (!r2Iter.MoveNext()) throw new InvalidOperationException();
                if (r2Iter.Current.Item2 != r1Iter.Current.Item2) throw new InvalidOperationException();
                if (!r3Iter.MoveNext()) throw new InvalidOperationException();
                if (r3Iter.Current.Item2 != r1Iter.Current.Item2) throw new InvalidOperationException();
                if (!r4Iter.MoveNext()) throw new InvalidOperationException();
                if (r4Iter.Current.Item2 != r1Iter.Current.Item2) throw new InvalidOperationException();
#else
                r2Iter.MoveNext();
                r3Iter.MoveNext();
                r4Iter.MoveNext();
#endif
                yield return ((T1)r1Iter.Current.Item1, (T2)r2Iter.Current.Item1, (T3)r3Iter.Current.Item1, (T4)r4Iter.Current.Item1, _data[r1Iter.Current.Item2]);
            }
        }
    }

    public class IndexedTable<T1, T2, T3, TValue> : IndexedTable<TValue>
#if !(NET472 || NET481)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
#endif
    {

        protected internal IndexedTable(IList<IDictionary<object, ISet<int>>> indices,
            IList<TValue> data, IList<ISet<int>> collapsedKeys) : base(indices, data, collapsedKeys) { }
        public IndexedTable() : this(new KeyValuePair<(T1, T2, T3), TValue>[0]) { }
        public IndexedTable(IEnumerable<KeyValuePair<(T1, T2, T3), TValue>> data)
        {
            for (var i = 0; i < 3; i++)
                _indices.Add(new Dictionary<object, ISet<int>>());

            var idx = -1;
            foreach (var d in data)
            {
                _data.Insert(++idx, d.Value);
                var key = (ITuple)d.Key;
                for (var i = 0; i < key.Length; i++)
                {
                    if (!_indices[i].TryGetValue(key[i], out var ind))
                    {
                        ind = new HashSet<int>();
                        _indices[i][key[i]] = ind;
                    }
                    ind.Add(idx);
                }
            }
        }
        public IndexedTable(IEnumerable<(T1, T2, T3, TValue)> data)
        {
            for (var i = 0; i < 3; i++)
                _indices.Add(new Dictionary<object, ISet<int>>());

            var idx = -1;
            foreach (var d in data)
            {
                _data.Insert(++idx, d.Item4);
                var key = (ITuple)d;
                for (var i = 0; i < key.Length - 1; i++)
                {
                    if (!_indices[i].TryGetValue(key[i], out var ind))
                    {
                        ind = new HashSet<int>();
                        _indices[i][key[i]] = ind;
                    }
                    ind.Add(idx);
                }
            }
        }

        public TValue this[T1 key1, T2 key2, T3 key3]
        {
            get
            {
                ExceptionHandling.ThrowIfNull(key1, nameof(key1));
                ExceptionHandling.ThrowIfNull(key2, nameof(key2));
                ExceptionHandling.ThrowIfNull(key3, nameof(key3));
                if (!_indices[0].TryGetValue(key1, out var indices1))
                    throw new ArgumentException($"Key not found {key1}");
                if (!_indices[1].TryGetValue(key2, out var indices2))
                    throw new ArgumentException($"Key not found {key2}");
                if (!_indices[2].TryGetValue(key3, out var indices3))
                    throw new ArgumentException($"Key not found {key3}");
                var index = -1;
                var indices = GetCollapsedIndexsetOrDefault();
                if (indices != null)
                {
                    indices.IntersectWith(indices1);
                    indices.IntersectWith(indices2);
                    indices.IntersectWith(indices3);
                    index = indices.Single();
                }
                else
                {
                    index = indices1.Intersect(indices2).Intersect(indices3).Single();
                }
                return _data[index];
            }
            set
            {
                ExceptionHandling.ThrowIfNull(key1, nameof(key1));
                ExceptionHandling.ThrowIfNull(key2, nameof(key2));
                ExceptionHandling.ThrowIfNull(key3, nameof(key3));
                var tuple = (ITuple)(key1, key2, key3);
                var index = -1;
                var indices = GetCollapsedIndexsetOrDefault();
                if (indices != null)
                {
                    for (var i = 0; i < tuple.Length; i++)
                    {
                        if (!_indices[i].TryGetValue(tuple[i], out var ind))
                        {
                            ind = new HashSet<int>();
                            _indices[i][tuple[i]] = ind;
                        }
                        indices.IntersectWith(ind);
                    }
                    if (indices.Count > 0)
                        index = indices.Single();
                }
                else
                {
                    IEnumerable<int> filtered = null;
                    for (var i = 0; i < tuple.Length; i++)
                    {
                        if (!_indices[i].TryGetValue(tuple[i], out var ind))
                        {
                            ind = new HashSet<int>();
                            _indices[i][tuple[i]] = ind;
                        }
                        filtered = filtered == null ? ind : filtered.Intersect(ind);
                    }
                    foreach (var i in filtered ?? Enumerable.Empty<int>())
                    {
                        if (index != -1) throw new InvalidOperationException(); // Single() for non-negative enumerable
                        index = i;
                    }
                }
                if (index < 0)
                {
                    index = _data.Count;
                    _data.Insert(index, value);
                    for (var i = 0; i < tuple.Length; i++)
                    {
                        _indices[i][tuple[i]].Add(index);
                    }
                    // the data needs to be added to all collapsed dimensions
                    foreach (var f in _collapsedKeys)
                    {
                        f.Add(index);
                    }
                }
                else
                {
                    _data[index] = value;
                }
            }
        }

        public TValue this[(T1 key1, T2 key2, T3 key3) keys]
        {
            get => this[keys.key1, keys.key2, keys.key3];
            set => this[keys.key1, keys.key2, keys.key3] = value;
        }

        public void Add(T1 key1, T2 key2, T3 key3, TValue value)
        {
            if (Contains(key1, key2, key3)) throw new ArgumentException($"Key already exists ({key1}, {key2}, {key3})");
            this[key1, key2, key3] = value;
        }

        public void Add((T1 key1, T2 key2, T3 key3) keys, TValue value) => Add(keys.key1, keys.key2, keys.key3, value);

        public bool TryGetValue(T1 key1, T2 key2, T3 key3, out TValue? value)
        {
            value = default;
            if (!_indices[0].TryGetValue(key1, out var indices1))
                return false;
            if (!_indices[1].TryGetValue(key2, out var indices2))
                return false;
            if (!_indices[2].TryGetValue(key3, out var indices3))
                return false;

            var indices = GetCollapsedIndexsetOrDefault();
            if (indices == null) indices = new HashSet<int>(indices1);
            else indices.IntersectWith(indices1);
            indices.IntersectWith(indices2);
            indices.IntersectWith(indices3);
            if (indices.Count != 1) return false;
            value = _data[indices.Single()];
            return true;
        }

        public IndexedTable<T2, T3, TValue> Slice1(T1 key1)
        {
            ExceptionHandling.ThrowIfNull(key1, nameof(key1));
            if (!_indices[0].TryGetValue(key1, out var ind))
            {
                throw new ArgumentException($"Key not found {key1}");
            }
            return new IndexedTable<T2, T3, TValue>(
                new[] { _indices[1], _indices[2] },
                _data, _collapsedKeys.Concat(new[] { ind }).ToList());
        }

        public IndexedTable<T1, T3, TValue> Slice2(T2 key2)
        {
            ExceptionHandling.ThrowIfNull(key2, nameof(key2));
            if (!_indices[1].TryGetValue(key2, out var ind))
            {
                throw new ArgumentException($"Key not found {key2}");
            }
            return new IndexedTable<T1, T3, TValue>(
                new[] { _indices[0], _indices[2] },
                _data, _collapsedKeys.Concat(new[] { ind }).ToList());
        }

        public IndexedTable<T1, T2, TValue> Slice3(T3 key3)
        {
            ExceptionHandling.ThrowIfNull(key3, nameof(key3));
            if (!_indices[2].TryGetValue(key3, out var ind))
            {
                throw new ArgumentException($"Key not found {key3}");
            }
            return new IndexedTable<T1, T2, TValue>(
                new[] { _indices[0], _indices[1] },
                _data, _collapsedKeys.Concat(new[] { ind }).ToList());
        }

        public bool ContainsKey1(T1 key1)
        {
            ExceptionHandling.ThrowIfNull(key1, nameof(key1));
            if (!_indices[0].TryGetValue(key1, out var ind))
                return false;

            var indices = GetCollapsedIndexsetOrDefault();
            if (indices == null)
            {
                return true; // otherwise, we'd have returned false above
            }
            else
            {
                indices.IntersectWith(ind);
            }
            if (indices.Count == 0) return false;
            return true;
        }

        public bool ContainsKey2(T2 key2)
        {
            ExceptionHandling.ThrowIfNull(key2, nameof(key2));
            if (!_indices[1].TryGetValue(key2, out var ind))
                return false;

            var indices = GetCollapsedIndexsetOrDefault();
            if (indices == null)
            {
                return true; // otherwise, we'd have returned false above
            }
            else
            {
                indices.IntersectWith(ind);
            }
            if (indices.Count == 0) return false;
            return true;
        }

        public bool ContainsKey3(T3 key3)
        {
            ExceptionHandling.ThrowIfNull(key3, nameof(key3));
            if (!_indices[2].TryGetValue(key3, out var ind))
                return false;

            var indices = GetCollapsedIndexsetOrDefault();
            if (indices == null)
            {
                return true; // otherwise, we'd have returned false above
            }
            else
            {
                indices.IntersectWith(ind);
            }
            if (indices.Count == 0) return false;
            return true;
        }

        public bool TrySlice1(T1 key1, out IndexedTable<T2, T3, TValue> values)
        {
            ExceptionHandling.ThrowIfNull(key1, nameof(key1));
            if (_indices[0].TryGetValue(key1, out var indices))
            {
                values = new IndexedTable<T2, T3, TValue>(new[] { _indices[1], _indices[2] },
                _data, _collapsedKeys.Concat(new[] { indices }).ToList());
                return true;
            }
            values = new IndexedTable<T2, T3, TValue>();
            return false;
        }

        public bool TrySlice2(T2 key2, out IndexedTable<T1, T3, TValue> values)
        {
            ExceptionHandling.ThrowIfNull(key2, nameof(key2));
            if (_indices[1].TryGetValue(key2, out var indices))
            {
                values = new IndexedTable<T1, T3, TValue>(new[] { _indices[0], _indices[2] },
                _data, _collapsedKeys.Concat(new[] { indices }).ToList());
                return true;
            }
            values = new IndexedTable<T1, T3, TValue>();
            return false;
        }

        public bool TrySlice3(T3 key3, out IndexedTable<T1, T2, TValue> values)
        {
            ExceptionHandling.ThrowIfNull(key3, nameof(key3));
            if (_indices[2].TryGetValue(key3, out var indices))
            {
                values = new IndexedTable<T1, T2, TValue>(new[] { _indices[0], _indices[1] },
                _data, _collapsedKeys.Concat(new[] { indices }).ToList());
                return true;
            }
            values = new IndexedTable<T1, T2, TValue>();
            return false;
        }

        public IEnumerable<(T1, T2, T3, TValue)> Enumerate()
        {
            var collapsedIndices = GetCollapsedIndexsetOrDefault();
            IEnumerable<(T1, int)> result1 = (collapsedIndices != null
                ? _indices[0].Select(x => x.Value.Where(y => collapsedIndices.Contains(y)).Select(y => ((T1)x.Key, y))).SelectMany(x => x)
                : _indices[0].Select(x => x.Value.Select(y => ((T1)x.Key, y))).SelectMany(x => x)).OrderBy(x => x.Item2);
            IEnumerable<(T2, int)> result2 = (collapsedIndices != null
                ? _indices[1].Select(x => x.Value.Where(y => collapsedIndices.Contains(y)).Select(y => ((T2)x.Key, y))).SelectMany(x => x)
                : _indices[1].Select(x => x.Value.Select(y => ((T2)x.Key, y))).SelectMany(x => x)).OrderBy(x => x.Item2);
            IEnumerable<(T3, int)> result3 = (collapsedIndices != null
                ? _indices[2].Select(x => x.Value.Where(y => collapsedIndices.Contains(y)).Select(y => ((T3)x.Key, y))).SelectMany(x => x)
                : _indices[2].Select(x => x.Value.Select(y => ((T3)x.Key, y))).SelectMany(x => x)).OrderBy(x => x.Item2);
            var r1Iter = result1.GetEnumerator();
            var r2Iter = result2.GetEnumerator();
            var r3Iter = result3.GetEnumerator();
            while (r1Iter.MoveNext())
            {
#if DEBUG
                if (!r2Iter.MoveNext()) throw new InvalidOperationException();
                if (r2Iter.Current.Item2 != r1Iter.Current.Item2) throw new InvalidOperationException();
                if (!r3Iter.MoveNext()) throw new InvalidOperationException();
                if (r3Iter.Current.Item2 != r1Iter.Current.Item2) throw new InvalidOperationException();
#else
                r2Iter.MoveNext();
                r3Iter.MoveNext();
#endif
                yield return ((T1)r1Iter.Current.Item1, (T2)r2Iter.Current.Item1, (T3)r3Iter.Current.Item1, _data[r1Iter.Current.Item2]);
            }
        }
    }

    public class IndexedTable<T1, T2, TValue> : IndexedTable<TValue>
#if !(NET472 || NET481)
        where T1 : notnull
        where T2 : notnull
#endif
    {

        protected internal IndexedTable(IList<IDictionary<object, ISet<int>>> indices,
            IList<TValue> data, IList<ISet<int>> collapsedKeys) : base(indices, data, collapsedKeys) { }
        public IndexedTable() : this(new KeyValuePair<(T1, T2), TValue>[0]) { }
        public IndexedTable(IEnumerable<KeyValuePair<(T1, T2), TValue>> data)
        {
            for (var i = 0; i < 2; i++)
                _indices.Add(new Dictionary<object, ISet<int>>());

            var idx = -1;
            foreach (var d in data)
            {
                _data.Insert(++idx, d.Value);
                var key = (ITuple)d.Key;
                for (var i = 0; i < key.Length; i++)
                {
                    if (!_indices[i].TryGetValue(key[i], out var ind))
                    {
                        ind = new HashSet<int>();
                        _indices[i][key[i]] = ind;
                    }
                    ind.Add(idx);
                }
            }
        }
        public IndexedTable(IEnumerable<(T1, T2, TValue)> data)
        {
            for (var i = 0; i < 2; i++)
                _indices.Add(new Dictionary<object, ISet<int>>());

            var idx = -1;
            foreach (var d in data)
            {
                _data.Insert(++idx, d.Item3);
                var key = (ITuple)d;
                for (var i = 0; i < key.Length - 1; i++)
                {
                    if (!_indices[i].TryGetValue(key[i], out var ind))
                    {
                        ind = new HashSet<int>();
                        _indices[i][key[i]] = ind;
                    }
                    ind.Add(idx);
                }
            }
        }

        public TValue this[T1 key1, T2 key2]
        {
            get
            {
                ExceptionHandling.ThrowIfNull(key1, nameof(key1));
                ExceptionHandling.ThrowIfNull(key2, nameof(key2));
                if (!_indices[0].TryGetValue(key1, out var indices1))
                    throw new ArgumentException($"Key not found {key1}");
                if (!_indices[1].TryGetValue(key2, out var indices2))
                    throw new ArgumentException($"Key not found {key2}");
                var index = -1;
                var indices = GetCollapsedIndexsetOrDefault();
                if (indices != null)
                {
                    indices.IntersectWith(indices1);
                    indices.IntersectWith(indices2);
                    index = indices.Single();
                }
                else
                {
                    index = indices1.Intersect(indices2).Single();
                }
                return _data[index];
            }
            set
            {
                ExceptionHandling.ThrowIfNull(key1, nameof(key1));
                ExceptionHandling.ThrowIfNull(key2, nameof(key2));
                var tuple = (ITuple)(key1, key2);
                var index = -1;
                var indices = GetCollapsedIndexsetOrDefault();
                if (indices != null)
                {
                    for (var i = 0; i < tuple.Length; i++)
                    {
                        if (!_indices[i].TryGetValue(tuple[i], out var ind))
                        {
                            ind = new HashSet<int>();
                            _indices[i][tuple[i]] = ind;
                        }
                        indices.IntersectWith(ind);
                    }
                    if (indices.Count > 0)
                        index = indices.Single();
                }
                else
                {
                    IEnumerable<int> filtered = null;
                    for (var i = 0; i < tuple.Length; i++)
                    {
                        if (!_indices[i].TryGetValue(tuple[i], out var ind))
                        {
                            ind = new HashSet<int>();
                            _indices[i][tuple[i]] = ind;
                        }
                        filtered = filtered == null ? ind : filtered.Intersect(ind);
                    }
                    foreach (var i in filtered ?? Enumerable.Empty<int>())
                    {
                        if (index != -1) throw new InvalidOperationException(); // Single() for non-negative enumerable
                        index = i;
                    }
                }
                if (index < 0)
                {
                    index = _data.Count;
                    _data.Insert(index, value);
                    for (var i = 0; i < tuple.Length; i++)
                    {
                        _indices[i][tuple[i]].Add(index);
                    }
                    // the data needs to be added to all collapsed dimensions
                    foreach (var f in _collapsedKeys)
                    {
                        f.Add(index);
                    }
                }
                else
                {
                    _data[index] = value;
                }
            }
        }

        public TValue this[(T1 key1, T2 key2) keys]
        {
            get => this[keys.key1, keys.key2];
            set => this[keys.key1, keys.key2] = value;
        }

        public void Add(T1 key1, T2 key2, TValue value)
        {
            if (Contains(key1, key2)) throw new ArgumentException($"Key already exists ({key1}, {key2})");
            this[key1, key2] = value;
        }

        public void Add((T1 key1, T2 key2) keys, TValue value) => Add(keys.key1, keys.key2, value);

        public bool TryGetValue(T1 key1, T2 key2, out TValue? value)
        {
            value = default;
            if (!_indices[0].TryGetValue(key1, out var indices1))
                return false;
            if (!_indices[1].TryGetValue(key2, out var indices2))
                return false;

            var indices = GetCollapsedIndexsetOrDefault();
            if (indices == null) indices = new HashSet<int>(indices1);
            else indices.IntersectWith(indices1);
            indices.IntersectWith(indices2);
            if (indices.Count != 1) return false;
            value = _data[indices.Single()];
            return true;
        }

        public IndexedTable<T2, TValue> Slice1(T1 key1)
        {
            ExceptionHandling.ThrowIfNull(key1, nameof(key1));
            if (!_indices[0].TryGetValue(key1, out var ind))
            {
                throw new ArgumentException($"Key not found {key1}");
            }
            return new IndexedTable<T2, TValue>(
                new[] { _indices[1] },
                _data, _collapsedKeys.Concat(new[] { ind }).ToList());
        }

        public IndexedTable<T1, TValue> Slice2(T2 key2)
        {
            ExceptionHandling.ThrowIfNull(key2, nameof(key2));
            if (!_indices[1].TryGetValue(key2, out var ind))
            {
                throw new ArgumentException($"Key not found {key2}");
            }
            return new IndexedTable<T1, TValue>(
                new[] { _indices[0] },
                _data, _collapsedKeys.Concat(new[] { ind }).ToList());
        }

        public bool ContainsKey1(T1 key1)
        {
            ExceptionHandling.ThrowIfNull(key1, nameof(key1));
            if (!_indices[0].TryGetValue(key1, out var ind))
                return false;

            var indices = GetCollapsedIndexsetOrDefault();
            if (indices == null)
            {
                return true; // otherwise, we'd have returned false above
            }
            else
            {
                indices.IntersectWith(ind);
            }
            if (indices.Count == 0) return false;
            return true;
        }

        public bool ContainsKey2(T2 key2)
        {
            ExceptionHandling.ThrowIfNull(key2, nameof(key2));
            if (!_indices[1].TryGetValue(key2, out var ind))
                return false;

            var indices = GetCollapsedIndexsetOrDefault();
            if (indices == null)
            {
                return true; // otherwise, we'd have returned false above
            }
            else
            {
                indices.IntersectWith(ind);
            }
            if (indices.Count == 0) return false;
            return true;
        }

        public bool TrySlice1(T1 key1, out IndexedTable<T2, TValue> values)
        {
            ExceptionHandling.ThrowIfNull(key1, nameof(key1));
            if (_indices[0].TryGetValue(key1, out var indices))
            {
                values = new IndexedTable<T2, TValue>(new[] { _indices[1] },
                _data, _collapsedKeys.Concat(new[] { indices }).ToList());
                return true;
            }
            values = new IndexedTable<T2, TValue>();
            return false;
        }

        public bool TrySlice2(T2 key2, out IndexedTable<T1, TValue> values)
        {
            ExceptionHandling.ThrowIfNull(key2, nameof(key2));
            if (_indices[1].TryGetValue(key2, out var indices))
            {
                values = new IndexedTable<T1, TValue>(new[] { _indices[0] },
                _data, _collapsedKeys.Concat(new[] { indices }).ToList());
                return true;
            }
            values = new IndexedTable<T1, TValue>();
            return false;
        }

        public IEnumerable<(T1, T2, TValue)> Enumerate()
        {
            var collapsedIndices = GetCollapsedIndexsetOrDefault();
            IEnumerable<(T1, int)> result1 = (collapsedIndices != null
                ? _indices[0].Select(x => x.Value.Where(y => collapsedIndices.Contains(y)).Select(y => ((T1)x.Key, y))).SelectMany(x => x)
                : _indices[0].Select(x => x.Value.Select(y => ((T1)x.Key, y))).SelectMany(x => x)).OrderBy(x => x.Item2);
            IEnumerable<(T2, int)> result2 = (collapsedIndices != null
                ? _indices[1].Select(x => x.Value.Where(y => collapsedIndices.Contains(y)).Select(y => ((T2)x.Key, y))).SelectMany(x => x)
                : _indices[1].Select(x => x.Value.Select(y => ((T2)x.Key, y))).SelectMany(x => x)).OrderBy(x => x.Item2);
            var r1Iter = result1.GetEnumerator();
            var r2Iter = result2.GetEnumerator();
            while (r1Iter.MoveNext())
            {
#if DEBUG
                if (!r2Iter.MoveNext()) throw new InvalidOperationException();
                if (r1Iter.Current.Item2 != r2Iter.Current.Item2) throw new InvalidOperationException();
#else
                r2Iter.MoveNext();
#endif
                yield return ((T1)r1Iter.Current.Item1, (T2)r2Iter.Current.Item1, _data[r1Iter.Current.Item2]);
            }
        }
    }

    public class IndexedTable<T1, TValue> : IndexedTable<TValue>
#if !(NET472 || NET481)
        where T1 : notnull
#endif
    {
        protected internal IndexedTable(IList<IDictionary<object, ISet<int>>> indices,
            IList<TValue> data, IList<ISet<int>> collapsedKeys) : base(indices, data, collapsedKeys) { }
        public IndexedTable() : this(new KeyValuePair<T1, TValue>[0]) { }
        public IndexedTable(IEnumerable<KeyValuePair<T1, TValue>> data)
        {
            _indices.Add(new Dictionary<object, ISet<int>>());

            var idx = -1;
            foreach (var d in data)
            {
                _data.Insert(++idx, d.Value);
                if (!_indices[0].TryGetValue(d.Key, out var ind))
                {
                    ind = new HashSet<int>();
                    _indices[0][d.Key] = ind;
                }
                ind.Add(idx);
            }
        }
        public IndexedTable(IEnumerable<(T1, TValue)> data)
        {
            for (var i = 0; i < 2; i++)
                _indices.Add(new Dictionary<object, ISet<int>>());

            var idx = -1;
            foreach (var d in data)
            {
                _data.Insert(++idx, d.Item2);
                if (!_indices[0].TryGetValue(d.Item1, out var ind))
                {
                    ind = new HashSet<int>();
                    _indices[0][d.Item1] = ind;
                }
                ind.Add(idx);
            }
        }

        public TValue this[T1 key1]
        {
            get
            {
                ExceptionHandling.ThrowIfNull(key1, nameof(key1));
                if (!_indices[0].TryGetValue(key1, out var indices1))
                    throw new ArgumentException($"Key not found {key1}");
                IEnumerable<int> filtered = indices1;
                foreach (var ck in _collapsedKeys)
                    filtered = filtered.Intersect(ck);
                var index = filtered.Single();
                return _data[index];
            }
            set
            {
                ExceptionHandling.ThrowIfNull(key1, nameof(key1));
                if (!_indices[0].TryGetValue(key1, out var ind))
                {
                    ind = new HashSet<int>();
                    _indices[0][key1] = ind;
                }
#if NET472 || NET481
                IEnumerable<int> filtered = ind;
#else
                IEnumerable<int>? filtered = ind;
#endif
                foreach (var ck in _collapsedKeys)
                    filtered = filtered.Intersect(ck);
                var index = -1;
                foreach (var i in filtered)
                {
                    if (index != -1) throw new InvalidOperationException();
                    index = i;
                }
                if (index < 0)
                {
                    index = _data.Count;
                    _data.Insert(index, value);
                    _indices[0][key1].Add(index);
                    // the data needs to be added to all collapsed dimensions
                    foreach (var f in _collapsedKeys)
                    {
                        f.Add(index);
                    }
                }
                else
                {
                    _data[index] = value;
                }
            }
        }

        public void Add(T1 key1, TValue value)
        {
            if (ContainsKey(key1)) throw new ArgumentException($"Key already exists {key1}");
            this[key1] = value;
        }

        public bool TryGetValue(T1 key1, out TValue? value)
        {
            value = default;
            if (!_indices[0].TryGetValue(key1, out var indices1))
                return false;

            var indices = GetCollapsedIndexsetOrDefault();
            if (indices == null) indices = new HashSet<int>(indices1);
            else indices.IntersectWith(indices1);
            if (indices.Count != 1) return false;
            value = _data[indices.Single()];
            return true;
        }

        public bool ContainsKey(T1 key1)
        {
            ExceptionHandling.ThrowIfNull(key1, nameof(key1));
            if (!_indices[0].TryGetValue(key1, out var ind))
                return false;

            var indices = GetCollapsedIndexsetOrDefault();
            if (indices == null)
            {
                return true; // otherwise, we'd have returned false above
            }
            else
            {
                indices.IntersectWith(ind);
            }
            if (indices.Count == 0) return false;
            return true;
        }

        public IEnumerable<(T1, TValue)> Enumerate(bool orderByIndex = true)
        {
            var collapsedIndices = GetCollapsedIndexsetOrDefault();
            IEnumerable<(T1, int)> result = collapsedIndices != null
                ? _indices[0].Select(x => x.Value.Where(y => collapsedIndices.Contains(y)).Select(y => ((T1)x.Key, y))).SelectMany(x => x)
                : _indices[0].Select(x => x.Value.Select(y => ((T1)x.Key, y))).SelectMany(x => x);
            if (orderByIndex) result = result.OrderBy(x => x.Item2);
            foreach (var r in result)
            {
                yield return ((T1)r.Item1, _data[r.Item2]);
            }
        }

        public Dictionary<T1, TValue> ToDictionary()
        {
            return Enumerate(false).ToDictionary(x => x.Item1, x => x.Item2);
        }
    }
}