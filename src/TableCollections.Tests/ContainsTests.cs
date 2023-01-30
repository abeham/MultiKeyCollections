using System.Collections.Generic;
using Xunit;

namespace TableCollections.Tests;

public class ContainsTests
{
    [Fact]
    public void TestContains()
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

        Assert.True(table5.Contains("a1", "b1", "c1", "d1", "e1"));
        Assert.True(table5.Contains("a1", "b2", "c1", "d2", "e2"));
        Assert.True(table5.Contains("a1", "b3", "c2", "d1", "e3"));
        Assert.True(table5.Contains("a2", "b1", "c2", "d2", "e4"));
        Assert.True(table5.Contains("a2", "b2", "c3", "d1", "e5"));
        Assert.True(table5.Contains("a2", "b3", "c3", "d2", "e6"));
        Assert.True(table5.Contains("a3", "b1", "c1", "d1", "e7"));
        Assert.True(table5.Contains("a3", "b2", "c2", "d2", "e8"));
        Assert.True(table5.Contains("a3", "b3", "c3", "d1", "e9"));
        Assert.True(table5.ContainsKey1("a1"));
        Assert.True(table5.ContainsKey1("a2"));
        Assert.True(table5.ContainsKey1("a3"));
        Assert.True(table5.ContainsKey2("b1"));
        Assert.True(table5.ContainsKey2("b2"));
        Assert.True(table5.ContainsKey2("b3"));
        Assert.True(table5.ContainsKey3("c1"));
        Assert.True(table5.ContainsKey3("c2"));
        Assert.True(table5.ContainsKey3("c3"));
        Assert.True(table5.ContainsKey4("d1"));
        Assert.True(table5.ContainsKey4("d2"));
        Assert.True(table5.ContainsKey5("e1"));
        Assert.True(table5.ContainsKey5("e2"));
        Assert.True(table5.ContainsKey5("e3"));
        Assert.True(table5.ContainsKey5("e4"));
        Assert.True(table5.ContainsKey5("e5"));
        Assert.True(table5.ContainsKey5("e6"));
        Assert.True(table5.ContainsKey5("e7"));
        Assert.True(table5.ContainsKey5("e8"));
        Assert.True(table5.ContainsKey5("e9"));

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

        Assert.True(table4.Contains("a1", "b1", "c1", "d1"));
        Assert.True(table4.Contains("a1", "b2", "c1", "d2"));
        Assert.True(table4.Contains("a1", "b3", "c2", "d1"));
        Assert.True(table4.Contains("a2", "b1", "c2", "d2"));
        Assert.True(table4.Contains("a2", "b2", "c3", "d1"));
        Assert.True(table4.Contains("a2", "b3", "c3", "d2"));
        Assert.True(table4.Contains("a3", "b1", "c1", "d1"));
        Assert.True(table4.Contains("a3", "b2", "c2", "d2"));
        Assert.True(table4.Contains("a3", "b3", "c3", "d1"));
        Assert.True(table4.ContainsKey1("a1"));
        Assert.True(table4.ContainsKey1("a2"));
        Assert.True(table4.ContainsKey1("a3"));
        Assert.True(table4.ContainsKey2("b1"));
        Assert.True(table4.ContainsKey2("b2"));
        Assert.True(table4.ContainsKey2("b3"));
        Assert.True(table4.ContainsKey3("c1"));
        Assert.True(table4.ContainsKey3("c2"));
        Assert.True(table4.ContainsKey3("c3"));
        Assert.True(table4.ContainsKey4("d1"));
        Assert.True(table4.ContainsKey4("d2"));

        var table3 = new IndexedTable<string, string, string, int>();
        table3["a1", "b1", "c1"] = 1;
        table3["a1", "b2", "c2"] = 2;
        table3["a1", "b3", "c3"] = 3;
        table3["a2", "b1", "c1"] = 4;
        table3["a2", "b2", "c2"] = 5;
        table3["a2", "b3", "c3"] = 6;
        table3["a3", "b1", "c1"] = 7;
        table3["a3", "b2", "c2"] = 8;
        table3["a3", "b3", "c3"] = 9;

        Assert.True(table3.Contains("a1", "b1", "c1"));
        Assert.True(table3.Contains("a1", "b2", "c2"));
        Assert.True(table3.Contains("a1", "b3", "c3"));
        Assert.True(table3.Contains("a2", "b1", "c1"));
        Assert.True(table3.Contains("a2", "b2", "c2"));
        Assert.True(table3.Contains("a2", "b3", "c3"));
        Assert.True(table3.Contains("a3", "b1", "c1"));
        Assert.True(table3.Contains("a3", "b2", "c2"));
        Assert.True(table3.Contains("a3", "b3", "c3"));
        Assert.True(table3.ContainsKey1("a1"));
        Assert.True(table3.ContainsKey1("a2"));
        Assert.True(table3.ContainsKey1("a3"));
        Assert.True(table3.ContainsKey2("b1"));
        Assert.True(table3.ContainsKey2("b2"));
        Assert.True(table3.ContainsKey2("b3"));
        Assert.True(table3.ContainsKey3("c1"));
        Assert.True(table3.ContainsKey3("c2"));
        Assert.True(table3.ContainsKey3("c3"));

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

        Assert.True(table2.Contains("a1", "b1"));
        Assert.True(table2.Contains("a1", "b2"));
        Assert.True(table2.Contains("a1", "b3"));
        Assert.True(table2.Contains("a2", "b1"));
        Assert.True(table2.Contains("a2", "b2"));
        Assert.True(table2.Contains("a2", "b3"));
        Assert.True(table2.Contains("a3", "b1"));
        Assert.True(table2.Contains("a3", "b2"));
        Assert.True(table2.Contains("a3", "b3"));
        Assert.True(table2.ContainsKey1("a1"));
        Assert.True(table2.ContainsKey1("a2"));
        Assert.True(table2.ContainsKey1("a3"));
        Assert.True(table2.ContainsKey2("b1"));
        Assert.True(table2.ContainsKey2("b2"));
        Assert.True(table2.ContainsKey2("b3"));

        var table1 = new IndexedTable<string, int>();
        table1["a1"] = 1;
        table1["a2"] = 2;
        table1["a3"] = 3;

        Assert.True(table1.Contains("a1"));
        Assert.True(table1.Contains("a2"));
        Assert.True(table1.Contains("a3"));
        Assert.True(table1.ContainsKey("a1"));
        Assert.True(table1.ContainsKey("a2"));
        Assert.True(table1.ContainsKey("a3"));
    }

    [Fact]
    public void TestContainsWithSlice() {        
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

        var table4 = table5.Slice1("a1");
        Assert.Equal(3, table4.Count);
        Assert.True(table4.Contains("b1", "c1", "d1", "e1"));
        Assert.True(table4.Contains("b2", "c1", "d2", "e2"));
        Assert.True(table4.Contains("b3", "c2", "d1", "e3"));
        Assert.False(table4.Contains("b1", "c2", "d2", "e4"));
        Assert.False(table4.Contains("b2", "c3", "d1", "e5"));
        Assert.False(table4.Contains("b3", "c3", "d2", "e6"));
        Assert.False(table4.Contains("b1", "c1", "d1", "e7"));
        Assert.False(table4.Contains("b2", "c2", "d2", "e8"));
        Assert.False(table4.Contains("b3", "c3", "d1", "e9"));
        Assert.True(table4.ContainsKey1("b1"));
        Assert.True(table4.ContainsKey1("b2"));
        Assert.True(table4.ContainsKey1("b3"));
        Assert.True(table4.ContainsKey2("c1"));
        Assert.True(table4.ContainsKey2("c2"));
        Assert.False(table4.ContainsKey2("c3"));
        Assert.True(table4.ContainsKey3("d1"));
        Assert.True(table4.ContainsKey3("d2"));
        Assert.True(table4.ContainsKey4("e1"));
        Assert.True(table4.ContainsKey4("e2"));
        Assert.True(table4.ContainsKey4("e3"));
        Assert.False(table4.ContainsKey4("e4"));
        Assert.False(table4.ContainsKey4("e5"));
        Assert.False(table4.ContainsKey4("e6"));
        Assert.False(table4.ContainsKey4("e7"));
        Assert.False(table4.ContainsKey4("e8"));
        Assert.False(table4.ContainsKey4("e9"));

        var table3 = table4.Slice1("b1");
        Assert.Equal(1, table3.Count);
        Assert.True(table3.Contains("c1", "d1", "e1"));
        Assert.False(table3.Contains("c1", "d2", "e2"));
        Assert.False(table3.Contains("c2", "d1", "e3"));
        Assert.False(table3.Contains("c2", "d2", "e4"));
        Assert.False(table3.Contains("c3", "d1", "e5"));
        Assert.False(table3.Contains("c3", "d2", "e6"));
        Assert.False(table3.Contains("c1", "d1", "e7"));
        Assert.False(table3.Contains("c2", "d2", "e8"));
        Assert.False(table3.Contains("c3", "d1", "e9"));
        Assert.True(table3.ContainsKey1("c1"));
        Assert.False(table3.ContainsKey1("c2"));
        Assert.False(table3.ContainsKey1("c3"));
        Assert.True(table3.ContainsKey2("d1"));
        Assert.False(table3.ContainsKey2("d2"));
        Assert.True(table3.ContainsKey3("e1"));
        Assert.False(table3.ContainsKey3("e2"));
        Assert.False(table3.ContainsKey3("e3"));
        Assert.False(table3.ContainsKey3("e4"));
        Assert.False(table3.ContainsKey3("e5"));
        Assert.False(table3.ContainsKey3("e6"));
        Assert.False(table3.ContainsKey3("e7"));
        Assert.False(table3.ContainsKey3("e8"));
        Assert.False(table3.ContainsKey3("e9"));

        var table2 = table3.Slice1("c1");
        Assert.Equal(1, table2.Count);
        Assert.True(table2.Contains("d1", "e1"));
        Assert.False(table2.Contains("d2", "e2"));
        Assert.False(table2.Contains("d1", "e3"));
        Assert.False(table2.Contains("d2", "e4"));
        Assert.False(table2.Contains("d1", "e5"));
        Assert.False(table2.Contains("d2", "e6"));
        Assert.False(table2.Contains("d1", "e7"));
        Assert.False(table2.Contains("d2", "e8"));
        Assert.False(table2.Contains("d1", "e9"));
        Assert.True(table2.ContainsKey1("d1"));
        Assert.False(table2.ContainsKey1("d2"));
        Assert.True(table2.ContainsKey2("e1"));
        Assert.False(table2.ContainsKey2("e2"));
        Assert.False(table2.ContainsKey2("e3"));
        Assert.False(table2.ContainsKey2("e4"));
        Assert.False(table2.ContainsKey2("e5"));
        Assert.False(table2.ContainsKey2("e6"));
        Assert.False(table2.ContainsKey2("e7"));
        Assert.False(table2.ContainsKey2("e8"));
        Assert.False(table2.ContainsKey2("e9"));

        var table1 = table2.Slice1("d1");
        Assert.Equal(1, table1.Count);
        Assert.True(table1.Contains("e1"));
        Assert.False(table1.Contains("e2"));
        Assert.False(table1.Contains("e3"));
        Assert.False(table1.Contains("e4"));
        Assert.False(table1.Contains("e5"));
        Assert.False(table1.Contains("e6"));
        Assert.False(table1.Contains("e7"));
        Assert.False(table1.Contains("e8"));
        Assert.False(table1.Contains("e9"));
        Assert.True(table1.ContainsKey("e1"));
        Assert.False(table1.ContainsKey("e2"));
        Assert.False(table1.ContainsKey("e3"));
        Assert.False(table1.ContainsKey("e4"));
        Assert.False(table1.ContainsKey("e5"));
        Assert.False(table1.ContainsKey("e6"));
        Assert.False(table1.ContainsKey("e7"));
        Assert.False(table1.ContainsKey("e8"));
        Assert.False(table1.ContainsKey("e9"));
    }
    [Fact]
    public void TestContainsWithManipulation()
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
        Assert.Equal(9, table5.Count);

        var table4 = table5.Slice1("a1");
        Assert.Equal(3, table4.Count);
        table4["b4", "c4", "d4", "e10"] = 10;
        Assert.Equal(4, table4.Count);
        Assert.True(table4.Contains("b4", "c4", "d4", "e10"));
        Assert.True(table5.Contains("a1", "b4", "c4", "d4", "e10"));

        var table3 = table4.Slice1("b1");
        Assert.Equal(1, table3.Count);
        table3["c4", "d4", "e11"] = 11;
        Assert.Equal(2, table3.Count);
        Assert.True(table3.Contains("c4", "d4", "e11"));
        Assert.True(table4.Contains("b1", "c4", "d4", "e11"));
        Assert.True(table5.Contains("a1", "b1", "c4", "d4", "e11"));

        var table2 = table3.Slice1("c1");
        Assert.Equal(1, table2.Count);
        table2["d4", "e12"] = 12;
        Assert.Equal(2, table2.Count);
        Assert.True(table2.Contains("d4", "e12"));
        Assert.True(table3.Contains("c1", "d4", "e12"));
        Assert.True(table4.Contains("b1", "c1", "d4", "e12"));
        Assert.True(table5.Contains("a1", "b1", "c1", "d4", "e12"));

        var table1 = table2.Slice1("d1");
        Assert.Equal(1, table1.Count);
        table1["e13"] = 13;
        Assert.Equal(2, table1.Count);
        Assert.True(table1.Contains("e13"));
        Assert.True(table2.Contains("d1", "e13"));
        Assert.True(table3.Contains("c1", "d1", "e13"));
        Assert.True(table4.Contains("b1", "c1", "d1", "e13"));
        Assert.True(table5.Contains("a1", "b1", "c1", "d1", "e13"));
    }
}