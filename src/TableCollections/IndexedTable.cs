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
        protected ISet<int>_unusedIndices;
        /// <summary>
        /// Removals do not actually remove data from the table, but just delete the index.
        /// If a lot of Remove operations are performed, the table will become very sparsely populated with actual data.
        /// This setting controls the absolute number of unused indices that will trigger a compaction.
        /// Both this and the relative threshold must be met for a compaction to occur.
        /// </summary>
        /// <value>The number of unused indices before a compaction is performed.</value>
        public int CompactingAbsoluteThreshold { get; set; } = 1000;
        /// <summary>
        /// Removals do not actually remove data from the table, but just delete the index.
        /// If a lot of Remove operations are performed, the table will become very sparsely populated with actual data.
        /// This setting controls the relative number of unused indices that will trigger a compaction.
        /// Both this and the absolute threshold must be met for a compaction to occur.
        /// </summary>
        /// <value>The relative number of unused indices (relative to the full size of the array) before a compaction is performed.</value>
        public double CompactingRelativeThreshold { get; set; } = 0.5;

        protected IndexedTable()
        {
            _indices = new List<IDictionary<object, ISet<int>>>();
            _data = new List<TValue>();
            _collapsedKeys = new List<ISet<int>>();
            _unusedIndices = new HashSet<int>();
        }

        protected internal IndexedTable(IList<IDictionary<object, ISet<int>>> indices,
            IList<TValue> data, IList<ISet<int>> collapsedKeys, ISet<int> unusedIndices)
        {
            _indices = indices;
            _data = data;
            _collapsedKeys = collapsedKeys;
            _unusedIndices = unusedIndices;
        }

        public int Count => GetCollapsedIndexsetOrDefault()?.Count ?? (_data.Count - _unusedIndices.Count);

        /// <summary>
        /// Sets *all* values in the table (respectively the slice) to the given value
        /// </summary>
        /// <param name="value"></param>
        public void Set(TValue value)
        {
            // This method also overrides unused indices *shrug*
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

        public bool Remove(params object[] keys)
        {
            ExceptionHandling.ThrowIfNull(keys, nameof(keys));
            if (keys.Length != _indices.Count)
                throw new ArgumentException($"Expected {nameof(keys)} to have {_indices.Count} elements");

            var indices = GetCollapsedIndexsetOrDefault();

            for (var i = 0; i < keys.Length; i++)
            {
                if (!_indices[i].TryGetValue(keys[i], out var ind) || ind.Count == 0)
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

            var index = indices.Single();
            _data[index] = default; // don't remove, just set to default
            _unusedIndices.Add(index);
            for (var i = 0; i < _collapsedKeys.Count; i++)
            {
                _collapsedKeys[i].Remove(index); // we don't need to manipulate all indices > index, because we don't remove above
            }
            for (var i = 0; i < _indices.Count; i++)
            {
                foreach (var ind in _indices[i].Values)
                {
                    ind.Remove(index); // we don't need to manipulate all indices > index, because we don't remove above
                }
            }
            CheckCompact();
            return true;
        }

        protected void CheckCompact()
        {
            if (_unusedIndices.Count > CompactingAbsoluteThreshold
                && _unusedIndices.Count > _data.Count * CompactingRelativeThreshold)
                Compact();
        }

        private void Compact()
        {
            if (_unusedIndices.Count == 0) return;
            var sequence = _unusedIndices.OrderByDescending(x => x).ToList();
            var offsets = new Dictionary<int, int>();
            var offset = 0;
            var offsetIndex = 0;
            for (var i = sequence.Count - 1; i >= 0; i--)
            {
                var index = sequence[i];
                if (offset > 0)
                {
                    for (var o = offsetIndex; o < index; o++)
                    {
                        offsets[o] = offset;
                    }
                }
                offsetIndex = index;
                offset++;
                _data.RemoveAt(sequence[sequence.Count - 1 - i]);
            }
            for (var o = offsetIndex; o < _data.Count + offset; o++)
            {
                offsets[o] = offset;
            }
            foreach (var indices in _collapsedKeys)
            {
                indices.ExceptWith(_unusedIndices);
                var reindex = indices.Select(x => (index: x, offset: offsets.TryGetValue(x, out var o) ? o : 0)).Where(x => x.offset > 0).ToList();
                if (reindex.Count > 0)
                {
                    indices.ExceptWith(reindex.Select(x => x.index));
                    indices.UnionWith(reindex.Select(x => x.index - x.offset));
                }
            }
            foreach (var dict in _indices)
            {
#if NET472 || NET481
                foreach (var kvp in dict)
                {
                    var key = kvp.Key;
                    var indices = kvp.Value;
#else
                foreach (var (key, indices) in dict)
                {
#endif
                    indices.ExceptWith(_unusedIndices);
                    var reindex = indices.Select(x => (index: x, offset: offsets.TryGetValue(x, out var o) ? o : 0)).Where(x => x.offset > 0).ToList();
                    if (reindex.Count > 0)
                    {
                        indices.ExceptWith(reindex.Select(x => x.index));
                        indices.UnionWith(reindex.Select(x => x.index - x.offset));
                    }
                }
            }
            _unusedIndices.Clear();
        }

        public bool Contains(params object[] keys)
        {
            ExceptionHandling.ThrowIfNull(keys, nameof(keys));
            if (keys.Length != _indices.Count)
                throw new ArgumentException($"Expected {nameof(keys)} to have {_indices.Count} elements");

            var indices = GetCollapsedIndexsetOrDefault();

            for (var i = 0; i < keys.Length; i++)
            {
                if (!_indices[i].TryGetValue(keys[i], out var ind) || ind.Count == 0)
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
            IList<TValue> data, IList<ISet<int>> collapsedKeys, ISet<int> unusedIndices) : base(indices, data, collapsedKeys, unusedIndices) { }
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
                if (!_indices[0].TryGetValue(key1, out var indices1) || indices1.Count == 0)
                    throw new ArgumentException($"Key not found {key1}");
                if (!_indices[1].TryGetValue(key2, out var indices2) || indices2.Count == 0)
                    throw new ArgumentException($"Key not found {key2}");
                if (!_indices[2].TryGetValue(key3, out var indices3) || indices3.Count == 0)
                    throw new ArgumentException($"Key not found {key3}");
                if (!_indices[3].TryGetValue(key4, out var indices4) || indices4.Count == 0)
                    throw new ArgumentException($"Key not found {key4}");
                if (!_indices[4].TryGetValue(key5, out var indices5) || indices5.Count == 0)
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
                    if (_unusedIndices.Count > 0)
                    {
                        // reuse an unused index
                        index = _unusedIndices.First();
                        _unusedIndices.Remove(index);
                        _data[index] = value;
                    }
                    else
                    {
                        // make an insert at the end
                        index = _data.Count;
                        _data.Insert(index, value);
                    }
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

        public int Remove1(T1 key1)
        {
            if (!_indices[0].TryGetValue(key1, out var indices1) || indices1.Count == 0)
                return 0;
            foreach (var indices in _collapsedKeys)
            {
                indices.ExceptWith(indices1);
            }
            foreach (var dict in _indices)
            {
                foreach (var indices in dict.Values)
                {
                    if (indices == indices1)
                        continue;
                    indices.ExceptWith(indices1);
                }
            }
            var removed = indices1.Count;
            _unusedIndices.UnionWith(indices1);
            indices1.Clear();
            CheckCompact();
            return removed;
        }

        public int Remove2(T2 key2)
        {
            if (!_indices[1].TryGetValue(key2, out var indices2) || indices2.Count == 0)
                return 0;
            foreach (var indices in _collapsedKeys)
            {
                indices.ExceptWith(indices2);
            }
            foreach (var dict in _indices)
            {
                foreach (var indices in dict.Values)
                {
                    if (indices == indices2)
                        continue;
                    indices.ExceptWith(indices2);
                }
            }
            var removed = indices2.Count;
            _unusedIndices.UnionWith(indices2);
            indices2.Clear();
            CheckCompact();
            return removed;
        }

        public int Remove3(T3 key3)
        {
            if (!_indices[2].TryGetValue(key3, out var indices3) || indices3.Count == 0)
                return 0;
            foreach (var indices in _collapsedKeys)
            {
                indices.ExceptWith(indices3);
            }
            foreach (var dict in _indices)
            {
                foreach (var indices in dict.Values)
                {
                    if (indices == indices3)
                        continue;
                    indices.ExceptWith(indices3);
                }
            }
            var removed = indices3.Count;
            _unusedIndices.UnionWith(indices3);
            indices3.Clear();
            CheckCompact();
            return removed;
        }

        public int Remove4(T4 key4)
        {
            if (!_indices[3].TryGetValue(key4, out var indices4) || indices4.Count == 0)
                return 0;
            foreach (var indices in _collapsedKeys)
            {
                indices.ExceptWith(indices4);
            }
            foreach (var dict in _indices)
            {
                foreach (var indices in dict.Values)
                {
                    if (indices == indices4)
                        continue;
                    indices.ExceptWith(indices4);
                }
            }
            var removed = indices4.Count;
            _unusedIndices.UnionWith(indices4);
            indices4.Clear();
            CheckCompact();
            return removed;
        }

        public int Remove5(T5 key5)
        {
            if (!_indices[4].TryGetValue(key5, out var indices5) || indices5.Count == 0)
                return 0;
            foreach (var indices in _collapsedKeys)
            {
                indices.ExceptWith(indices5);
            }
            foreach (var dict in _indices)
            {
                foreach (var indices in dict.Values)
                {
                    if (indices == indices5)
                        continue;
                    indices.ExceptWith(indices5);
                }
            }
            var removed = indices5.Count;
            _unusedIndices.UnionWith(indices5);
            indices5.Clear();
            CheckCompact();
            return removed;
        }

#if NET472 || NET481 || NETSTANDARD2_1
        public bool TryGetValue(T1 key1, T2 key2, T3 key3, T4 key4, T5 key5, out TValue value)
#else
        public bool TryGetValue(T1 key1, T2 key2, T3 key3, T4 key4, T5 key5, out TValue? value)
#endif
        {
            value = default;
            if (!_indices[0].TryGetValue(key1, out var indices1) || indices1.Count == 0)
                return false;
            if (!_indices[1].TryGetValue(key2, out var indices2) || indices2.Count == 0)
                return false;
            if (!_indices[2].TryGetValue(key3, out var indices3) || indices3.Count == 0)
                return false;
            if (!_indices[3].TryGetValue(key4, out var indices4) || indices4.Count == 0)
                return false;
            if (!_indices[4].TryGetValue(key5, out var indices5) || indices5.Count == 0)
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
            if (!_indices[0].TryGetValue(key1, out var ind) || ind.Count == 0)
            {
                throw new ArgumentException($"Key not found {key1}");
            }
            return new IndexedTable<T2, T3, T4, T5, TValue>(
                new[] { _indices[1], _indices[2], _indices[3], _indices[4] },
                _data, _collapsedKeys.Concat(new[] { ind }).ToList(), _unusedIndices);
        }

        public IndexedTable<T1, T3, T4, T5, TValue> Slice2(T2 key2)
        {
            ExceptionHandling.ThrowIfNull(key2, nameof(key2));
            if (!_indices[1].TryGetValue(key2, out var ind) || ind.Count == 0)
            {
                throw new ArgumentException($"Key not found {key2}");
            }
            return new IndexedTable<T1, T3, T4, T5, TValue>(
                new[] { _indices[0], _indices[2], _indices[3], _indices[4] },
                _data, _collapsedKeys.Concat(new[] { ind }).ToList(), _unusedIndices);
        }

        public IndexedTable<T1, T2, T4, T5, TValue> Slice3(T3 key3)
        {
            ExceptionHandling.ThrowIfNull(key3, nameof(key3));
            if (!_indices[2].TryGetValue(key3, out var ind) || ind.Count == 0)
            {
                throw new ArgumentException($"Key not found {key3}");
            }
            return new IndexedTable<T1, T2, T4, T5, TValue>(
                new[] { _indices[0], _indices[1], _indices[3], _indices[4] },
                _data, _collapsedKeys.Concat(new[] { ind }).ToList(), _unusedIndices);
        }

        public IndexedTable<T1, T2, T3, T5, TValue> Slice4(T4 key4)
        {
            ExceptionHandling.ThrowIfNull(key4, nameof(key4));
            if (!_indices[3].TryGetValue(key4, out var ind) || ind.Count == 0)
            {
                throw new ArgumentException($"Key not found {key4}");
            }
            return new IndexedTable<T1, T2, T3, T5, TValue>(
                new[] { _indices[0], _indices[1], _indices[2], _indices[4] },
                _data, _collapsedKeys.Concat(new[] { ind }).ToList(), _unusedIndices);
        }

        public IndexedTable<T1, T2, T3, T4, TValue> Slice5(T5 key5)
        {
            ExceptionHandling.ThrowIfNull(key5, nameof(key5));
            if (!_indices[4].TryGetValue(key5, out var ind) || ind.Count == 0)
            {
                throw new ArgumentException($"Key not found {key5}");
            }
            return new IndexedTable<T1, T2, T3, T4, TValue>(
                new[] { _indices[0], _indices[1], _indices[2], _indices[3] },
                _data, _collapsedKeys.Concat(new[] { ind }).ToList(), _unusedIndices);
        }

        public bool ContainsKey1(T1 key1)
        {
            ExceptionHandling.ThrowIfNull(key1, nameof(key1));
            if (!_indices[0].TryGetValue(key1, out var ind) || ind.Count == 0)
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
            if (!_indices[1].TryGetValue(key2, out var ind) || ind.Count == 0)
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
            if (!_indices[2].TryGetValue(key3, out var ind) || ind.Count == 0)
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
            if (!_indices[3].TryGetValue(key4, out var ind) || ind.Count == 0)
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
            if (!_indices[4].TryGetValue(key5, out var ind) || ind.Count == 0)
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
            if (_indices[0].TryGetValue(key1, out var indices) && indices.Count > 0)
            {
                values = new IndexedTable<T2, T3, T4, T5, TValue>(new[] { _indices[1], _indices[2], _indices[3], _indices[4] },
                _data, _collapsedKeys.Concat(new[] { indices }).ToList(), _unusedIndices);
                return true;
            }
            values = new IndexedTable<T2, T3, T4, T5, TValue>();
            return false;
        }

        public bool TrySlice2(T2 key2, out IndexedTable<T1, T3, T4, T5, TValue> values)
        {
            ExceptionHandling.ThrowIfNull(key2, nameof(key2));
            if (_indices[1].TryGetValue(key2, out var indices) && indices.Count > 0)
            {
                values = new IndexedTable<T1, T3, T4, T5, TValue>(new[] { _indices[0], _indices[2], _indices[3], _indices[4] },
                _data, _collapsedKeys.Concat(new[] { indices }).ToList(), _unusedIndices);
                return true;
            }
            values = new IndexedTable<T1, T3, T4, T5, TValue>();
            return false;
        }

        public bool TrySlice3(T3 key3, out IndexedTable<T1, T2, T4, T5, TValue> values)
        {
            ExceptionHandling.ThrowIfNull(key3, nameof(key3));
            if (_indices[2].TryGetValue(key3, out var indices) && indices.Count > 0)
            {
                values = new IndexedTable<T1, T2, T4, T5, TValue>(new[] { _indices[0], _indices[1], _indices[3], _indices[4] },
                _data, _collapsedKeys.Concat(new[] { indices }).ToList(), _unusedIndices);
                return true;
            }
            values = new IndexedTable<T1, T2, T4, T5, TValue>();
            return false;
        }

        public bool TrySlice4(T4 key4, out IndexedTable<T1, T2, T3, T5, TValue> values)
        {
            ExceptionHandling.ThrowIfNull(key4, nameof(key4));
            if (_indices[3].TryGetValue(key4, out var indices) && indices.Count > 0)
            {
                values = new IndexedTable<T1, T2, T3, T5, TValue>(new[] { _indices[0], _indices[1], _indices[2], _indices[4] },
                _data, _collapsedKeys.Concat(new[] { indices }).ToList(), _unusedIndices);
                return true;
            }
            values = new IndexedTable<T1, T2, T3, T5, TValue>();
            return false;
        }

        public bool TrySlice5(T5 key5, out IndexedTable<T1, T2, T3, T4, TValue> values)
        {
            ExceptionHandling.ThrowIfNull(key5, nameof(key5));
            if (_indices[4].TryGetValue(key5, out var indices) && indices.Count > 0)
            {
                values = new IndexedTable<T1, T2, T3, T4, TValue>(new[] { _indices[0], _indices[1], _indices[2], _indices[3] },
                _data, _collapsedKeys.Concat(new[] { indices }).ToList(), _unusedIndices);
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
            IList<TValue> data, IList<ISet<int>> collapsedKeys, ISet<int> unusedIndices) : base(indices, data, collapsedKeys, unusedIndices) { }
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
                if (!_indices[0].TryGetValue(key1, out var indices1) || indices1.Count == 0)
                    throw new ArgumentException($"Key not found {key1}");
                if (!_indices[1].TryGetValue(key2, out var indices2) || indices2.Count == 0)
                    throw new ArgumentException($"Key not found {key2}");
                if (!_indices[2].TryGetValue(key3, out var indices3) || indices3.Count == 0)
                    throw new ArgumentException($"Key not found {key3}");
                if (!_indices[3].TryGetValue(key4, out var indices4) || indices4.Count == 0)
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
                    if (_unusedIndices.Count > 0)
                    {
                        // reuse an unused index
                        index = _unusedIndices.First();
                        _unusedIndices.Remove(index);
                        _data[index] = value;
                    }
                    else
                    {
                        // make an insert at the end
                        index = _data.Count;
                        _data.Insert(index, value);
                    }
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

        public int Remove1(T1 key1)
        {
            if (!_indices[0].TryGetValue(key1, out var indices1) || indices1.Count == 0)
                return 0;
            foreach (var indices in _collapsedKeys)
            {
                indices.ExceptWith(indices1);
            }
            foreach (var dict in _indices)
            {
                foreach (var indices in dict.Values)
                {
                    if (indices == indices1)
                        continue;
                    indices.ExceptWith(indices1);
                }
            }
            var removed = indices1.Count;
            _unusedIndices.UnionWith(indices1);
            indices1.Clear();
            CheckCompact();
            return removed;
        }

        public int Remove2(T2 key2)
        {
            if (!_indices[1].TryGetValue(key2, out var indices2) || indices2.Count == 0)
                return 0;
            foreach (var indices in _collapsedKeys)
            {
                indices.ExceptWith(indices2);
            }
            foreach (var dict in _indices)
            {
                foreach (var indices in dict.Values)
                {
                    if (indices == indices2)
                        continue;
                    indices.ExceptWith(indices2);
                }
            }
            var removed = indices2.Count;
            _unusedIndices.UnionWith(indices2);
            indices2.Clear();
            CheckCompact();
            return removed;
        }

        public int Remove3(T3 key3)
        {
            if (!_indices[2].TryGetValue(key3, out var indices3) || indices3.Count == 0)
                return 0;
            foreach (var indices in _collapsedKeys)
            {
                indices.ExceptWith(indices3);
            }
            foreach (var dict in _indices)
            {
                foreach (var indices in dict.Values)
                {
                    if (indices == indices3)
                        continue;
                    indices.ExceptWith(indices3);
                }
            }
            var removed = indices3.Count;
            _unusedIndices.UnionWith(indices3);
            indices3.Clear();
            CheckCompact();
            return removed;
        }

        public int Remove4(T4 key4)
        {
            if (!_indices[3].TryGetValue(key4, out var indices4) || indices4.Count == 0)
                return 0;
            foreach (var indices in _collapsedKeys)
            {
                indices.ExceptWith(indices4);
            }
            foreach (var dict in _indices)
            {
                foreach (var indices in dict.Values)
                {
                    if (indices == indices4)
                        continue;
                    indices.ExceptWith(indices4);
                }
            }
            var removed = indices4.Count;
            _unusedIndices.UnionWith(indices4);
            indices4.Clear();
            CheckCompact();
            return removed;
        }

#if NET472 || NET481 || NETSTANDARD2_1
        public bool TryGetValue(T1 key1, T2 key2, T3 key3, T4 key4, out TValue value)
#else
        public bool TryGetValue(T1 key1, T2 key2, T3 key3, T4 key4, out TValue? value)
#endif
        {
            value = default;
            if (!_indices[0].TryGetValue(key1, out var indices1) || indices1.Count == 0)
                return false;
            if (!_indices[1].TryGetValue(key2, out var indices2) || indices2.Count == 0)
                return false;
            if (!_indices[2].TryGetValue(key3, out var indices3) || indices3.Count == 0)
                return false;
            if (!_indices[3].TryGetValue(key4, out var indices4) || indices4.Count == 0)
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
            if (!_indices[0].TryGetValue(key1, out var ind) || ind.Count == 0)
            {
                throw new ArgumentException($"Key not found {key1}");
            }
            return new IndexedTable<T2, T3, T4, TValue>(
                new[] { _indices[1], _indices[2], _indices[3] },
                _data, _collapsedKeys.Concat(new[] { ind }).ToList(), _unusedIndices);
        }

        public IndexedTable<T1, T3, T4, TValue> Slice2(T2 key2)
        {
            ExceptionHandling.ThrowIfNull(key2, nameof(key2));
            if (!_indices[1].TryGetValue(key2, out var ind) || ind.Count == 0)
            {
                throw new ArgumentException($"Key not found {key2}");
            }
            return new IndexedTable<T1, T3, T4, TValue>(
                new[] { _indices[0], _indices[2], _indices[3] },
                _data, _collapsedKeys.Concat(new[] { ind }).ToList(), _unusedIndices);
        }

        public IndexedTable<T1, T2, T4, TValue> Slice3(T3 key3)
        {
            ExceptionHandling.ThrowIfNull(key3, nameof(key3));
            if (!_indices[2].TryGetValue(key3, out var ind) || ind.Count == 0)
            {
                throw new ArgumentException($"Key not found {key3}");
            }
            return new IndexedTable<T1, T2, T4, TValue>(
                new[] { _indices[0], _indices[1], _indices[3] },
                _data, _collapsedKeys.Concat(new[] { ind }).ToList(), _unusedIndices);
        }

        public IndexedTable<T1, T2, T3, TValue> Slice4(T4 key4)
        {
            ExceptionHandling.ThrowIfNull(key4, nameof(key4));
            if (!_indices[3].TryGetValue(key4, out var ind) || ind.Count == 0)
            {
                throw new ArgumentException($"Key not found {key4}");
            }
            return new IndexedTable<T1, T2, T3, TValue>(
                new[] { _indices[0], _indices[1], _indices[2] },
                _data, _collapsedKeys.Concat(new[] { ind }).ToList(), _unusedIndices);
        }

        public bool ContainsKey1(T1 key1)
        {
            ExceptionHandling.ThrowIfNull(key1, nameof(key1));
            if (!_indices[0].TryGetValue(key1, out var ind) || ind.Count == 0)
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
            if (!_indices[1].TryGetValue(key2, out var ind) || ind.Count == 0)
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
            if (!_indices[2].TryGetValue(key3, out var ind) || ind.Count == 0)
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
            if (!_indices[3].TryGetValue(key4, out var ind) || ind.Count == 0)
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
            if (_indices[0].TryGetValue(key1, out var indices) && indices.Count > 0)
            {
                values = new IndexedTable<T2, T3, T4, TValue>(new[] { _indices[1], _indices[2], _indices[3] },
                _data, _collapsedKeys.Concat(new[] { indices }).ToList(), _unusedIndices);
                return true;
            }
            values = new IndexedTable<T2, T3, T4, TValue>();
            return false;
        }

        public bool TrySlice2(T2 key2, out IndexedTable<T1, T3, T4, TValue> values)
        {
            ExceptionHandling.ThrowIfNull(key2, nameof(key2));
            if (_indices[1].TryGetValue(key2, out var indices) && indices.Count > 0)
            {
                values = new IndexedTable<T1, T3, T4, TValue>(new[] { _indices[0], _indices[2], _indices[3] },
                _data, _collapsedKeys.Concat(new[] { indices }).ToList(), _unusedIndices);
                return true;
            }
            values = new IndexedTable<T1, T3, T4, TValue>();
            return false;
        }

        public bool TrySlice3(T3 key3, out IndexedTable<T1, T2, T4, TValue> values)
        {
            ExceptionHandling.ThrowIfNull(key3, nameof(key3));
            if (_indices[2].TryGetValue(key3, out var indices) && indices.Count > 0)
            {
                values = new IndexedTable<T1, T2, T4, TValue>(new[] { _indices[0], _indices[1], _indices[3] },
                _data, _collapsedKeys.Concat(new[] { indices }).ToList(), _unusedIndices);
                return true;
            }
            values = new IndexedTable<T1, T2, T4, TValue>();
            return false;
        }

        public bool TrySlice4(T4 key4, out IndexedTable<T1, T2, T3, TValue> values)
        {
            ExceptionHandling.ThrowIfNull(key4, nameof(key4));
            if (_indices[3].TryGetValue(key4, out var indices) && indices.Count > 0)
            {
                values = new IndexedTable<T1, T2, T3, TValue>(new[] { _indices[0], _indices[1], _indices[2] },
                _data, _collapsedKeys.Concat(new[] { indices }).ToList(), _unusedIndices);
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
            IList<TValue> data, IList<ISet<int>> collapsedKeys, ISet<int> unusedIndices) : base(indices, data, collapsedKeys, unusedIndices) { }
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
                if (!_indices[0].TryGetValue(key1, out var indices1) || indices1.Count == 0)
                    throw new ArgumentException($"Key not found {key1}");
                if (!_indices[1].TryGetValue(key2, out var indices2) || indices2.Count == 0)
                    throw new ArgumentException($"Key not found {key2}");
                if (!_indices[2].TryGetValue(key3, out var indices3) || indices3.Count == 0)
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
                    if (_unusedIndices.Count > 0)
                    {
                        // reuse an unused index
                        index = _unusedIndices.First();
                        _unusedIndices.Remove(index);
                        _data[index] = value;
                    }
                    else
                    {
                        // make an insert at the end
                        index = _data.Count;
                        _data.Insert(index, value);
                    }
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

        public int Remove1(T1 key1)
        {
            if (!_indices[0].TryGetValue(key1, out var indices1) || indices1.Count == 0)
                return 0;
            foreach (var indices in _collapsedKeys)
            {
                indices.ExceptWith(indices1);
            }
            foreach (var dict in _indices)
            {
                foreach (var indices in dict.Values)
                {
                    if (indices == indices1)
                        continue;
                    indices.ExceptWith(indices1);
                }
            }
            var removed = indices1.Count;
            _unusedIndices.UnionWith(indices1);
            indices1.Clear();
            CheckCompact();
            return removed;
        }

        public int Remove2(T2 key2)
        {
            if (!_indices[1].TryGetValue(key2, out var indices2) || indices2.Count == 0)
                return 0;
            foreach (var indices in _collapsedKeys)
            {
                indices.ExceptWith(indices2);
            }
            foreach (var dict in _indices)
            {
                foreach (var indices in dict.Values)
                {
                    if (indices == indices2)
                        continue;
                    indices.ExceptWith(indices2);
                }
            }
            var removed = indices2.Count;
            _unusedIndices.UnionWith(indices2);
            indices2.Clear();
            CheckCompact();
            return removed;
        }

        public int Remove3(T3 key3)
        {
            if (!_indices[2].TryGetValue(key3, out var indices3) || indices3.Count == 0)
                return 0;
            foreach (var indices in _collapsedKeys)
            {
                indices.ExceptWith(indices3);
            }
            foreach (var dict in _indices)
            {
                foreach (var indices in dict.Values)
                {
                    if (indices == indices3)
                        continue;
                    indices.ExceptWith(indices3);
                }
            }
            var removed = indices3.Count;
            _unusedIndices.UnionWith(indices3);
            indices3.Clear();
            CheckCompact();
            return removed;
        }

#if NET472 || NET481 || NETSTANDARD2_1
        public bool TryGetValue(T1 key1, T2 key2, T3 key3, out TValue value)
#else
        public bool TryGetValue(T1 key1, T2 key2, T3 key3, out TValue? value)
#endif
        {
            value = default;
            if (!_indices[0].TryGetValue(key1, out var indices1) || indices1.Count == 0)
                return false;
            if (!_indices[1].TryGetValue(key2, out var indices2) || indices2.Count == 0)
                return false;
            if (!_indices[2].TryGetValue(key3, out var indices3) || indices3.Count == 0)
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
            if (!_indices[0].TryGetValue(key1, out var ind) || ind.Count == 0)
            {
                throw new ArgumentException($"Key not found {key1}");
            }
            return new IndexedTable<T2, T3, TValue>(
                new[] { _indices[1], _indices[2] },
                _data, _collapsedKeys.Concat(new[] { ind }).ToList(), _unusedIndices);
        }

        public IndexedTable<T1, T3, TValue> Slice2(T2 key2)
        {
            ExceptionHandling.ThrowIfNull(key2, nameof(key2));
            if (!_indices[1].TryGetValue(key2, out var ind) || ind.Count == 0)
            {
                throw new ArgumentException($"Key not found {key2}");
            }
            return new IndexedTable<T1, T3, TValue>(
                new[] { _indices[0], _indices[2] },
                _data, _collapsedKeys.Concat(new[] { ind }).ToList(), _unusedIndices);
        }

        public IndexedTable<T1, T2, TValue> Slice3(T3 key3)
        {
            ExceptionHandling.ThrowIfNull(key3, nameof(key3));
            if (!_indices[2].TryGetValue(key3, out var ind) || ind.Count == 0)
            {
                throw new ArgumentException($"Key not found {key3}");
            }
            return new IndexedTable<T1, T2, TValue>(
                new[] { _indices[0], _indices[1] },
                _data, _collapsedKeys.Concat(new[] { ind }).ToList(), _unusedIndices);
        }

        public bool ContainsKey1(T1 key1)
        {
            ExceptionHandling.ThrowIfNull(key1, nameof(key1));
            if (!_indices[0].TryGetValue(key1, out var ind) || ind.Count == 0)
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
            if (!_indices[1].TryGetValue(key2, out var ind) || ind.Count == 0)
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
            if (!_indices[2].TryGetValue(key3, out var ind) || ind.Count == 0)
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
            if (_indices[0].TryGetValue(key1, out var indices) && indices.Count > 0)
            {
                values = new IndexedTable<T2, T3, TValue>(new[] { _indices[1], _indices[2] },
                _data, _collapsedKeys.Concat(new[] { indices }).ToList(), _unusedIndices);
                return true;
            }
            values = new IndexedTable<T2, T3, TValue>();
            return false;
        }

        public bool TrySlice2(T2 key2, out IndexedTable<T1, T3, TValue> values)
        {
            ExceptionHandling.ThrowIfNull(key2, nameof(key2));
            if (_indices[1].TryGetValue(key2, out var indices) && indices.Count > 0)
            {
                values = new IndexedTable<T1, T3, TValue>(new[] { _indices[0], _indices[2] },
                _data, _collapsedKeys.Concat(new[] { indices }).ToList(), _unusedIndices);
                return true;
            }
            values = new IndexedTable<T1, T3, TValue>();
            return false;
        }

        public bool TrySlice3(T3 key3, out IndexedTable<T1, T2, TValue> values)
        {
            ExceptionHandling.ThrowIfNull(key3, nameof(key3));
            if (_indices[2].TryGetValue(key3, out var indices) && indices.Count > 0)
            {
                values = new IndexedTable<T1, T2, TValue>(new[] { _indices[0], _indices[1] },
                _data, _collapsedKeys.Concat(new[] { indices }).ToList(), _unusedIndices);
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
            IList<TValue> data, IList<ISet<int>> collapsedKeys, ISet<int> unusedIndices) : base(indices, data, collapsedKeys, unusedIndices) { }
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
                if (!_indices[0].TryGetValue(key1, out var indices1) || indices1.Count == 0)
                    throw new ArgumentException($"Key not found {key1}");
                if (!_indices[1].TryGetValue(key2, out var indices2) || indices2.Count == 0)
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
                    if (_unusedIndices.Count > 0)
                    {
                        // reuse an unused index
                        index = _unusedIndices.First();
                        _unusedIndices.Remove(index);
                        _data[index] = value;
                    }
                    else
                    {
                        // make an insert at the end
                        index = _data.Count;
                        _data.Insert(index, value);
                    }
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

        public int Remove1(T1 key1)
        {
            if (!_indices[0].TryGetValue(key1, out var indices1) || indices1.Count == 0)
                return 0;
            foreach (var indices in _collapsedKeys)
            {
                indices.ExceptWith(indices1);
            }
            foreach (var dict in _indices)
            {
                foreach (var indices in dict.Values)
                {
                    if (indices == indices1)
                        continue;
                    indices.ExceptWith(indices1);
                }
            }
            var removed = indices1.Count;
            _unusedIndices.UnionWith(indices1);
            indices1.Clear();
            CheckCompact();
            return removed;
        }

        public int Remove2(T2 key2)
        {
            if (!_indices[1].TryGetValue(key2, out var indices2) || indices2.Count == 0)
                return 0;
            foreach (var indices in _collapsedKeys)
            {
                indices.ExceptWith(indices2);
            }
            foreach (var dict in _indices)
            {
                foreach (var indices in dict.Values)
                {
                    if (indices == indices2)
                        continue;
                    indices.ExceptWith(indices2);
                }
            }
            var removed = indices2.Count;
            _unusedIndices.UnionWith(indices2);
            indices2.Clear();
            CheckCompact();
            return removed;
        }

#if NET472 || NET481 || NETSTANDARD2_1
        public bool TryGetValue(T1 key1, T2 key2, out TValue value)
#else
        public bool TryGetValue(T1 key1, T2 key2, out TValue? value)
#endif
        {
            value = default;
            if (!_indices[0].TryGetValue(key1, out var indices1) || indices1.Count == 0)
                return false;
            if (!_indices[1].TryGetValue(key2, out var indices2) || indices2.Count == 0)
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
            if (!_indices[0].TryGetValue(key1, out var ind) || ind.Count == 0)
            {
                throw new ArgumentException($"Key not found {key1}");
            }
            return new IndexedTable<T2, TValue>(
                new[] { _indices[1] },
                _data, _collapsedKeys.Concat(new[] { ind }).ToList(), _unusedIndices);
        }

        public IndexedTable<T1, TValue> Slice2(T2 key2)
        {
            ExceptionHandling.ThrowIfNull(key2, nameof(key2));
            if (!_indices[1].TryGetValue(key2, out var ind) || ind.Count == 0)
            {
                throw new ArgumentException($"Key not found {key2}");
            }
            return new IndexedTable<T1, TValue>(
                new[] { _indices[0] },
                _data, _collapsedKeys.Concat(new[] { ind }).ToList(), _unusedIndices);
        }

        public bool ContainsKey1(T1 key1)
        {
            ExceptionHandling.ThrowIfNull(key1, nameof(key1));
            if (!_indices[0].TryGetValue(key1, out var ind) || ind.Count == 0)
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
            if (!_indices[1].TryGetValue(key2, out var ind) || ind.Count == 0)
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
            if (_indices[0].TryGetValue(key1, out var indices) && indices.Count > 0)
            {
                values = new IndexedTable<T2, TValue>(new[] { _indices[1] },
                _data, _collapsedKeys.Concat(new[] { indices }).ToList(), _unusedIndices);
                return true;
            }
            values = new IndexedTable<T2, TValue>();
            return false;
        }

        public bool TrySlice2(T2 key2, out IndexedTable<T1, TValue> values)
        {
            ExceptionHandling.ThrowIfNull(key2, nameof(key2));
            if (_indices[1].TryGetValue(key2, out var indices) && indices.Count > 0)
            {
                values = new IndexedTable<T1, TValue>(new[] { _indices[0] },
                _data, _collapsedKeys.Concat(new[] { indices }).ToList(), _unusedIndices);
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
            IList<TValue> data, IList<ISet<int>> collapsedKeys, ISet<int> unusedIndices) : base(indices, data, collapsedKeys, unusedIndices) { }
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
                if (!_indices[0].TryGetValue(key1, out var indices1) || indices1.Count == 0)
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
                    if (_unusedIndices.Count > 0)
                    {
                        // reuse an unused index
                        index = _unusedIndices.First();
                        _unusedIndices.Remove(index);
                        _data[index] = value;
                    }
                    else
                    {
                        // make an insert at the end
                        index = _data.Count;
                        _data.Insert(index, value);
                    }
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

#if NET472 || NET481 || NETSTANDARD2_1
        public bool TryGetValue(T1 key1, out TValue value)
#else
        public bool TryGetValue(T1 key1, out TValue? value)
#endif
        {
            value = default;
            if (!_indices[0].TryGetValue(key1, out var indices1) || indices1.Count == 0)
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
            if (!_indices[0].TryGetValue(key1, out var ind) || ind.Count == 0)
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