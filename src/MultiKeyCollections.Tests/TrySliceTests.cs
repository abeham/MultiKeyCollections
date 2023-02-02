using Xunit;

namespace MultiKeyCollections.Tests;

public class TrySliceXTests
{
    [Fact]
    public void TestTrySliceX()
    {
        // Generate TrySlice5 to TrySlice1 Tests for IndexedTable from 5 to 4 columns
        var dict5 = new MultiKeyDictionary<string, string, string, string, string, int>();
        dict5["a", "b", "c", "d", "e"] = 1;
        dict5["a", "b", "c", "d", "f"] = 2;
        dict5["a", "b", "c", "d", "g"] = 3;
        Assert.True(dict5.TrySlice1("a", out var _));
        Assert.False(dict5.TrySlice1("b", out _));
        Assert.True(dict5.TrySlice2("b", out _));
        Assert.False(dict5.TrySlice2("c", out _));
        Assert.True(dict5.TrySlice3("c", out _));
        Assert.False(dict5.TrySlice3("d", out _));
        Assert.True(dict5.TrySlice4("d", out _));
        Assert.False(dict5.TrySlice4("e", out _));
        Assert.True(dict5.TrySlice5("e", out _));
        Assert.True(dict5.TrySlice5("f", out _));
        Assert.True(dict5.TrySlice5("g", out _));
        Assert.False(dict5.TrySlice5("h", out _));

        var dict4 = new MultiKeyDictionary<string, string, string, string, int>();
        dict4["a", "b", "c", "d"] = 1;
        dict4["a", "b", "c", "e"] = 2;
        dict4["a", "b", "c", "f"] = 3;

        Assert.True(dict4.TrySlice1("a", out var _));
        Assert.False(dict4.TrySlice1("b", out _));
        Assert.True(dict4.TrySlice2("b", out _));
        Assert.False(dict4.TrySlice2("c", out _));
        Assert.True(dict4.TrySlice3("c", out _));
        Assert.False(dict4.TrySlice3("d", out _));
        Assert.True(dict4.TrySlice4("d", out _));
        Assert.True(dict4.TrySlice4("e", out _));
        Assert.True(dict4.TrySlice4("f", out _));
        Assert.False(dict4.TrySlice4("g", out _));

        var dict3 = new MultiKeyDictionary<string, string, string, int>();
        dict3["a", "b", "c"] = 1;
        dict3["a", "b", "d"] = 2;
        dict3["a", "b", "e"] = 3;

        Assert.True(dict3.TrySlice1("a", out var _));
        Assert.False(dict3.TrySlice1("b", out _));
        Assert.True(dict3.TrySlice2("b", out _));
        Assert.False(dict3.TrySlice2("c", out _));
        Assert.True(dict3.TrySlice3("c", out _));
        Assert.True(dict3.TrySlice3("d", out _));
        Assert.True(dict3.TrySlice3("e", out _));
        Assert.False(dict3.TrySlice3("f", out _));

        var dict2 = new MultiKeyDictionary<string, string, int>();
        dict2["a", "b"] = 1;
        dict2["a", "c"] = 2;
        dict2["a", "d"] = 3;

        Assert.True(dict2.TrySlice1("a", out var _));
        Assert.False(dict2.TrySlice1("b", out _));
        Assert.True(dict2.TrySlice2("b", out _));
        Assert.True(dict2.TrySlice2("c", out _));
        Assert.True(dict2.TrySlice2("d", out _));
        Assert.False(dict2.TrySlice2("e", out _));
    }
}