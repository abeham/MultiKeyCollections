# TableCollections

This library adds the `IndexedTable<TKey1, TKey2, ..., TValue>` class.

This class is a table that can be indexed by multiple keys. It is similar to a `Dictionary<TKey, TValue>` but with multiple keys. It allows to be sliced, e.g., `table.Slice1(key1)` or `table.Slice2(key2)`, which returns a lower dimensional representation and supports quering as well as manipulation.

The class internally holds all data values in a single list and uses a `Dictionary<TKey, ISet<int>>` to map keys to indices in the list.

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
```