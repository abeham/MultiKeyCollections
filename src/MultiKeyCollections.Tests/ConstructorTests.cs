using Xunit;

namespace MultiKeyCollections.Tests;

public class ConstructorTests
{
    [Fact]
    public void TestDefaultConstructor()
    {
        var dict5 = new MultiKeyDictionary<string, string, string, string, string, int>();
        Assert.Equal(0, dict5.Count);
        var dict4 = new MultiKeyDictionary<string, string, string, string, int>();
        Assert.Equal(0, dict4.Count);
        var dict3 = new MultiKeyDictionary<string, string, string, int>();
        Assert.Equal(0, dict3.Count);
        var dict2 = new MultiKeyDictionary<string, string, int>();
        Assert.Equal(0, dict2.Count);
        var dict1 = new MultiKeyDictionary<string, int>();
        Assert.Equal(0, dict1.Count);
    }

    [Fact]
    public void TestConstructorKeyValuePair()
    {
        var dict5 = new MultiKeyDictionary<string, string, string, string, string, int>(
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

        Assert.Equal(expected5, dict5.Enumerate());

        var dict4 = new MultiKeyDictionary<string, string, string, string, int>(
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

        Assert.Equal(expected4, dict4.Enumerate());

        var dict3 = new MultiKeyDictionary<string, string, string, int>(
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

        Assert.Equal(expected3, dict3.Enumerate());

        var dict2 = new MultiKeyDictionary<string, string, int>(
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

        Assert.Equal(expected2, dict2.Enumerate());

        var dict1 = new MultiKeyDictionary<string, int>(
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

        Assert.Equal(expected1, dict1.Enumerate());
    }

    [Fact]
    public void TestConstructorTuple()
    {
        var dict5 = new MultiKeyDictionary<string, string, string, string, string, int>(
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

        Assert.Equal(expected5, dict5.Enumerate());

        var dict4 = new MultiKeyDictionary<string, string, string, string, int>(
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

        Assert.Equal(expected4, dict4.Enumerate());

        var dict3 = new MultiKeyDictionary<string, string, string, int>(
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

        Assert.Equal(expected3, dict3.Enumerate());

        var dict2 = new MultiKeyDictionary<string, string, int>(
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

        Assert.Equal(expected2, dict2.Enumerate());

        var dict1 = new MultiKeyDictionary<string, int>(
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

        Assert.Equal(expected1, dict1.Enumerate());
    }
}