# MultiKeyCollections

This library adds the `MultiKeyCollections<TKey1, TKey2, ..., TValue>` class with up to 5 keys.

This class is similar to a `Dictionary<TKey, TValue>` but with multiple keys. However, it is more complex than a `Dictionary<(T1, T2), TValue>` which cannot be queried for just one key. MultiKeyDictionaries allow to be sliced, e.g., `dict.Slice1(key1)` or `dict.Slice2(key2)`, which returns a lower dimensional representation, i.e., a `MultiKeyDictionary` with one less generic key type. This lower dimensional representation still supports data manipulation which will affect the higher dimensional representation as well. That means any slice shares the data with any other and with the full representation.

The class internally holds all data values in a single list and uses a `Dictionary<TKey, ISet<int>>` per key to map keys to indices in the list. When a slice is created, the data is not copied. It is possible to add and remove data from the dictionary, however when removing data, the list of data is not necessarily shortened. Instead, the index is marked unused and can be subsequently re-used when another `Add` occurs. When the dictionary has recorded at least 1000 unused indices with a size of the data array of at least 2000, a compacting step will be performed upon the next `Remove`. However, the actual keys are never removed. Like with most dictionary implementations, the order of the data returned when enumerated is not guaranteed and should not be relied upon.

There are a range of unit tests which achieve a coverage of 87% of the lines.

For a full N x M dictionary, the amount of memory required is 3 x N x M, because there will be N x M indices stored for each key in addition to the N x M data values. Creating a slice does not increase the memory usage, except for a very small overhead. So slices are very cheap.

The class is not thread-safe.

## Example

```csharp
// in this example, three keys are defined (int, string, string) and the value is an int.
var dict = new MultiKeyCollections<int, string, string, int>();
dict.Add(1, "a", "x", 100);
dict.Add(1, "a", "y", 200);
dict.Add(1, "b", "x", 300);
dict.Add(1, "b", "y", 400);

dict.Slice2("a").Add(2, "x", 500);
dict.Contains(2, "a", "x"); // true
dict.Contains(2, "b", "x"); // false
dict[2, "a", "x"]; // 500

var slice = dict.Slice1(1);
slice["c", "z"] = 600;
dict.Contains(1, "c", "z"); // true
dict[1, "c", "z"]; // 600

dict.ToList(); // returns a list of all values { 100, 200, 300, 400, 500, 600 } (order not guaranteed)
dict.Slice2("b").ToList(); // returns a list of all values { 300, 400 } (order not guaranteed)

// Slices contain a view of the data and also hold only a reference to the indices. Thus, adding to the
// higher level representation, is also reflected in any slice:
dict.Add(1, "c", "x", 700); // the key (1, "c", "x") was not present when slice was created
slice.Contains("c", "x"); // true
slice["c", "x"]; // 700
```