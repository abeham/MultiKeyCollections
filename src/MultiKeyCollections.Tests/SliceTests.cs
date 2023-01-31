using Xunit;

namespace TableCollections.Tests;

public class SliceTests
{
    [Fact]
    public void TestSlicing()
    {
        var dict5 = new MultiKeyDictionary<string, string, string, string, string, int>();
        dict5["a1", "b1", "c1", "d1", "e1"] = 1;
        dict5["a1", "b2", "c1", "d2", "e2"] = 2;
        dict5["a1", "b3", "c2", "d1", "e3"] = 3;
        dict5["a2", "b1", "c2", "d2", "e4"] = 4;
        dict5["a2", "b2", "c3", "d1", "e5"] = 5;
        dict5["a2", "b3", "c3", "d2", "e6"] = 6;
        dict5["a3", "b1", "c1", "d1", "e7"] = 7;
        dict5["a3", "b2", "c2", "d2", "e8"] = 8;
        dict5["a3", "b3", "c3", "d1", "e9"] = 9;

        Assert.Equal(9, dict5.Count);
        Assert.Equal(3, dict5.Slice1("a1").Count);
        Assert.Equal(3, dict5.Slice1("a2").Count);
        Assert.Equal(3, dict5.Slice1("a3").Count);
        Assert.Equal(3, dict5.Slice2("b1").Count);
        Assert.Equal(3, dict5.Slice2("b2").Count);
        Assert.Equal(3, dict5.Slice2("b3").Count);
        Assert.Equal(3, dict5.Slice3("c1").Count);
        Assert.Equal(3, dict5.Slice3("c2").Count);
        Assert.Equal(3, dict5.Slice3("c3").Count);
        Assert.Equal(5, dict5.Slice4("d1").Count);
        Assert.Equal(4, dict5.Slice4("d2").Count);
        Assert.Equal(1, dict5.Slice5("e1").Count);
        Assert.Equal(1, dict5.Slice5("e2").Count);
        Assert.Equal(1, dict5.Slice5("e3").Count);
        Assert.Equal(1, dict5.Slice5("e4").Count);
        Assert.Equal(1, dict5.Slice5("e5").Count);
        Assert.Equal(1, dict5.Slice5("e6").Count);
        Assert.Equal(1, dict5.Slice5("e7").Count);
        Assert.Equal(1, dict5.Slice5("e8").Count);
        Assert.Equal(1, dict5.Slice5("e9").Count);        
        Assert.Equal(6, dict5.Slice1("a1").Sum());
        Assert.Equal(24, dict5.Slice1("a3").Sum());
        Assert.Equal(12, dict5.Slice2("b1").Sum());
        Assert.Equal(15, dict5.Slice3("c2").Sum());
        Assert.Equal(20, dict5.Slice4("d2").Sum());
        Assert.Equal(6, dict5.Slice5("e6").Sum());

        var dict4 = new MultiKeyDictionary<string, string, string, string, int>();
        dict4["a1", "b1", "c1", "d1"] = 1;
        dict4["a1", "b2", "c1", "d2"] = 2;
        dict4["a1", "b3", "c2", "d1"] = 3;
        dict4["a2", "b1", "c2", "d2"] = 4;
        dict4["a2", "b2", "c3", "d1"] = 5;
        dict4["a2", "b3", "c3", "d2"] = 6;
        dict4["a3", "b1", "c1", "d1"] = 7;
        dict4["a3", "b2", "c2", "d2"] = 8;
        dict4["a3", "b3", "c3", "d1"] = 9;

        Assert.Equal(9, dict4.Count);
        Assert.Equal(3, dict4.Slice1("a1").Count);
        Assert.Equal(3, dict4.Slice1("a2").Count);
        Assert.Equal(3, dict4.Slice1("a3").Count);
        Assert.Equal(3, dict4.Slice2("b1").Count);
        Assert.Equal(3, dict4.Slice2("b2").Count);
        Assert.Equal(3, dict4.Slice2("b3").Count);
        Assert.Equal(3, dict4.Slice3("c1").Count);
        Assert.Equal(3, dict4.Slice3("c2").Count);
        Assert.Equal(3, dict4.Slice3("c3").Count);
        Assert.Equal(5, dict4.Slice4("d1").Count);
        Assert.Equal(4, dict4.Slice4("d2").Count);
        Assert.Equal(6, dict4.Slice1("a1").Sum());
        Assert.Equal(24, dict4.Slice1("a3").Sum());
        Assert.Equal(12, dict4.Slice2("b1").Sum());
        Assert.Equal(15, dict4.Slice3("c2").Sum());
        Assert.Equal(20, dict4.Slice4("d2").Sum());

        var dict3 = new MultiKeyDictionary<string, string, string, int>();
        dict3["a1", "b1", "c1"] = 1;
        dict3["a1", "b2", "c1"] = 2;
        dict3["a1", "b3", "c2"] = 3;
        dict3["a2", "b1", "c2"] = 4;
        dict3["a2", "b2", "c3"] = 5;
        dict3["a2", "b3", "c3"] = 6;
        dict3["a3", "b1", "c1"] = 7;
        dict3["a3", "b2", "c2"] = 8;
        dict3["a3", "b3", "c3"] = 9;

        Assert.Equal(9, dict3.Count);
        Assert.Equal(3, dict3.Slice1("a1").Count);
        Assert.Equal(3, dict3.Slice1("a2").Count);
        Assert.Equal(3, dict3.Slice1("a3").Count);
        Assert.Equal(3, dict3.Slice2("b1").Count);
        Assert.Equal(3, dict3.Slice2("b2").Count);
        Assert.Equal(3, dict3.Slice2("b3").Count);
        Assert.Equal(3, dict3.Slice3("c1").Count);
        Assert.Equal(3, dict3.Slice3("c2").Count);
        Assert.Equal(3, dict3.Slice3("c3").Count);
        Assert.Equal(6, dict3.Slice1("a1").Sum());
        Assert.Equal(24, dict3.Slice1("a3").Sum());
        Assert.Equal(12, dict3.Slice2("b1").Sum());
        Assert.Equal(15, dict3.Slice3("c2").Sum());

        var dict2 = new MultiKeyDictionary<string, string, int>();
        dict2["a", "b"] = 1;
        dict2["a", "c"] = 2;
        dict2["a", "d"] = 3;
        dict2["b", "b"] = 4;
        dict2["b", "c"] = 5;
        dict2["b", "d"] = 6;        

        Assert.Equal(6, dict2.Count);
        Assert.Equal(3, dict2.Slice1("a").Count);
        Assert.Equal(3, dict2.Slice1("b").Count);
        Assert.Equal(2, dict2.Slice2("b").Count);
        Assert.Equal(2, dict2.Slice2("c").Count);
        Assert.Equal(2, dict2.Slice2("d").Count);
        Assert.Equal(6, dict2.Slice1("a").Sum());
        Assert.Equal(15, dict2.Slice1("b").Sum());
        Assert.Equal(5, dict2.Slice2("b").Sum());
        Assert.Equal(7, dict2.Slice2("c").Sum());
        Assert.Equal(9, dict2.Slice2("d").Sum());

        var dict = new MultiKeyDictionary<string, int>();
        dict["a"] = 1;
        dict["b"] = 2;
        dict["c"] = 3;
        dict["d"] = 4;
        dict["e"] = 5;
        dict["f"] = 6;

        Assert.Equal(6, dict.Count);
        Assert.Equal(1, dict["a"]);
    }
    
    [Fact]
    public void TestSlicingWithManipulation()
    {
        var dict5 = new MultiKeyDictionary<string, string, string, string, string, int>();
        dict5["a1", "b1", "c1", "d1", "e1"] = 1;
        dict5["a1", "b2", "c1", "d2", "e2"] = 2;
        dict5["a1", "b3", "c2", "d1", "e3"] = 3;
        dict5["a2", "b1", "c2", "d2", "e4"] = 4;
        dict5["a2", "b2", "c3", "d1", "e5"] = 5;
        dict5["a2", "b3", "c3", "d2", "e6"] = 6;
        dict5["a3", "b1", "c1", "d1", "e7"] = 7;
        dict5["a3", "b2", "c2", "d2", "e8"] = 8;
        dict5["a3", "b3", "c3", "d1", "e9"] = 9;

        dict5.Slice1("a1").Add("b4", "c4", "d4", "e4", 4);
        dict5.Slice2("b1").Add("a4", "c4", "d4", "e4", 4);
        dict5.Slice3("c1").Add("a4", "b4", "d4", "e4", 4);
        dict5.Slice4("d1").Add("a4", "b4", "c4", "e4", 4);
        dict5.Slice5("e1").Add("a4", "b4", "c4", "d4", 4);

        Assert.Equal(14, dict5.Count);
        Assert.Equal(65, dict5.Sum());
        Assert.Equal(10, dict5.Slice1("a1").Sum());
        Assert.Equal(16, dict5.Slice2("b1").Sum());
        Assert.Equal(14, dict5.Slice3("c1").Sum());
        Assert.Equal(29, dict5.Slice4("d1").Sum());
        Assert.Equal( 5, dict5.Slice5("e1").Sum());
        Assert.Equal(16, dict5.Slice1("a4").Sum());
        Assert.Equal(16, dict5.Slice2("b4").Sum());
        Assert.Equal(16, dict5.Slice3("c4").Sum());
        Assert.Equal(16, dict5.Slice4("d4").Sum());
        Assert.Equal(20, dict5.Slice5("e4").Sum());

        var dict4 = new MultiKeyDictionary<string, string, string, string, int>();
        dict4["a1", "b1", "c1", "d1"] = 1;
        dict4["a1", "b2", "c1", "d2"] = 2;
        dict4["a1", "b3", "c2", "d1"] = 3;
        dict4["a2", "b1", "c2", "d2"] = 4;
        dict4["a2", "b2", "c3", "d1"] = 5;
        dict4["a2", "b3", "c3", "d2"] = 6;
        dict4["a3", "b1", "c1", "d1"] = 7;
        dict4["a3", "b2", "c2", "d2"] = 8;
        dict4["a3", "b3", "c3", "d1"] = 9;

        dict4.Slice1("a1").Add("b4", "c4", "d4", 4);
        dict4.Slice2("b1").Add("a4", "c4", "d4", 4);
        dict4.Slice3("c1").Add("a4", "b4", "d4", 4);
        dict4.Slice4("d1").Add("a4", "b4", "c4", 4);

        Assert.Equal(13, dict4.Count);
        Assert.Equal(61, dict4.Sum());
        Assert.Equal(10, dict4.Slice1("a1").Sum());
        Assert.Equal(16, dict4.Slice2("b1").Sum());
        Assert.Equal(14, dict4.Slice3("c1").Sum());
        Assert.Equal(29, dict4.Slice4("d1").Sum());
        Assert.Equal(12, dict4.Slice1("a4").Sum());
        Assert.Equal(12, dict4.Slice2("b4").Sum());
        Assert.Equal(12, dict4.Slice3("c4").Sum());
        Assert.Equal(12, dict4.Slice4("d4").Sum());

        var dict3 = new MultiKeyDictionary<string, string, string, int>();
        dict3["a1", "b1", "c1"] = 1;
        dict3["a1", "b2", "c1"] = 2;
        dict3["a1", "b3", "c2"] = 3;
        dict3["a2", "b1", "c2"] = 4;
        dict3["a2", "b2", "c3"] = 5;
        dict3["a2", "b3", "c3"] = 6;
        dict3["a3", "b1", "c1"] = 7;
        dict3["a3", "b2", "c2"] = 8;
        dict3["a3", "b3", "c3"] = 9;

        dict3.Slice1("a1").Add("b4", "c4", 4);
        dict3.Slice2("b1").Add("a4", "c4", 4);
        dict3.Slice3("c1").Add("a4", "b4", 4);

        Assert.Equal(12, dict3.Count);
        Assert.Equal(57, dict3.Sum());
        Assert.Equal(10, dict3.Slice1("a1").Sum());
        Assert.Equal(16, dict3.Slice2("b1").Sum());
        Assert.Equal(14, dict3.Slice3("c1").Sum());
        Assert.Equal( 8, dict3.Slice1("a4").Sum());
        Assert.Equal( 8, dict3.Slice2("b4").Sum());
        Assert.Equal( 8, dict3.Slice3("c4").Sum());

        var dict2 = new MultiKeyDictionary<string, string, int>();
        dict2["a1", "b1"] = 1;
        dict2["a1", "b2"] = 2;
        dict2["a1", "b3"] = 3;
        dict2["a2", "b1"] = 4;
        dict2["a2", "b2"] = 5;
        dict2["a2", "b3"] = 6;
        dict2["a3", "b1"] = 7;
        dict2["a3", "b2"] = 8;
        dict2["a3", "b3"] = 9;

        dict2.Slice1("a1").Add("b4", 4);
        dict2.Slice2("b1").Add("a4", 4);

        Assert.Equal(11, dict2.Count);
        Assert.Equal(53, dict2.Sum());
        Assert.Equal(10, dict2.Slice1("a1").Sum());
        Assert.Equal(16, dict2.Slice2("b1").Sum());
        Assert.Equal( 4, dict2.Slice1("a4").Sum());
        Assert.Equal( 4, dict2.Slice2("b4").Sum());
    }

    [Fact]
    public void TestManipulationAfterSlicing()
    {
        var dict5 = new MultiKeyDictionary<string, string, string, string, string, int>();
        dict5["a1", "b1", "c1", "d1", "e1"] = 1;
        dict5["a1", "b2", "c1", "d2", "e2"] = 2;
        dict5["a1", "b3", "c2", "d1", "e3"] = 3;
        dict5["a2", "b1", "c2", "d2", "e4"] = 4;
        dict5["a2", "b2", "c3", "d1", "e5"] = 5;
        dict5["a2", "b3", "c3", "d2", "e6"] = 6;
        dict5["a3", "b1", "c1", "d1", "e7"] = 7;
        dict5["a3", "b2", "c2", "d2", "e8"] = 8;
        dict5["a3", "b3", "c3", "d1", "e9"] = 9;

        var slice4 = dict5.Slice1("a1");
        dict5.Add("a1", "b4", "c4", "d4", "e4", 4);

        Assert.True(slice4.ContainsKey1("b4"));
        Assert.True(slice4.ContainsKey2("c4"));
        Assert.True(slice4.ContainsKey3("d4"));
        Assert.True(slice4.ContainsKey4("e4"));
        Assert.Equal(4, slice4["b4", "c4", "d4", "e4"]);

        var dict4 = new MultiKeyDictionary<string, string, string, string, int>();
        dict4["a1", "b1", "c1", "d1"] = 1;
        dict4["a1", "b2", "c1", "d2"] = 2;
        dict4["a1", "b3", "c2", "d1"] = 3;
        dict4["a2", "b1", "c2", "d2"] = 4;
        dict4["a2", "b2", "c3", "d1"] = 5;
        dict4["a2", "b3", "c3", "d2"] = 6;
        dict4["a3", "b1", "c1", "d1"] = 7;
        dict4["a3", "b2", "c2", "d2"] = 8;
        dict4["a3", "b3", "c3", "d1"] = 9;

        var slice3 = dict4.Slice1("a1");
        dict4.Add("a1", "b4", "c4", "d4", 4);

        Assert.True(slice3.ContainsKey1("b4"));
        Assert.True(slice3.ContainsKey2("c4"));
        Assert.True(slice3.ContainsKey3("d4"));
        Assert.Equal(4, slice3["b4", "c4", "d4"]);

        var dict3 = new MultiKeyDictionary<string, string, string, int>();
        dict3["a1", "b1", "c1"] = 1;
        dict3["a1", "b2", "c1"] = 2;
        dict3["a1", "b3", "c2"] = 3;
        dict3["a2", "b1", "c2"] = 4;
        dict3["a2", "b2", "c3"] = 5;
        dict3["a2", "b3", "c3"] = 6;
        dict3["a3", "b1", "c1"] = 7;
        dict3["a3", "b2", "c2"] = 8;
        dict3["a3", "b3", "c3"] = 9;

        var slice2 = dict3.Slice1("a1");
        dict3.Add("a1", "b4", "c4", 4);

        Assert.True(slice2.ContainsKey1("b4"));
        Assert.True(slice2.ContainsKey2("c4"));
        Assert.Equal(4, slice2["b4", "c4"]);
        
        var dict2 = new MultiKeyDictionary<string, string, int>();
        dict2["a1", "b1"] = 1;
        dict2["a1", "b2"] = 2;
        dict2["a1", "b3"] = 3;
        dict2["a2", "b1"] = 4;
        dict2["a2", "b2"] = 5;
        dict2["a2", "b3"] = 6;
        dict2["a3", "b1"] = 7;
        dict2["a3", "b2"] = 8;
        dict2["a3", "b3"] = 9;

        var slice = dict2.Slice1("a1");
        dict2.Add("a1", "b4", 4);

        Assert.True(slice.ContainsKey("b4"));
        Assert.Equal(4, slice["b4"]);
    }

    [Fact]
    public void TestManipulationAfterSlicingWithIndexer()
    {
        var dict5 = new MultiKeyDictionary<string, string, string, string, string, int>();
        dict5["a1", "b1", "c1", "d1", "e1"] = 1;
        dict5["a1", "b2", "c1", "d2", "e2"] = 2;
        dict5["a1", "b3", "c2", "d1", "e3"] = 3;
        dict5["a2", "b1", "c2", "d2", "e4"] = 4;
        dict5["a2", "b2", "c3", "d1", "e5"] = 5;
        dict5["a2", "b3", "c3", "d2", "e6"] = 6;
        dict5["a3", "b1", "c1", "d1", "e7"] = 7;
        dict5["a3", "b2", "c2", "d2", "e8"] = 8;
        dict5["a3", "b3", "c3", "d1", "e9"] = 9;

        var slice4 = dict5.Slice1("a1");
        slice4["b4", "c4", "d4", "e4"] = 4;

        Assert.True(slice4.ContainsKey1("b4"));
        Assert.True(slice4.ContainsKey2("c4"));
        Assert.True(slice4.ContainsKey3("d4"));
        Assert.True(slice4.ContainsKey4("e4"));
        Assert.Equal(4, slice4["b4", "c4", "d4", "e4"]);
        Assert.True(dict5.ContainsKey2("b4"));
        Assert.True(dict5.ContainsKey3("c4"));
        Assert.True(dict5.ContainsKey4("d4"));
        Assert.True(dict5.ContainsKey5("e4"));
        Assert.Equal(4, dict5["a1", "b4", "c4", "d4", "e4"]);

        var dict4 = new MultiKeyDictionary<string, string, string, string, int>();
        dict4["a1", "b1", "c1", "d1"] = 1;
        dict4["a1", "b2", "c1", "d2"] = 2;
        dict4["a1", "b3", "c2", "d1"] = 3;
        dict4["a2", "b1", "c2", "d2"] = 4;
        dict4["a2", "b2", "c3", "d1"] = 5;
        dict4["a2", "b3", "c3", "d2"] = 6;
        dict4["a3", "b1", "c1", "d1"] = 7;
        dict4["a3", "b2", "c2", "d2"] = 8;
        dict4["a3", "b3", "c3", "d1"] = 9;

        var slice3 = dict4.Slice1("a1");
        slice3["b4", "c4", "d4"] = 4;

        Assert.True(slice3.ContainsKey1("b4"));
        Assert.True(slice3.ContainsKey2("c4"));
        Assert.True(slice3.ContainsKey3("d4"));
        Assert.Equal(4, slice3["b4", "c4", "d4"]);
        Assert.True(dict4.ContainsKey2("b4"));
        Assert.True(dict4.ContainsKey3("c4"));
        Assert.True(dict4.ContainsKey4("d4"));
        Assert.Equal(4, dict4["a1", "b4", "c4", "d4"]);

        var dict3 = new MultiKeyDictionary<string, string, string, int>();
        dict3["a1", "b1", "c1"] = 1;
        dict3["a1", "b2", "c1"] = 2;
        dict3["a1", "b3", "c2"] = 3;
        dict3["a2", "b1", "c2"] = 4;
        dict3["a2", "b2", "c3"] = 5;
        dict3["a2", "b3", "c3"] = 6;
        dict3["a3", "b1", "c1"] = 7;
        dict3["a3", "b2", "c2"] = 8;
        dict3["a3", "b3", "c3"] = 9;

        var slice2 = dict3.Slice1("a1");
        slice2["b4", "c4"] = 4;

        Assert.True(slice2.ContainsKey1("b4"));
        Assert.True(slice2.ContainsKey2("c4"));
        Assert.Equal(4, slice2["b4", "c4"]);
        Assert.True(dict3.ContainsKey2("b4"));
        Assert.True(dict3.ContainsKey3("c4"));
        Assert.Equal(4, dict3["a1", "b4", "c4"]);
        
        var dict2 = new MultiKeyDictionary<string, string, int>();
        dict2["a1", "b1"] = 1;
        dict2["a1", "b2"] = 2;
        dict2["a1", "b3"] = 3;
        dict2["a2", "b1"] = 4;
        dict2["a2", "b2"] = 5;
        dict2["a2", "b3"] = 6;
        dict2["a3", "b1"] = 7;
        dict2["a3", "b2"] = 8;
        dict2["a3", "b3"] = 9;

        var slice = dict2.Slice1("a1");
        slice["b4"] = 4;

        Assert.True(slice.ContainsKey("b4"));
        Assert.Equal(4, slice["b4"]);
        Assert.True(dict2.ContainsKey2("b4"));
        Assert.Equal(4, dict2["a1", "b4"]);
    }

    [Fact]
    public void TestSlicingWithClear()
    {
        var dict5 = new MultiKeyDictionary<string, string, string, string, string, int>();
        dict5["a1", "b1", "c1", "d1", "e1"] = 1;
        dict5["a1", "b2", "c1", "d2", "e2"] = 2;
        dict5["a1", "b3", "c2", "d1", "e3"] = 3;
        dict5["a2", "b1", "c2", "d2", "e4"] = 4;
        dict5["a2", "b2", "c3", "d1", "e5"] = 5;
        dict5["a2", "b3", "c3", "d2", "e6"] = 6;

        Assert.Equal(6, dict5.Count);
        var slice45 = dict5.Slice1("a1");
        slice45.Clear();
        Assert.Equal(3, dict5.Count);
        Assert.Equal(0, slice45.Count);
        slice45["b1", "c1", "d1", "e1"] = 1;
        Assert.Equal(4, dict5.Count);
        Assert.True(dict5.Contains("a1", "b1", "c1", "d1", "e1"));
        Assert.Equal(1, slice45.Count);
        Assert.True(slice45.Contains("b1", "c1", "d1", "e1"));
        dict5.Clear();
        Assert.Equal(0, dict5.Count);
        Assert.Equal(0, slice45.Count);
        dict5["a1", "b1", "c1", "d1", "e1"] = 1;
        Assert.Equal(1, dict5.Count);
        Assert.Equal(1, slice45.Count);
        Assert.True(dict5.Contains("a1", "b1", "c1", "d1", "e1"));
        Assert.True(slice45.Contains("b1", "c1", "d1", "e1"));

        var dict4 = new MultiKeyDictionary<string, string, string, string, int>();
        dict4["a1", "b1", "c1", "d1"] = 1;
        dict4["a1", "b2", "c1", "d2"] = 2;
        dict4["a1", "b3", "c2", "d1"] = 3;
        dict4["a2", "b1", "c2", "d2"] = 4;
        dict4["a2", "b2", "c3", "d1"] = 5;
        dict4["a2", "b3", "c3", "d2"] = 6;

        Assert.Equal(6, dict4.Count);
        var slice34 = dict4.Slice1("a1");
        slice34.Clear();
        Assert.Equal(3, dict4.Count);
        Assert.Equal(0, slice34.Count);
        slice34["b1", "c1", "d1"] = 1;
        Assert.Equal(4, dict4.Count);
        Assert.True(dict4.Contains("a1", "b1", "c1", "d1"));
        Assert.Equal(1, slice34.Count);
        Assert.True(slice34.Contains("b1", "c1", "d1"));
        dict4.Clear();
        Assert.Equal(0, dict4.Count);
        Assert.Equal(0, slice34.Count);
        dict4["a1", "b1", "c1", "d1"] = 1;
        Assert.Equal(1, dict4.Count);
        Assert.Equal(1, slice34.Count);
        Assert.True(dict4.Contains("a1", "b1", "c1", "d1"));
        Assert.True(slice34.Contains("b1", "c1", "d1"));

        var dict3 = new MultiKeyDictionary<string, string, string, int>();
        dict3["a1", "b1", "c1"] = 1;
        dict3["a1", "b2", "c2"] = 2;
        dict3["a1", "b3", "c3"] = 3;
        dict3["a2", "b1", "c1"] = 4;
        dict3["a2", "b2", "c2"] = 5;
        dict3["a2", "b3", "c3"] = 6;

        Assert.Equal(6, dict3.Count);
        var slice23 = dict3.Slice1("a1");
        slice23.Clear();
        Assert.Equal(3, dict3.Count);
        Assert.Equal(0, slice23.Count);
        slice23["b1", "c1"] = 1;
        Assert.Equal(4, dict3.Count);
        Assert.True(dict3.Contains("a1", "b1", "c1"));
        Assert.Equal(1, slice23.Count);
        Assert.True(slice23.Contains("b1", "c1"));
        dict3.Clear();
        Assert.Equal(0, dict3.Count);
        Assert.Equal(0, slice23.Count);
        dict3["a1", "b1", "c1"] = 1;
        Assert.Equal(1, dict3.Count);
        Assert.Equal(1, slice23.Count);
        Assert.True(dict3.Contains("a1", "b1", "c1"));
        Assert.True(slice23.Contains("b1", "c1"));

        var dict2 = new MultiKeyDictionary<string, string, int>();
        dict2["a1", "b1"] = 1;
        dict2["a1", "b2"] = 2;
        dict2["a1", "b3"] = 3;
        dict2["a2", "b1"] = 4;
        dict2["a2", "b2"] = 5;
        dict2["a2", "b3"] = 6;

        Assert.Equal(6, dict2.Count);
        var slice12 = dict2.Slice1("a1");
        slice12.Clear();
        Assert.Equal(3, dict2.Count);
        Assert.Equal(0, slice12.Count);
        slice12["b1"] = 1;
        Assert.Equal(4, dict2.Count);
        Assert.True(dict2.Contains("a1", "b1"));
        Assert.Equal(1, slice12.Count);
        Assert.True(slice12.Contains("b1"));
        dict2.Clear();
        Assert.Equal(0, dict2.Count);
        Assert.Equal(0, slice12.Count);
        dict2["a1", "b1"] = 1;
        Assert.Equal(1, dict2.Count);
        Assert.Equal(1, slice12.Count);
        Assert.True(dict2.Contains("a1", "b1"));
        Assert.True(slice12.Contains("b1"));

        var dict1 = new MultiKeyDictionary<string, int>();
        dict1["a1"] = 1;
        dict1["a2"] = 2;
        dict1["a3"] = 3;
        dict1["a4"] = 4;
        dict1["a5"] = 5;
        dict1["a6"] = 6;

        Assert.Equal(6, dict1.Count);
        dict1.Clear();
        Assert.Equal(0, dict1.Count);
        dict1["a1"] = 1;
        Assert.Equal(1, dict1.Count);
        Assert.True(dict1.Contains("a1"));
    }
}