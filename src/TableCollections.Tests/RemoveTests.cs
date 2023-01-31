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

        var table4 = new IndexedTable<string, string, string, string, int>();
        table4["a1", "b1", "c1", "d1"] = 1;
        table4["a1", "b2", "c1", "d2"] = 2;
        table4["a1", "b3", "c2", "d1"] = 3;
        table4["a2", "b1", "c2", "d2"] = 4;
        table4["a2", "b2", "c3", "d1"] = 5;
        table4["a2", "b3", "c3", "d2"] = 6;
        table4["a3", "b1", "c1", "d1"] = 7;
        table4["a3", "b2", "c2", "d2"] = 8;
        table4["a3", "b3", "c3", "d1"] = 9;

        Assert.True(table4.Remove("a1", "b1", "c1", "d1"));
        Assert.Equal(8, table4.Count);
        Assert.False(table4.Contains("a1", "b1", "c1", "d1"));
        Assert.False(table4.Remove("a1", "b1", "c1", "d1"));
        Assert.True(table4.Remove("a2", "b2", "c3", "d1"));
        Assert.Equal(7, table4.Count);

        var slice34 = table4.Slice1("a3");
        slice34.Remove("b2", "c2", "d2");
        Assert.Equal(6, table4.Count);
        Assert.Equal(2, slice34.Count);
        Assert.False(table4.Contains("a3", "b2", "c2", "d2"));
        Assert.False(slice34.Contains("b2", "c2", "d2"));

        table4.Add("a3", "b2", "c2", "d2", 8);
        Assert.Equal(7, table4.Count);
        Assert.Equal(3, slice34.Count);
        Assert.True(table4.Contains("a3", "b2", "c2", "d2"));
        Assert.True(slice34.Contains("b2", "c2", "d2"));

        slice34 = table4.Slice1("a1");
        slice34.Add("b1", "c1", "d1", 1);
        Assert.Equal(8, table4.Count);
        Assert.Equal(3, slice34.Count);
        Assert.True(table4.Contains("a1", "b1", "c1", "d1"));
        Assert.True(slice34.Contains("b1", "c1", "d1"));

        var table3 = new IndexedTable<string, string, string, int>();
        table3["a1", "b1", "c1"] = 1;
        table3["a1", "b2", "c1"] = 2;
        table3["a1", "b3", "c2"] = 3;
        table3["a2", "b1", "c2"] = 4;
        table3["a2", "b2", "c3"] = 5;
        table3["a2", "b3", "c3"] = 6;
        table3["a3", "b1", "c1"] = 7;
        table3["a3", "b2", "c2"] = 8;
        table3["a3", "b3", "c3"] = 9;

        Assert.True(table3.Remove("a1", "b1", "c1"));
        Assert.Equal(8, table3.Count);
        Assert.False(table3.Contains("a1", "b1", "c1"));
        Assert.False(table3.Remove("a1", "b1", "c1"));
        Assert.True(table3.Remove("a2", "b2", "c3"));
        Assert.Equal(7, table3.Count);

        var slice23 = table3.Slice1("a3");
        slice23.Remove("b2", "c2");
        Assert.Equal(6, table3.Count);
        Assert.Equal(2, slice23.Count);
        Assert.False(table3.Contains("a3", "b2", "c2"));
        Assert.False(slice23.Contains("b2", "c2"));

        table3.Add("a3", "b2", "c2", 8);
        Assert.Equal(7, table3.Count);
        Assert.Equal(3, slice23.Count);
        Assert.True(table3.Contains("a3", "b2", "c2"));
        Assert.True(slice23.Contains("b2", "c2"));

        slice23 = table3.Slice1("a1");
        slice23.Add("b1", "c1", 1);
        Assert.Equal(8, table3.Count);
        Assert.Equal(3, slice23.Count);
        Assert.True(table3.Contains("a1", "b1", "c1"));
        Assert.True(slice23.Contains("b1", "c1"));

        var table2 = new IndexedTable<string, string, int>();
        table2["a1", "b1"] = 1;
        table2["a1", "b2"] = 2;
        table2["a1", "b3"] = 3;
        table2["a2", "b1"] = 4;
        table2["a2", "b2"] = 5;
        table2["a2", "b3"] = 6;
        table2["a3", "b1"] = 7;
        table2["a3", "b2"] = 8;
        table2["a3", "b3"] = 9;

        Assert.True(table2.Remove("a1", "b1"));
        Assert.Equal(8, table2.Count);
        Assert.False(table2.Contains("a1", "b1"));
        Assert.False(table2.Remove("a1", "b1"));
        Assert.True(table2.Remove("a2", "b2"));
        Assert.Equal(7, table2.Count);

        var slice12 = table2.Slice1("a3");
        slice12.Remove("b2");
        Assert.Equal(6, table2.Count);
        Assert.Equal(2, slice12.Count);
        Assert.False(table2.Contains("a3", "b2"));
        Assert.False(slice12.Contains("b2"));

        table2.Add("a3", "b2", 8);
        Assert.Equal(7, table2.Count);
        Assert.Equal(3, slice12.Count);
        Assert.True(table2.Contains("a3", "b2"));
        Assert.True(slice12.Contains("b2"));

        slice12 = table2.Slice1("a1");
        Assert.False(slice12.ContainsKey("b1"));
        slice12.Add("b1", 1);
        Assert.Equal(8, table2.Count);
        Assert.Equal(3, slice12.Count);
        Assert.True(table2.Contains("a1", "b1"));
        Assert.True(slice12.Contains("b1"));

        var table1 = new IndexedTable<string, int>();
        table1["a1"] = 1;
        table1["a2"] = 2;
        table1["a3"] = 3;

        Assert.True(table1.Remove("a1"));
        Assert.Equal(2, table1.Count);
        Assert.False(table1.Contains("a1"));
        Assert.False(table1.Remove("a1"));
        Assert.True(table1.Remove("a2"));
        Assert.Equal(1, table1.Count);

        table1.Add("a2", 2);
        Assert.Equal(2, table1.Count);
        Assert.True(table1.Contains("a2"));
    }

    [Fact]
    public void TestCompacting()
    {
        var table5 = new IndexedTable<string, string, string, string, string, int>()
        {
            CompactingAbsoluteThreshold = 0,
            CompactingRelativeThreshold = 0
        };
        table5["a1", "b1", "c1", "d1", "e1"] = 1;
        table5["a1", "b2", "c1", "d2", "e2"] = 2;
        table5["a1", "b3", "c2", "d1", "e3"] = 3;
        table5["a2", "b1", "c2", "d2", "e4"] = 4;
        table5["a2", "b2", "c3", "d1", "e5"] = 5;
        table5["a2", "b3", "c3", "d2", "e6"] = 6;
        table5["a3", "b1", "c1", "d1", "e7"] = 7;
        table5["a3", "b2", "c2", "d2", "e8"] = 8;
        table5["a3", "b3", "c3", "d1", "e9"] = 9;
        
        var unusedIndices = (ISet<int>)typeof(IndexedTable<string, string, string, string, string, int>)
            .GetField("_unusedIndices", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(table5);
        Assert.Equal(9, table5.Count);
        table5.Remove("a1", "b1", "c1", "d1", "e1");
        Assert.Equal(8, table5.Count);
        Assert.Equal(0, unusedIndices.Count);
        table5.Remove("a1", "b2", "c1", "d2", "e2");
        Assert.Equal(7, table5.Count);
        Assert.Equal(0, unusedIndices.Count);
        table5.Remove("a1", "b3", "c2", "d1", "e3");
        Assert.Equal(6, table5.Count);
        Assert.Equal(0, unusedIndices.Count);
        table5.Remove("a2", "b1", "c2", "d2", "e4");
        Assert.Equal(5, table5.Count);
        Assert.Equal(0, unusedIndices.Count);
        table5.Remove("a2", "b2", "c3", "d1", "e5");
        Assert.Equal(4, table5.Count);
        Assert.Equal(0, unusedIndices.Count);
        table5.CompactingAbsoluteThreshold = 2;
        table5.Remove("a2", "b3", "c3", "d2", "e6");
        Assert.Equal(3, table5.Count);
        Assert.Equal(1, unusedIndices.Count);
        table5.Remove("a3", "b1", "c1", "d1", "e7");
        Assert.Equal(2, table5.Count);
        Assert.Equal(2, unusedIndices.Count);
        table5.Remove("a3", "b2", "c2", "d2", "e8");
        Assert.Equal(1, table5.Count);
        Assert.Equal(0, unusedIndices.Count);
        table5.Remove("a3", "b3", "c3", "d1", "e9");
        Assert.Equal(0, table5.Count);
        Assert.Equal(1, unusedIndices.Count);
        table5.Add("a4", "b4", "c4", "d4", "e10", 10);
        Assert.Equal(1, table5.Count);
        Assert.Equal(0, unusedIndices.Count);

        var table4 = new IndexedTable<string, string, string, string, int>()
        {
            CompactingAbsoluteThreshold = 0,
            CompactingRelativeThreshold = 0
        };
        table4["a1", "b1", "c1", "d1"] = 1;
        table4["a1", "b2", "c1", "d2"] = 2;
        table4["a1", "b3", "c2", "d1"] = 3;
        table4["a2", "b1", "c2", "d2"] = 4;
        table4["a2", "b2", "c3", "d1"] = 5;
        table4["a2", "b3", "c3", "d2"] = 6;
        table4["a3", "b1", "c1", "d1"] = 7;
        table4["a3", "b2", "c2", "d2"] = 8;
        table4["a3", "b3", "c3", "d1"] = 9;

        unusedIndices = (ISet<int>)typeof(IndexedTable<string, string, string, string, int>)
            .GetField("_unusedIndices", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(table4);
        Assert.Equal(9, table4.Count);
        table4.Remove("a1", "b1", "c1", "d1");
        Assert.Equal(8, table4.Count);
        Assert.Equal(0, unusedIndices.Count);
        table4.Remove("a1", "b2", "c1", "d2");
        Assert.Equal(7, table4.Count);
        Assert.Equal(0, unusedIndices.Count);
        table4.Remove("a1", "b3", "c2", "d1");
        Assert.Equal(6, table4.Count);
        Assert.Equal(0, unusedIndices.Count);
        table4.Remove("a2", "b1", "c2", "d2");
        Assert.Equal(5, table4.Count);
        Assert.Equal(0, unusedIndices.Count);
        table4.Remove("a2", "b2", "c3", "d1");
        Assert.Equal(4, table4.Count);
        Assert.Equal(0, unusedIndices.Count);
        table4.CompactingAbsoluteThreshold = 2;
        table4.Remove("a2", "b3", "c3", "d2");
        Assert.Equal(3, table4.Count);
        Assert.Equal(1, unusedIndices.Count);
        table4.Remove("a3", "b1", "c1", "d1");
        Assert.Equal(2, table4.Count);
        Assert.Equal(2, unusedIndices.Count);
        table4.Remove("a3", "b2", "c2", "d2");
        Assert.Equal(1, table4.Count);
        Assert.Equal(0, unusedIndices.Count);
        table4.Remove("a3", "b3", "c3", "d1");
        Assert.Equal(0, table4.Count);
        Assert.Equal(1, unusedIndices.Count);
        table4.Add("a4", "b4", "c4", "d4", 10);
        Assert.Equal(1, table4.Count);
        Assert.Equal(0, unusedIndices.Count);

        var table3 = new IndexedTable<string, string, string, int>()
        {
            CompactingAbsoluteThreshold = 0,
            CompactingRelativeThreshold = 0
        };
        table3["a1", "b1", "c1"] = 1;
        table3["a1", "b2", "c2"] = 2;
        table3["a1", "b3", "c3"] = 3;
        table3["a2", "b1", "c1"] = 4;
        table3["a2", "b2", "c2"] = 5;
        table3["a2", "b3", "c3"] = 6;
        table3["a3", "b1", "c1"] = 7;
        table3["a3", "b2", "c2"] = 8;
        table3["a3", "b3", "c3"] = 9;

        unusedIndices = (ISet<int>)typeof(IndexedTable<string, string, string, int>)
            .GetField("_unusedIndices", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(table3);
        Assert.Equal(9, table3.Count);
        table3.Remove("a1", "b1", "c1");
        Assert.Equal(8, table3.Count);
        Assert.Equal(0, unusedIndices.Count);
        table3.Remove("a1", "b2", "c2");
        Assert.Equal(7, table3.Count);
        Assert.Equal(0, unusedIndices.Count);
        table3.Remove("a1", "b3", "c3");
        Assert.Equal(6, table3.Count);
        Assert.Equal(0, unusedIndices.Count);
        table3.Remove("a2", "b1", "c1");
        Assert.Equal(5, table3.Count);
        Assert.Equal(0, unusedIndices.Count);
        table3.Remove("a2", "b2", "c2");
        Assert.Equal(4, table3.Count);
        Assert.Equal(0, unusedIndices.Count);
        table3.CompactingAbsoluteThreshold = 2;
        table3.Remove("a2", "b3", "c3");
        Assert.Equal(3, table3.Count);
        Assert.Equal(1, unusedIndices.Count);
        table3.Remove("a3", "b1", "c1");
        Assert.Equal(2, table3.Count);
        Assert.Equal(2, unusedIndices.Count);
        table3.Remove("a3", "b2", "c2");
        Assert.Equal(1, table3.Count);
        Assert.Equal(0, unusedIndices.Count);
        table3.Remove("a3", "b3", "c3");
        Assert.Equal(0, table3.Count);
        Assert.Equal(1, unusedIndices.Count);
        table3.Add("a4", "b4", "c4", 10);
        Assert.Equal(1, table3.Count);
        Assert.Equal(0, unusedIndices.Count);

        var table2 = new IndexedTable<string, string, int>()
        {
            CompactingAbsoluteThreshold = 0,
            CompactingRelativeThreshold = 0
        };
        table2["a1", "b1"] = 1;
        table2["a1", "b2"] = 2;
        table2["a1", "b3"] = 3;
        table2["a2", "b1"] = 4;
        table2["a2", "b2"] = 5;
        table2["a2", "b3"] = 6;
        table2["a3", "b1"] = 7;
        table2["a3", "b2"] = 8;
        table2["a3", "b3"] = 9;

        unusedIndices = (ISet<int>)typeof(IndexedTable<string, string, int>)
            .GetField("_unusedIndices", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(table2);
        Assert.Equal(9, table2.Count);
        table2.Remove("a1", "b1");
        Assert.Equal(8, table2.Count);
        Assert.Equal(0, unusedIndices.Count);
        table2.Remove("a1", "b2");
        Assert.Equal(7, table2.Count);
        Assert.Equal(0, unusedIndices.Count);
        table2.Remove("a1", "b3");
        Assert.Equal(6, table2.Count);
        Assert.Equal(0, unusedIndices.Count);
        table2.Remove("a2", "b1");
        Assert.Equal(5, table2.Count);
        Assert.Equal(0, unusedIndices.Count);
        table2.Remove("a2", "b2");
        Assert.Equal(4, table2.Count);
        Assert.Equal(0, unusedIndices.Count);
        table2.CompactingAbsoluteThreshold = 2;
        table2.Remove("a2", "b3");
        Assert.Equal(3, table2.Count);
        Assert.Equal(1, unusedIndices.Count);
        table2.Remove("a3", "b1");
        Assert.Equal(2, table2.Count);
        Assert.Equal(2, unusedIndices.Count);
        table2.Remove("a3", "b2");
        Assert.Equal(1, table2.Count);
        Assert.Equal(0, unusedIndices.Count);
        table2.Remove("a3", "b3");
        Assert.Equal(0, table2.Count);
        Assert.Equal(1, unusedIndices.Count);
        table2.Add("a4", "b4", 10);
        Assert.Equal(1, table2.Count);
        Assert.Equal(0, unusedIndices.Count);

        var table1 = new IndexedTable<string, int>()
        {
            CompactingAbsoluteThreshold = 0,
            CompactingRelativeThreshold = 0
        };
        table1["a1"] = 1;
        table1["a2"] = 2;
        table1["a3"] = 3;
        table1["a4"] = 4;
        table1["a5"] = 5;
        table1["a6"] = 6;

        unusedIndices = (ISet<int>)typeof(IndexedTable<string, int>)
            .GetField("_unusedIndices", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(table1);
        Assert.Equal(6, table1.Count);
        table1.Remove("a1");
        Assert.Equal(5, table1.Count);
        Assert.Equal(0, unusedIndices.Count);
        table1.Remove("a2");
        Assert.Equal(4, table1.Count);
        Assert.Equal(0, unusedIndices.Count);
        table1.CompactingAbsoluteThreshold = 2;
        table1.Remove("a3");
        Assert.Equal(3, table1.Count);
        Assert.Equal(1, unusedIndices.Count);
        table1.Remove("a4");
        Assert.Equal(2, table1.Count);
        Assert.Equal(2, unusedIndices.Count);
        table1.Remove("a5");
        Assert.Equal(1, table1.Count);
        Assert.Equal(0, unusedIndices.Count);
        table1.Remove("a6");
        Assert.Equal(0, table1.Count);
        Assert.Equal(1, unusedIndices.Count);
        table1.Add("a7", 7);
        Assert.Equal(1, table1.Count);
        Assert.Equal(0, unusedIndices.Count);
    }

    [Fact]
    public void TestRemoveX()
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

        Assert.Equal(3, table5.Remove1("a1"));
        Assert.Equal(6, table5.Count);
        Assert.Equal(2, table5.Remove2("b1"));
        Assert.Equal(4, table5.Count);
        Assert.Equal(1, table5.Remove3("c2"));
        Assert.Equal(3, table5.Count);
        Assert.Equal(2, table5.Remove4("d1"));
        Assert.Equal(1, table5.Count);
        Assert.Equal(1, table5.Remove5("e6"));
        Assert.Equal(0, table5.Count);

        var table4 = new IndexedTable<string, string, string, string, int>();
        table4["a1", "b1", "c1", "d1"] = 1;
        table4["a1", "b2", "c1", "d2"] = 2;
        table4["a1", "b3", "c2", "d1"] = 3;
        table4["a2", "b1", "c2", "d2"] = 4;
        table4["a2", "b2", "c3", "d1"] = 5;
        table4["a2", "b3", "c3", "d2"] = 6;
        table4["a3", "b1", "c1", "d1"] = 7;
        table4["a3", "b2", "c2", "d2"] = 8;
        table4["a3", "b3", "c3", "d1"] = 9;

        Assert.Equal(3, table4.Remove1("a1"));
        Assert.Equal(6, table4.Count);
        Assert.Equal(2, table4.Remove2("b1"));
        Assert.Equal(4, table4.Count);
        Assert.Equal(1, table4.Remove3("c2"));
        Assert.Equal(3, table4.Count);
        Assert.Equal(2, table4.Remove4("d1"));
        Assert.Equal(1, table4.Count);
        Assert.Equal(1, table4.Remove4("d2"));
        Assert.Equal(0, table4.Count);

        var table3 = new IndexedTable<string, string, string, int>();
        table3["a1", "b1", "c1"] = 1;
        table3["a1", "b2", "c1"] = 2;
        table3["a1", "b3", "c2"] = 3;
        table3["a2", "b1", "c2"] = 4;
        table3["a2", "b2", "c3"] = 5;
        table3["a2", "b3", "c3"] = 6;
        table3["a3", "b1", "c1"] = 7;
        table3["a3", "b2", "c2"] = 8;
        table3["a3", "b3", "c3"] = 9;

        Assert.Equal(3, table3.Remove1("a1"));
        Assert.Equal(6, table3.Count);
        Assert.Equal(2, table3.Remove2("b1"));
        Assert.Equal(4, table3.Count);
        Assert.Equal(1, table3.Remove3("c2"));
        Assert.Equal(3, table3.Count);
        Assert.Equal(3, table3.Remove3("c3"));
        Assert.Equal(0, table3.Count);

        var table2 = new IndexedTable<string, string, int>();
        table2["a1", "b1"] = 1;
        table2["a1", "b2"] = 2;
        table2["a1", "b3"] = 3;
        table2["a2", "b1"] = 4;
        table2["a2", "b2"] = 5;
        table2["a2", "b3"] = 6;
        table2["a3", "b1"] = 7;
        table2["a3", "b2"] = 8;
        table2["a3", "b3"] = 9;

        Assert.Equal(3, table2.Remove1("a1"));
        Assert.Equal(6, table2.Count);
        Assert.Equal(2, table2.Remove2("b1"));
        Assert.Equal(4, table2.Count);
        Assert.Equal(2, table2.Remove2("b2"));
        Assert.Equal(2, table2.Count);
        Assert.Equal(2, table2.Remove2("b3"));
        Assert.Equal(0, table2.Count);
    }
}