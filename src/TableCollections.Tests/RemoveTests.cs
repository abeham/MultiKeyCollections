using Xunit;

namespace TableCollections.Tests;

public class RemoveTests
{
    [Fact]
    public void TestRemove()
    {
        var table5 = new IndexedTable<string, string, string, string, string, int>();
        table5["a1", "b1", "c1", "d1", "e1"] = 1;
        table5["a1", "b2", "c1", "d2", "e2"] = 2;
        table5["a1", "b3", "c2", "d1", "e3"] = 3;
        table5["a2", "b1", "c2", "d2", "e4"] = 4;
        table5["a2", "b2", "c3", "d1", "e5"] = 5;
        table5["a2", "b3", "c3", "d2", "e6"] = 6;
        table5["a3", "b1", "c1", "d1", "e7"] = 7;
        table5["a3", "b2", "c2", "d2", "e8"] = 8;
        table5["a3", "b3", "c3", "d1", "e9"] = 9;

        Assert.True(table5.Remove("a1", "b1", "c1", "d1", "e1"));
        Assert.Equal(8, table5.Count);
        Assert.False(table5.Contains("a1", "b1", "c1", "d1", "e1"));
        Assert.False(table5.Remove("a1", "b1", "c1", "d1", "e1"));
        Assert.True(table5.Remove("a2", "b2", "c3", "d1", "e5"));
        Assert.Equal(7, table5.Count);

        var slice45 = table5.Slice1("a3");
        slice45.Remove("b2", "c2", "d2", "e8");
        Assert.Equal(6, table5.Count);
        Assert.Equal(2, slice45.Count);
        Assert.False(table5.Contains("a3", "b2", "c2", "d2", "e8"));
        Assert.False(slice45.Contains("b2", "c2", "d2", "e8"));

        table5.Add("a3", "b2", "c2", "d2", "e8", 8);
        Assert.Equal(7, table5.Count);
        Assert.Equal(3, slice45.Count);
        Assert.True(table5.Contains("a3", "b2", "c2", "d2", "e8"));
        Assert.True(slice45.Contains("b2", "c2", "d2", "e8"));

        slice45 = table5.Slice1("a1");
        slice45.Add("b1", "c1", "d1", "e1", 1);
        Assert.Equal(8, table5.Count);
        Assert.Equal(3, slice45.Count);
        Assert.True(table5.Contains("a1", "b1", "c1", "d1", "e1"));
        Assert.True(slice45.Contains("b1", "c1", "d1", "e1"));
    }
}