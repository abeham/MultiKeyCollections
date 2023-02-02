using Xunit;

namespace MultiKeyCollections.Tests;

public class ToDictionaryTests
{
    [Fact]
    public void TestToDictionary()
    {
        var dict5 = new MultiKeyDictionary<string, string, string, string, string, int>()
        {
            { ("a1", "b1", "c1", "d1", "e1"), 1 },
            { ("a1", "b2", "c1", "d2", "e2"), 2 },
            { ("a1", "b3", "c2", "d1", "e3"), 3 }
        };
        var expected5 = new Dictionary<(string, string, string, string, string), int>()
        {
            { ("a1", "b1", "c1", "d1", "e1"), 1 },
            { ("a1", "b2", "c1", "d2", "e2"), 2 },
            { ("a1", "b3", "c2", "d1", "e3"), 3 }
        };
        Assert.Equal(expected5, dict5.ToDictionary());

        var dict4 = new MultiKeyDictionary<string, string, string, string, int>()
        {
            { ("a1", "b1", "c1", "d1"), 1 },
            { ("a1", "b2", "c1", "d2"), 2 },
            { ("a1", "b3", "c2", "d1"), 3 }
        };
        var expected4 = new Dictionary<(string, string, string, string), int>()
        {
            { ("a1", "b1", "c1", "d1"), 1 },
            { ("a1", "b2", "c1", "d2"), 2 },
            { ("a1", "b3", "c2", "d1"), 3 }
        };
        Assert.Equal(expected4, dict4.ToDictionary());

        var dict3 = new MultiKeyDictionary<string, string, string, int>()
        {
            { ("a1", "b1", "c1"), 1 },
            { ("a1", "b2", "c1"), 2 },
            { ("a1", "b3", "c2"), 3 }
        };
        var expected3 = new Dictionary<(string, string, string), int>()
        {
            { ("a1", "b1", "c1"), 1 },
            { ("a1", "b2", "c1"), 2 },
            { ("a1", "b3", "c2"), 3 }
        };
        Assert.Equal(expected3, dict3.ToDictionary());

        var dict2 = new MultiKeyDictionary<string, string, int>()
        {
            { ("a1", "b1"), 1 },
            { ("a1", "b2"), 2 },
            { ("a1", "b3"), 3 }
        };
        var expected2 = new Dictionary<(string, string), int>()
        {
            { ("a1", "b1"), 1 },
            { ("a1", "b2"), 2 },
            { ("a1", "b3"), 3 }
        };
        Assert.Equal(expected2, dict2.ToDictionary());

        var dict = new MultiKeyDictionary<string, int>()
        {
            { "a", 1 },
            { "b", 2 },
            { "c", 3 }
        };
        var expected = new Dictionary<string, int>()
        {
            { "a", 1 },
            { "b", 2 },
            { "c", 3 }
        };
        Assert.Equal(expected, dict.ToDictionary());
    }
}