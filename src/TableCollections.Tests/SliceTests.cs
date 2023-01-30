using Xunit;

namespace TableCollections.Tests;

public class SliceTests
{
    [Fact]
    public void TestSlicing()
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
        Assert.Equal(3, table5.Slice1("a1").Count);
        Assert.Equal(3, table5.Slice1("a2").Count);
        Assert.Equal(3, table5.Slice1("a3").Count);
        Assert.Equal(3, table5.Slice2("b1").Count);
        Assert.Equal(3, table5.Slice2("b2").Count);
        Assert.Equal(3, table5.Slice2("b3").Count);
        Assert.Equal(3, table5.Slice3("c1").Count);
        Assert.Equal(3, table5.Slice3("c2").Count);
        Assert.Equal(3, table5.Slice3("c3").Count);
        Assert.Equal(5, table5.Slice4("d1").Count);
        Assert.Equal(4, table5.Slice4("d2").Count);
        Assert.Equal(1, table5.Slice5("e1").Count);
        Assert.Equal(1, table5.Slice5("e2").Count);
        Assert.Equal(1, table5.Slice5("e3").Count);
        Assert.Equal(1, table5.Slice5("e4").Count);
        Assert.Equal(1, table5.Slice5("e5").Count);
        Assert.Equal(1, table5.Slice5("e6").Count);
        Assert.Equal(1, table5.Slice5("e7").Count);
        Assert.Equal(1, table5.Slice5("e8").Count);
        Assert.Equal(1, table5.Slice5("e9").Count);        
        Assert.Equal(6, table5.Slice1("a1").Sum());
        Assert.Equal(24, table5.Slice1("a3").Sum());
        Assert.Equal(12, table5.Slice2("b1").Sum());
        Assert.Equal(15, table5.Slice3("c2").Sum());
        Assert.Equal(20, table5.Slice4("d2").Sum());
        Assert.Equal(6, table5.Slice5("e6").Sum());

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

        Assert.Equal(9, table4.Count);
        Assert.Equal(3, table4.Slice1("a1").Count);
        Assert.Equal(3, table4.Slice1("a2").Count);
        Assert.Equal(3, table4.Slice1("a3").Count);
        Assert.Equal(3, table4.Slice2("b1").Count);
        Assert.Equal(3, table4.Slice2("b2").Count);
        Assert.Equal(3, table4.Slice2("b3").Count);
        Assert.Equal(3, table4.Slice3("c1").Count);
        Assert.Equal(3, table4.Slice3("c2").Count);
        Assert.Equal(3, table4.Slice3("c3").Count);
        Assert.Equal(5, table4.Slice4("d1").Count);
        Assert.Equal(4, table4.Slice4("d2").Count);
        Assert.Equal(6, table4.Slice1("a1").Sum());
        Assert.Equal(24, table4.Slice1("a3").Sum());
        Assert.Equal(12, table4.Slice2("b1").Sum());
        Assert.Equal(15, table4.Slice3("c2").Sum());
        Assert.Equal(20, table4.Slice4("d2").Sum());

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

        Assert.Equal(9, table3.Count);
        Assert.Equal(3, table3.Slice1("a1").Count);
        Assert.Equal(3, table3.Slice1("a2").Count);
        Assert.Equal(3, table3.Slice1("a3").Count);
        Assert.Equal(3, table3.Slice2("b1").Count);
        Assert.Equal(3, table3.Slice2("b2").Count);
        Assert.Equal(3, table3.Slice2("b3").Count);
        Assert.Equal(3, table3.Slice3("c1").Count);
        Assert.Equal(3, table3.Slice3("c2").Count);
        Assert.Equal(3, table3.Slice3("c3").Count);
        Assert.Equal(6, table3.Slice1("a1").Sum());
        Assert.Equal(24, table3.Slice1("a3").Sum());
        Assert.Equal(12, table3.Slice2("b1").Sum());
        Assert.Equal(15, table3.Slice3("c2").Sum());

        var table2 = new IndexedTable<string, string, int>();
        table2["a", "b"] = 1;
        table2["a", "c"] = 2;
        table2["a", "d"] = 3;
        table2["b", "b"] = 4;
        table2["b", "c"] = 5;
        table2["b", "d"] = 6;        

        Assert.Equal(6, table2.Count);
        Assert.Equal(3, table2.Slice1("a").Count);
        Assert.Equal(3, table2.Slice1("b").Count);
        Assert.Equal(2, table2.Slice2("b").Count);
        Assert.Equal(2, table2.Slice2("c").Count);
        Assert.Equal(2, table2.Slice2("d").Count);
        Assert.Equal(6, table2.Slice1("a").Sum());
        Assert.Equal(15, table2.Slice1("b").Sum());
        Assert.Equal(5, table2.Slice2("b").Sum());
        Assert.Equal(7, table2.Slice2("c").Sum());
        Assert.Equal(9, table2.Slice2("d").Sum());

        var table = new IndexedTable<string, int>();
        table["a"] = 1;
        table["b"] = 2;
        table["c"] = 3;
        table["d"] = 4;
        table["e"] = 5;
        table["f"] = 6;

        Assert.Equal(6, table.Count);
        Assert.Equal(1, table["a"]);
    }
    
    [Fact]
    public void TestSlicingWithManipulation()
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

        table5.Slice1("a1").Add("b4", "c4", "d4", "e4", 4);
        table5.Slice2("b1").Add("a4", "c4", "d4", "e4", 4);
        table5.Slice3("c1").Add("a4", "b4", "d4", "e4", 4);
        table5.Slice4("d1").Add("a4", "b4", "c4", "e4", 4);
        table5.Slice5("e1").Add("a4", "b4", "c4", "d4", 4);

        Assert.Equal(14, table5.Count);
        Assert.Equal(65, table5.Sum());
        Assert.Equal(10, table5.Slice1("a1").Sum());
        Assert.Equal(16, table5.Slice2("b1").Sum());
        Assert.Equal(14, table5.Slice3("c1").Sum());
        Assert.Equal(29, table5.Slice4("d1").Sum());
        Assert.Equal( 5, table5.Slice5("e1").Sum());
        Assert.Equal(16, table5.Slice1("a4").Sum());
        Assert.Equal(16, table5.Slice2("b4").Sum());
        Assert.Equal(16, table5.Slice3("c4").Sum());
        Assert.Equal(16, table5.Slice4("d4").Sum());
        Assert.Equal(20, table5.Slice5("e4").Sum());

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

        table4.Slice1("a1").Add("b4", "c4", "d4", 4);
        table4.Slice2("b1").Add("a4", "c4", "d4", 4);
        table4.Slice3("c1").Add("a4", "b4", "d4", 4);
        table4.Slice4("d1").Add("a4", "b4", "c4", 4);

        Assert.Equal(13, table4.Count);
        Assert.Equal(61, table4.Sum());
        Assert.Equal(10, table4.Slice1("a1").Sum());
        Assert.Equal(16, table4.Slice2("b1").Sum());
        Assert.Equal(14, table4.Slice3("c1").Sum());
        Assert.Equal(29, table4.Slice4("d1").Sum());
        Assert.Equal(12, table4.Slice1("a4").Sum());
        Assert.Equal(12, table4.Slice2("b4").Sum());
        Assert.Equal(12, table4.Slice3("c4").Sum());
        Assert.Equal(12, table4.Slice4("d4").Sum());

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

        table3.Slice1("a1").Add("b4", "c4", 4);
        table3.Slice2("b1").Add("a4", "c4", 4);
        table3.Slice3("c1").Add("a4", "b4", 4);

        Assert.Equal(12, table3.Count);
        Assert.Equal(57, table3.Sum());
        Assert.Equal(10, table3.Slice1("a1").Sum());
        Assert.Equal(16, table3.Slice2("b1").Sum());
        Assert.Equal(14, table3.Slice3("c1").Sum());
        Assert.Equal( 8, table3.Slice1("a4").Sum());
        Assert.Equal( 8, table3.Slice2("b4").Sum());
        Assert.Equal( 8, table3.Slice3("c4").Sum());

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

        table2.Slice1("a1").Add("b4", 4);
        table2.Slice2("b1").Add("a4", 4);

        Assert.Equal(11, table2.Count);
        Assert.Equal(53, table2.Sum());
        Assert.Equal(10, table2.Slice1("a1").Sum());
        Assert.Equal(16, table2.Slice2("b1").Sum());
        Assert.Equal( 4, table2.Slice1("a4").Sum());
        Assert.Equal( 4, table2.Slice2("b4").Sum());
    }

    [Fact]
    public void TestManipulationAfterSlicing()
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

        var slice4 = table5.Slice1("a1");
        table5.Add("a1", "b4", "c4", "d4", "e4", 4);

        Assert.True(slice4.ContainsKey1("b4"));
        Assert.True(slice4.ContainsKey2("c4"));
        Assert.True(slice4.ContainsKey3("d4"));
        Assert.True(slice4.ContainsKey4("e4"));
        Assert.Equal(4, slice4["b4", "c4", "d4", "e4"]);

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

        var slice3 = table4.Slice1("a1");
        table4.Add("a1", "b4", "c4", "d4", 4);

        Assert.True(slice3.ContainsKey1("b4"));
        Assert.True(slice3.ContainsKey2("c4"));
        Assert.True(slice3.ContainsKey3("d4"));
        Assert.Equal(4, slice3["b4", "c4", "d4"]);

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

        var slice2 = table3.Slice1("a1");
        table3.Add("a1", "b4", "c4", 4);

        Assert.True(slice2.ContainsKey1("b4"));
        Assert.True(slice2.ContainsKey2("c4"));
        Assert.Equal(4, slice2["b4", "c4"]);
        
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

        var slice = table2.Slice1("a1");
        table2.Add("a1", "b4", 4);

        Assert.True(slice.ContainsKey("b4"));
        Assert.Equal(4, slice["b4"]);
    }
}