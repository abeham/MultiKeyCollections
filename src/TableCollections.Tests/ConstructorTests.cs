using Xunit;

namespace TableCollections.Tests;

public class ConstructorTests
{
    [Fact]
    public void TestDefaultConstructor()
    {
        var table5 = new IndexedTable<string, string, string, string, string, int>();
        Assert.Equal(0, table5.Count);
        var table4 = new IndexedTable<string, string, string, string, int>();
        Assert.Equal(0, table4.Count);
        var table3 = new IndexedTable<string, string, string, int>();
        Assert.Equal(0, table3.Count);
        var table2 = new IndexedTable<string, string, int>();
        Assert.Equal(0, table2.Count);
        var table1 = new IndexedTable<string, int>();
        Assert.Equal(0, table1.Count);
    }

    [Fact]
    public void TestConstructorKeyValuePair()
    {
        var table5 = new IndexedTable<string, string, string, string, string, int>(
            new [] { new KeyValuePair<(string, string, string, string, string), int>(("a", "b", "c", "d", "e"), 1),
                     new KeyValuePair<(string, string, string, string, string), int>(("a", "b", "c", "d", "f"), 2),
                     new KeyValuePair<(string, string, string, string, string), int>(("a", "b", "c", "d", "g"), 3) }
        );

        var expected5 = new List<(string, string, string, string, string, int)>()
        {
            ("a", "b", "c", "d", "e", 1),
            ("a", "b", "c", "d", "f", 2),
            ("a", "b", "c", "d", "g", 3)
        };

        Assert.Equal(expected5, table5.Enumerate());

        var table4 = new IndexedTable<string, string, string, string, int>(
            new [] { new KeyValuePair<(string, string, string, string), int>(("a", "b", "c", "d"), 1),
                     new KeyValuePair<(string, string, string, string), int>(("a", "b", "c", "e"), 2),
                     new KeyValuePair<(string, string, string, string), int>(("a", "b", "c", "f"), 3) }
        );

        var expected4 = new List<(string, string, string, string, int)>()
        {
            ("a", "b", "c", "d", 1),
            ("a", "b", "c", "e", 2),
            ("a", "b", "c", "f", 3)
        };

        Assert.Equal(expected4, table4.Enumerate());

        var table3 = new IndexedTable<string, string, string, int>(
            new [] { new KeyValuePair<(string, string, string), int>(("a", "b", "c"), 1),
                     new KeyValuePair<(string, string, string), int>(("a", "b", "d"), 2),
                     new KeyValuePair<(string, string, string), int>(("a", "b", "e"), 3) }
        );

        var expected3 = new List<(string, string, string, int)>()
        {
            ("a", "b", "c", 1),
            ("a", "b", "d", 2),
            ("a", "b", "e", 3)
        };

        Assert.Equal(expected3, table3.Enumerate());

        var table2 = new IndexedTable<string, string, int>(
            new [] { new KeyValuePair<(string, string), int>(("a", "b"), 1),
                     new KeyValuePair<(string, string), int>(("a", "c"), 2),
                     new KeyValuePair<(string, string), int>(("a", "d"), 3) }
        );

        var expected2 = new List<(string, string, int)>()
        {
            ("a", "b", 1),
            ("a", "c", 2),
            ("a", "d", 3)
        };

        Assert.Equal(expected2, table2.Enumerate());

        var table1 = new IndexedTable<string, int>(
            new [] { new KeyValuePair<string, int>(("a"), 1),
                     new KeyValuePair<string, int>(("b"), 2),
                     new KeyValuePair<string, int>(("c"), 3) }
        );

        var expected1 = new List<(string, int)>()
        {
            ("a", 1),
            ("b", 2),
            ("c", 3)
        };

        Assert.Equal(expected1, table1.Enumerate());
    }

    [Fact]
    public void TestConstructorTuple()
    {
        var table5 = new IndexedTable<string, string, string, string, string, int>(
            new [] { ("a", "b", "c", "d", "e", 1),
                     ("a", "b", "c", "d", "f", 2),
                     ("a", "b", "c", "d", "g", 3) }
        );

        var expected5 = new List<(string, string, string, string, string, int)>()
        {
            ("a", "b", "c", "d", "e", 1),
            ("a", "b", "c", "d", "f", 2),
            ("a", "b", "c", "d", "g", 3)
        };

        Assert.Equal(expected5, table5.Enumerate());

        var table4 = new IndexedTable<string, string, string, string, int>(
            new [] { ("a", "b", "c", "d", 1),
                     ("a", "b", "c", "e", 2),
                     ("a", "b", "c", "f", 3) }
        );

        var expected4 = new List<(string, string, string, string, int)>()
        {
            ("a", "b", "c", "d", 1),
            ("a", "b", "c", "e", 2),
            ("a", "b", "c", "f", 3)
        };

        Assert.Equal(expected4, table4.Enumerate());

        var table3 = new IndexedTable<string, string, string, int>(
            new [] { ("a", "b", "c", 1),
                     ("a", "b", "d", 2),
                     ("a", "b", "e", 3) }
        );

        var expected3 = new List<(string, string, string, int)>()
        {
            ("a", "b", "c", 1),
            ("a", "b", "d", 2),
            ("a", "b", "e", 3)
        };

        Assert.Equal(expected3, table3.Enumerate());

        var table2 = new IndexedTable<string, string, int>(
            new [] { ("a", "b", 1),
                     ("a", "c", 2),
                     ("a", "d", 3) }
        );

        var expected2 = new List<(string, string, int)>()
        {
            ("a", "b", 1),
            ("a", "c", 2),
            ("a", "d", 3)
        };

        Assert.Equal(expected2, table2.Enumerate());

        var table1 = new IndexedTable<string, int>(
            new [] { ("a", 1),
                     ("b", 2),
                     ("c", 3) }
        );

        var expected1 = new List<(string, int)>()
        {
            ("a", 1),
            ("b", 2),
            ("c", 3)
        };

        Assert.Equal(expected1, table1.Enumerate());
    }
}