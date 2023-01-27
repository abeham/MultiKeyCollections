using System.Linq;
using System.Collections.Generic;
using Xunit;

namespace TableCollections.Tests;

public class SliceTests
{
    [Fact]
    public void TestSlicing()
    {
        var table = new IndexedTable<string, string, int>();
        table["a", "b"] = 1;
        table["a", "c"] = 2;
        table["a", "d"] = 3;
        table["b", "b"] = 4;
        table["b", "c"] = 5;
        table["b", "d"] = 6;        

        Assert.Equal(6, table.Slice1("a").Sum());
        Assert.Equal(15, table.Slice1("b").Sum());
        Assert.Equal(5, table.Slice2("b").Sum());
        Assert.Equal(7, table.Slice2("c").Sum());
        Assert.Equal(9, table.Slice2("d").Sum());
    }
    
    [Fact]
    public void TestSlicingWithManipulation()
    {
        var table = new IndexedTable<string, string, int>();
        table["a", "b"] = 1;
        table["a", "c"] = 2;
        table["a", "d"] = 3;
        table["b", "b"] = 4;
        table["b", "c"] = 5;
        table["b", "d"] = 6;

        table.Slice1("a")["e"] = 7;
        table.Slice1("b")["e"] = 8;      

        Assert.Equal(13, table.Slice1("a").Sum());
        Assert.Equal(23, table.Slice1("b").Sum());
        Assert.Equal(5, table.Slice2("b").Sum());
        Assert.Equal(7, table.Slice2("c").Sum());
        Assert.Equal(9, table.Slice2("d").Sum());
        Assert.Equal(15, table.Slice2("e").Sum());
    }
}