# TableCollections

This library adds the `IndexedTable<TKey1, TKey2, ..., TValue>` class.

This class is similar to a `Dictionary<TKey, TValue>` but with multiple keys. However, it is more complex than a `Dictionary<(T1, T2), TValue>` which cannot be queried for just one key. IndexedTables allow to be sliced, e.g., `table.Slice1(key1)` or `table.Slice2(key2)`, which returns a lower dimensional representation. This lower dimensional representation still supports data manipulation which will affect the higher dimensional representation as well.

The class internally holds all data values in a single list and uses a `Dictionary<TKey, ISet<int>>` per key to map keys to indices in the list. When a slice is created, the data is not copied. It is also not possible to remove data from the table, only adding and overwriting is possible.

For a full N x M table, the amount of memory required is 3 x N x M, because there will be N x M indices stored for each key in addition to the N x M data values. Creating a slice does not increase the memory usage, except for a very small overhead. So slices are very cheap.

The class is not thread-safe.

## Example

```csharp
var table = new IndexedTable<int, string, string, int>();
table.Add(1, "a", "x", 100);
table.Add(1, "a", "y", 200);
table.Add(1, "b", "x", 300);
table.Add(1, "b", "y", 400);

table.Slice2("a").Add(2, "x", 500);
table.Contains(2, "a", "x"); // true
table.Contains(2, "b", "x"); // false
table[2, "a", "x"]; // 500

var slice = table.Slice1(1);
slice["c", "z"] = 600;
table.Contains(1, "c", "z"); // true
table[1, "c", "z"]; // 600

table.ToList(); // returns a list of all values { 100, 200, 300, 400, 500, 600 } (order not guaranteed)
table.Slice2("b").ToList(); // returns a list of all values { 300, 400 } (order not guaranteed)

// Slices contain a view of the data and also hold only a reference to the indices. Thus, adding to the
// higher level representation, is also reflected in any slice:
table.Add(1, "c", "x", 700); // the key (1, "c", "x") was not present when slice was created
slice.Contains("c", "x"); // true
slice["c", "x"]; // 700
```