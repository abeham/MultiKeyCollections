namespace TableCollections.Tests;

using Xunit;

public class AddTests
{
    [Fact]
    public void TestAdd()
    {
        var table5 = new IndexedTable<string, string, string, string, string, int>();
        table5.Add(("a1", "b1", "c1", "d1", "e1"), 1);
        Assert.Equal(1, table5.Count);
        Assert.Equal(1, table5[("a1", "b1", "c1", "d1", "e1")]);
        table5.Add(("a1", "b2", "c1", "d2", "e2"), 2);
        Assert.Equal(2, table5.Count);
        Assert.Equal(2, table5[("a1", "b2", "c1", "d2", "e2")]);
        table5.Add(("a1", "b3", "c2", "d1", "e3"), 3);
        Assert.Equal(3, table5.Count);
        Assert.Equal(3, table5[("a1", "b3", "c2", "d1", "e3")]);

        var table4 = new IndexedTable<string, string, string, string, int>();
        table4.Add(("a1", "b1", "c1", "d1"), 1);
        Assert.Equal(1, table4.Count);
        Assert.Equal(1, table4[("a1", "b1", "c1", "d1")]);
        table4.Add(("a1", "b2", "c1", "d2"), 2);
        Assert.Equal(2, table4.Count);
        Assert.Equal(2, table4[("a1", "b2", "c1", "d2")]);
        table4.Add(("a1", "b3", "c2", "d1"), 3);
        Assert.Equal(3, table4.Count);
        Assert.Equal(3, table4[("a1", "b3", "c2", "d1")]);

        var table3 = new IndexedTable<string, string, string, int>();
        table3.Add(("a1", "b1", "c1"), 1);
        Assert.Equal(1, table3.Count);
        Assert.Equal(1, table3[("a1", "b1", "c1")]);
        table3.Add(("a1", "b2", "c1"), 2);
        Assert.Equal(2, table3.Count);
        Assert.Equal(2, table3[("a1", "b2", "c1")]);
        table3.Add(("a1", "b3", "c2"), 3);
        Assert.Equal(3, table3.Count);
        Assert.Equal(3, table3[("a1", "b3", "c2")]);

        var table2 = new IndexedTable<string, string, int>();
        table2.Add(("a1", "b1"), 1);
        Assert.Equal(1, table2.Count);
        Assert.Equal(1, table2[("a1", "b1")]);
        table2.Add(("a1", "b2"), 2);
        Assert.Equal(2, table2.Count);
        Assert.Equal(2, table2[("a1", "b2")]);
        table2.Add(("a1", "b3"), 3);
        Assert.Equal(3, table2.Count);
        Assert.Equal(3, table2[("a1", "b3")]);

    }
}