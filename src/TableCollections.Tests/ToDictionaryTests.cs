using Xunit;

namespace TableCollections.Tests;

public class ToDictionaryTests
{
    [Fact]
    public void TestToDictionary()
    {
        var table = new IndexedTable<string, int>()
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

        Assert.Equal(expected, table.ToDictionary());
    }
}