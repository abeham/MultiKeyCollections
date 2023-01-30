using Xunit;

namespace TableCollections.Tests;

public class SetTests
{
    [Fact]
    public void TestSet()
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

        table5.Set(1);
        Assert.Equal(1, table5["a1", "b1", "c1", "d1", "e1"]);
        Assert.Equal(1, table5["a1", "b2", "c1", "d2", "e2"]);
        Assert.Equal(1, table5["a1", "b3", "c2", "d1", "e3"]);
        Assert.Equal(1, table5["a2", "b1", "c2", "d2", "e4"]);
        Assert.Equal(1, table5["a2", "b2", "c3", "d1", "e5"]);
        Assert.Equal(1, table5["a2", "b3", "c3", "d2", "e6"]);
        Assert.Equal(1, table5["a3", "b1", "c1", "d1", "e7"]);
        Assert.Equal(1, table5["a3", "b2", "c2", "d2", "e8"]);
        Assert.Equal(1, table5["a3", "b3", "c3", "d1", "e9"]);
        Assert.Equal(9, table5.Count);
        Assert.Equal(9, table5.Sum());

        var slice45 = table5.Slice1("a1");
        slice45.Set(2);
        Assert.Equal(2, table5["a1", "b1", "c1", "d1", "e1"]);
        Assert.Equal(2, table5["a1", "b2", "c1", "d2", "e2"]);
        Assert.Equal(2, table5["a1", "b3", "c2", "d1", "e3"]);
        Assert.Equal(1, table5["a2", "b1", "c2", "d2", "e4"]);
        Assert.Equal(1, table5["a2", "b2", "c3", "d1", "e5"]);
        Assert.Equal(1, table5["a2", "b3", "c3", "d2", "e6"]);
        Assert.Equal(1, table5["a3", "b1", "c1", "d1", "e7"]);
        Assert.Equal(1, table5["a3", "b2", "c2", "d2", "e8"]);
        Assert.Equal(1, table5["a3", "b3", "c3", "d1", "e9"]);
        Assert.Equal(6, slice45.Sum());
        Assert.Equal(3, slice45.Count);
        Assert.Equal(9, table5.Count);
        Assert.Equal(12, table5.Sum());

        var slice345 = slice45.Slice2("c1");
        slice345.Set(3);
        Assert.Equal(3, table5["a1", "b1", "c1", "d1", "e1"]);
        Assert.Equal(3, table5["a1", "b2", "c1", "d2", "e2"]);
        Assert.Equal(2, table5["a1", "b3", "c2", "d1", "e3"]);
        Assert.Equal(1, table5["a2", "b1", "c2", "d2", "e4"]);
        Assert.Equal(1, table5["a2", "b2", "c3", "d1", "e5"]);
        Assert.Equal(1, table5["a2", "b3", "c3", "d2", "e6"]);
        Assert.Equal(1, table5["a3", "b1", "c1", "d1", "e7"]);
        Assert.Equal(1, table5["a3", "b2", "c2", "d2", "e8"]);
        Assert.Equal(1, table5["a3", "b3", "c3", "d1", "e9"]);
        Assert.Equal(6, slice345.Sum());
        Assert.Equal(2, slice345.Count);
        Assert.Equal(8, slice45.Sum());
        Assert.Equal(14, table5.Sum());

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

        table4.Set(1);
        Assert.Equal(1, table4["a1", "b1", "c1", "d1"]);
        Assert.Equal(1, table4["a1", "b2", "c1", "d2"]);
        Assert.Equal(1, table4["a1", "b3", "c2", "d1"]);
        Assert.Equal(1, table4["a2", "b1", "c2", "d2"]);
        Assert.Equal(1, table4["a2", "b2", "c3", "d1"]);
        Assert.Equal(1, table4["a2", "b3", "c3", "d2"]);
        Assert.Equal(1, table4["a3", "b1", "c1", "d1"]);
        Assert.Equal(1, table4["a3", "b2", "c2", "d2"]);
        Assert.Equal(1, table4["a3", "b3", "c3", "d1"]);
        Assert.Equal(9, table4.Count);
        Assert.Equal(9, table4.Sum());

        var slice34 = table4.Slice1("a1");
        slice34.Set(2);
        Assert.Equal(2, table4["a1", "b1", "c1", "d1"]);
        Assert.Equal(2, table4["a1", "b2", "c1", "d2"]);
        Assert.Equal(2, table4["a1", "b3", "c2", "d1"]);
        Assert.Equal(1, table4["a2", "b1", "c2", "d2"]);
        Assert.Equal(1, table4["a2", "b2", "c3", "d1"]);
        Assert.Equal(1, table4["a2", "b3", "c3", "d2"]);
        Assert.Equal(1, table4["a3", "b1", "c1", "d1"]);
        Assert.Equal(1, table4["a3", "b2", "c2", "d2"]);
        Assert.Equal(1, table4["a3", "b3", "c3", "d1"]);
        Assert.Equal(6, slice34.Sum());
        Assert.Equal(3, slice34.Count);
        Assert.Equal(9, table4.Count);
        Assert.Equal(12, table4.Sum());

        var slice234 = slice34.Slice2("c1");
        slice234.Set(3);
        Assert.Equal(3, table4["a1", "b1", "c1", "d1"]);
        Assert.Equal(3, table4["a1", "b2", "c1", "d2"]);
        Assert.Equal(2, table4["a1", "b3", "c2", "d1"]);
        Assert.Equal(1, table4["a2", "b1", "c2", "d2"]);
        Assert.Equal(1, table4["a2", "b2", "c3", "d1"]);
        Assert.Equal(1, table4["a2", "b3", "c3", "d2"]);
        Assert.Equal(1, table4["a3", "b1", "c1", "d1"]);
        Assert.Equal(1, table4["a3", "b2", "c2", "d2"]);
        Assert.Equal(1, table4["a3", "b3", "c3", "d1"]);
        Assert.Equal(6, slice234.Sum());
        Assert.Equal(2, slice234.Count);
        Assert.Equal(8, slice34.Sum());
        Assert.Equal(3, slice34.Count);
        Assert.Equal(9, table4.Count);
        Assert.Equal(14, table4.Sum());

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

        table3.Set(1);
        Assert.Equal(1, table3["a1", "b1", "c1"]);
        Assert.Equal(1, table3["a1", "b2", "c1"]);
        Assert.Equal(1, table3["a1", "b3", "c2"]);
        Assert.Equal(1, table3["a2", "b1", "c2"]);
        Assert.Equal(1, table3["a2", "b2", "c3"]);
        Assert.Equal(1, table3["a2", "b3", "c3"]);
        Assert.Equal(1, table3["a3", "b1", "c1"]);
        Assert.Equal(1, table3["a3", "b2", "c2"]);
        Assert.Equal(1, table3["a3", "b3", "c3"]);
        Assert.Equal(9, table3.Count);
        Assert.Equal(9, table3.Sum());
        
        var slice23 = table3.Slice1("a1");
        slice23.Set(2);
        Assert.Equal(2, table3["a1", "b1", "c1"]);
        Assert.Equal(2, table3["a1", "b2", "c1"]);
        Assert.Equal(2, table3["a1", "b3", "c2"]);
        Assert.Equal(1, table3["a2", "b1", "c2"]);
        Assert.Equal(1, table3["a2", "b2", "c3"]);
        Assert.Equal(1, table3["a2", "b3", "c3"]);
        Assert.Equal(1, table3["a3", "b1", "c1"]);
        Assert.Equal(1, table3["a3", "b2", "c2"]);
        Assert.Equal(1, table3["a3", "b3", "c3"]);
        Assert.Equal(6, slice23.Sum());
        Assert.Equal(3, slice23.Count);
        Assert.Equal(9, table3.Count);
        Assert.Equal(12, table3.Sum());

        var slice123 = slice23.Slice2("c1");
        slice123.Set(3);
        Assert.Equal(3, table3["a1", "b1", "c1"]);
        Assert.Equal(3, table3["a1", "b2", "c1"]);
        Assert.Equal(2, table3["a1", "b3", "c2"]);
        Assert.Equal(1, table3["a2", "b1", "c2"]);
        Assert.Equal(1, table3["a2", "b2", "c3"]);
        Assert.Equal(1, table3["a2", "b3", "c3"]);
        Assert.Equal(1, table3["a3", "b1", "c1"]);
        Assert.Equal(1, table3["a3", "b2", "c2"]);
        Assert.Equal(1, table3["a3", "b3", "c3"]);
        Assert.Equal(6, slice123.Sum());
        Assert.Equal(2, slice123.Count);
        Assert.Equal(8, slice23.Sum());
        Assert.Equal(3, slice23.Count);
        Assert.Equal(9, table3.Count);
        Assert.Equal(14, table3.Sum());

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

        table2.Set(1);
        Assert.Equal(1, table2["a1", "b1"]);
        Assert.Equal(1, table2["a1", "b2"]);
        Assert.Equal(1, table2["a1", "b3"]);
        Assert.Equal(1, table2["a2", "b1"]);
        Assert.Equal(1, table2["a2", "b2"]);
        Assert.Equal(1, table2["a2", "b3"]);
        Assert.Equal(1, table2["a3", "b1"]);
        Assert.Equal(1, table2["a3", "b2"]);
        Assert.Equal(1, table2["a3", "b3"]);
        Assert.Equal(9, table2.Count);
        Assert.Equal(9, table2.Sum());
        
        var slice12 = table2.Slice1("a1");
        slice12.Set(2);
        Assert.Equal(2, table2["a1", "b1"]);
        Assert.Equal(2, table2["a1", "b2"]);
        Assert.Equal(2, table2["a1", "b3"]);
        Assert.Equal(1, table2["a2", "b1"]);
        Assert.Equal(1, table2["a2", "b2"]);
        Assert.Equal(1, table2["a2", "b3"]);
        Assert.Equal(1, table2["a3", "b1"]);
        Assert.Equal(1, table2["a3", "b2"]);
        Assert.Equal(1, table2["a3", "b3"]);
        Assert.Equal(6, slice12.Sum());
        Assert.Equal(3, slice12.Count);
        Assert.Equal(9, table2.Count);
        Assert.Equal(12, table2.Sum());

        var table1 = new IndexedTable<string, int>();
        table1["a1"] = 1;
        table1["a2"] = 2;
        table1["a3"] = 3;

        table1.Set(1);
        Assert.Equal(1, table1["a1"]);
        Assert.Equal(1, table1["a2"]);
        Assert.Equal(1, table1["a3"]);
        Assert.Equal(3, table1.Count);
        Assert.Equal(3, table1.Sum());
        
    }
}