using Xunit;

namespace TableCollections.Tests;

public class TrySliceXTests
{
    [Fact]
    public void TestTrySliceX()
    {
        // Generate TrySlice5 to TrySlice1 Tests for IndexedTable from 5 to 4 columns
        var table5 = new IndexedTable<string, string, string, string, string, int>();
        table5["a", "b", "c", "d", "e"] = 1;
        table5["a", "b", "c", "d", "f"] = 2;
        table5["a", "b", "c", "d", "g"] = 3;
        Assert.True(table5.TrySlice1("a", out var _));
        Assert.False(table5.TrySlice1("b", out _));
        Assert.True(table5.TrySlice2("b", out _));
        Assert.False(table5.TrySlice2("c", out _));
        Assert.True(table5.TrySlice3("c", out _));
        Assert.False(table5.TrySlice3("d", out _));
        Assert.True(table5.TrySlice4("d", out _));
        Assert.False(table5.TrySlice4("e", out _));
        Assert.True(table5.TrySlice5("e", out _));
        Assert.True(table5.TrySlice5("f", out _));
        Assert.True(table5.TrySlice5("g", out _));
        Assert.False(table5.TrySlice5("h", out _));

        var table4 = new IndexedTable<string, string, string, string, int>();
        table4["a", "b", "c", "d"] = 1;
        table4["a", "b", "c", "e"] = 2;
        table4["a", "b", "c", "f"] = 3;

        Assert.True(table4.TrySlice1("a", out var _));
        Assert.False(table4.TrySlice1("b", out _));
        Assert.True(table4.TrySlice2("b", out _));
        Assert.False(table4.TrySlice2("c", out _));
        Assert.True(table4.TrySlice3("c", out _));
        Assert.False(table4.TrySlice3("d", out _));
        Assert.True(table4.TrySlice4("d", out _));
        Assert.True(table4.TrySlice4("e", out _));
        Assert.True(table4.TrySlice4("f", out _));
        Assert.False(table4.TrySlice4("g", out _));

        var table3 = new IndexedTable<string, string, string, int>();
        table3["a", "b", "c"] = 1;
        table3["a", "b", "d"] = 2;
        table3["a", "b", "e"] = 3;

        Assert.True(table3.TrySlice1("a", out var _));
        Assert.False(table3.TrySlice1("b", out _));
        Assert.True(table3.TrySlice2("b", out _));
        Assert.False(table3.TrySlice2("c", out _));
        Assert.True(table3.TrySlice3("c", out _));
        Assert.True(table3.TrySlice3("d", out _));
        Assert.True(table3.TrySlice3("e", out _));
        Assert.False(table3.TrySlice3("f", out _));

        var table2 = new IndexedTable<string, string, int>();
        table2["a", "b"] = 1;
        table2["a", "c"] = 2;
        table2["a", "d"] = 3;

        Assert.True(table2.TrySlice1("a", out var _));
        Assert.False(table2.TrySlice1("b", out _));
        Assert.True(table2.TrySlice2("b", out _));
        Assert.True(table2.TrySlice2("c", out _));
        Assert.True(table2.TrySlice2("d", out _));
        Assert.False(table2.TrySlice2("e", out _));
    }
}