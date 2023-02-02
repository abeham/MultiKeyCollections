using System.Collections.Generic;
using Xunit;

namespace MultiKeyCollections.Tests;

public class ContainsTests
{
    [Fact]
    public void TestContains()
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

        Assert.True(dict5.Contains("a1", "b1", "c1", "d1", "e1"));
        Assert.True(dict5.Contains("a1", "b2", "c1", "d2", "e2"));
        Assert.True(dict5.Contains("a1", "b3", "c2", "d1", "e3"));
        Assert.True(dict5.Contains("a2", "b1", "c2", "d2", "e4"));
        Assert.True(dict5.Contains("a2", "b2", "c3", "d1", "e5"));
        Assert.True(dict5.Contains("a2", "b3", "c3", "d2", "e6"));
        Assert.True(dict5.Contains("a3", "b1", "c1", "d1", "e7"));
        Assert.True(dict5.Contains("a3", "b2", "c2", "d2", "e8"));
        Assert.True(dict5.Contains("a3", "b3", "c3", "d1", "e9"));
        Assert.True(dict5.ContainsKey1("a1"));
        Assert.True(dict5.ContainsKey1("a2"));
        Assert.True(dict5.ContainsKey1("a3"));
        Assert.True(dict5.ContainsKey2("b1"));
        Assert.True(dict5.ContainsKey2("b2"));
        Assert.True(dict5.ContainsKey2("b3"));
        Assert.True(dict5.ContainsKey3("c1"));
        Assert.True(dict5.ContainsKey3("c2"));
        Assert.True(dict5.ContainsKey3("c3"));
        Assert.True(dict5.ContainsKey4("d1"));
        Assert.True(dict5.ContainsKey4("d2"));
        Assert.True(dict5.ContainsKey5("e1"));
        Assert.True(dict5.ContainsKey5("e2"));
        Assert.True(dict5.ContainsKey5("e3"));
        Assert.True(dict5.ContainsKey5("e4"));
        Assert.True(dict5.ContainsKey5("e5"));
        Assert.True(dict5.ContainsKey5("e6"));
        Assert.True(dict5.ContainsKey5("e7"));
        Assert.True(dict5.ContainsKey5("e8"));
        Assert.True(dict5.ContainsKey5("e9"));

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

        Assert.True(dict4.Contains("a1", "b1", "c1", "d1"));
        Assert.True(dict4.Contains("a1", "b2", "c1", "d2"));
        Assert.True(dict4.Contains("a1", "b3", "c2", "d1"));
        Assert.True(dict4.Contains("a2", "b1", "c2", "d2"));
        Assert.True(dict4.Contains("a2", "b2", "c3", "d1"));
        Assert.True(dict4.Contains("a2", "b3", "c3", "d2"));
        Assert.True(dict4.Contains("a3", "b1", "c1", "d1"));
        Assert.True(dict4.Contains("a3", "b2", "c2", "d2"));
        Assert.True(dict4.Contains("a3", "b3", "c3", "d1"));
        Assert.True(dict4.ContainsKey1("a1"));
        Assert.True(dict4.ContainsKey1("a2"));
        Assert.True(dict4.ContainsKey1("a3"));
        Assert.True(dict4.ContainsKey2("b1"));
        Assert.True(dict4.ContainsKey2("b2"));
        Assert.True(dict4.ContainsKey2("b3"));
        Assert.True(dict4.ContainsKey3("c1"));
        Assert.True(dict4.ContainsKey3("c2"));
        Assert.True(dict4.ContainsKey3("c3"));
        Assert.True(dict4.ContainsKey4("d1"));
        Assert.True(dict4.ContainsKey4("d2"));

        var dict3 = new MultiKeyDictionary<string, string, string, int>();
        dict3["a1", "b1", "c1"] = 1;
        dict3["a1", "b2", "c2"] = 2;
        dict3["a1", "b3", "c3"] = 3;
        dict3["a2", "b1", "c1"] = 4;
        dict3["a2", "b2", "c2"] = 5;
        dict3["a2", "b3", "c3"] = 6;
        dict3["a3", "b1", "c1"] = 7;
        dict3["a3", "b2", "c2"] = 8;
        dict3["a3", "b3", "c3"] = 9;

        Assert.True(dict3.Contains("a1", "b1", "c1"));
        Assert.True(dict3.Contains("a1", "b2", "c2"));
        Assert.True(dict3.Contains("a1", "b3", "c3"));
        Assert.True(dict3.Contains("a2", "b1", "c1"));
        Assert.True(dict3.Contains("a2", "b2", "c2"));
        Assert.True(dict3.Contains("a2", "b3", "c3"));
        Assert.True(dict3.Contains("a3", "b1", "c1"));
        Assert.True(dict3.Contains("a3", "b2", "c2"));
        Assert.True(dict3.Contains("a3", "b3", "c3"));
        Assert.True(dict3.ContainsKey1("a1"));
        Assert.True(dict3.ContainsKey1("a2"));
        Assert.True(dict3.ContainsKey1("a3"));
        Assert.True(dict3.ContainsKey2("b1"));
        Assert.True(dict3.ContainsKey2("b2"));
        Assert.True(dict3.ContainsKey2("b3"));
        Assert.True(dict3.ContainsKey3("c1"));
        Assert.True(dict3.ContainsKey3("c2"));
        Assert.True(dict3.ContainsKey3("c3"));

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

        Assert.True(dict2.Contains("a1", "b1"));
        Assert.True(dict2.Contains("a1", "b2"));
        Assert.True(dict2.Contains("a1", "b3"));
        Assert.True(dict2.Contains("a2", "b1"));
        Assert.True(dict2.Contains("a2", "b2"));
        Assert.True(dict2.Contains("a2", "b3"));
        Assert.True(dict2.Contains("a3", "b1"));
        Assert.True(dict2.Contains("a3", "b2"));
        Assert.True(dict2.Contains("a3", "b3"));
        Assert.True(dict2.ContainsKey1("a1"));
        Assert.True(dict2.ContainsKey1("a2"));
        Assert.True(dict2.ContainsKey1("a3"));
        Assert.True(dict2.ContainsKey2("b1"));
        Assert.True(dict2.ContainsKey2("b2"));
        Assert.True(dict2.ContainsKey2("b3"));

        var dict1 = new MultiKeyDictionary<string, int>();
        dict1["a1"] = 1;
        dict1["a2"] = 2;
        dict1["a3"] = 3;

        Assert.True(dict1.Contains("a1"));
        Assert.True(dict1.Contains("a2"));
        Assert.True(dict1.Contains("a3"));
        Assert.True(dict1.ContainsKey("a1"));
        Assert.True(dict1.ContainsKey("a2"));
        Assert.True(dict1.ContainsKey("a3"));
    }

    [Fact]
    public void TestContainsWithSlice() {        
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

        var dict4 = dict5.Slice1("a1");
        Assert.Equal(3, dict4.Count);
        Assert.True(dict4.Contains("b1", "c1", "d1", "e1"));
        Assert.True(dict4.Contains("b2", "c1", "d2", "e2"));
        Assert.True(dict4.Contains("b3", "c2", "d1", "e3"));
        Assert.False(dict4.Contains("b1", "c2", "d2", "e4"));
        Assert.False(dict4.Contains("b2", "c3", "d1", "e5"));
        Assert.False(dict4.Contains("b3", "c3", "d2", "e6"));
        Assert.False(dict4.Contains("b1", "c1", "d1", "e7"));
        Assert.False(dict4.Contains("b2", "c2", "d2", "e8"));
        Assert.False(dict4.Contains("b3", "c3", "d1", "e9"));
        Assert.True(dict4.ContainsKey1("b1"));
        Assert.True(dict4.ContainsKey1("b2"));
        Assert.True(dict4.ContainsKey1("b3"));
        Assert.True(dict4.ContainsKey2("c1"));
        Assert.True(dict4.ContainsKey2("c2"));
        Assert.False(dict4.ContainsKey2("c3"));
        Assert.True(dict4.ContainsKey3("d1"));
        Assert.True(dict4.ContainsKey3("d2"));
        Assert.True(dict4.ContainsKey4("e1"));
        Assert.True(dict4.ContainsKey4("e2"));
        Assert.True(dict4.ContainsKey4("e3"));
        Assert.False(dict4.ContainsKey4("e4"));
        Assert.False(dict4.ContainsKey4("e5"));
        Assert.False(dict4.ContainsKey4("e6"));
        Assert.False(dict4.ContainsKey4("e7"));
        Assert.False(dict4.ContainsKey4("e8"));
        Assert.False(dict4.ContainsKey4("e9"));

        var dict3 = dict4.Slice1("b1");
        Assert.Equal(1, dict3.Count);
        Assert.True(dict3.Contains("c1", "d1", "e1"));
        Assert.False(dict3.Contains("c1", "d2", "e2"));
        Assert.False(dict3.Contains("c2", "d1", "e3"));
        Assert.False(dict3.Contains("c2", "d2", "e4"));
        Assert.False(dict3.Contains("c3", "d1", "e5"));
        Assert.False(dict3.Contains("c3", "d2", "e6"));
        Assert.False(dict3.Contains("c1", "d1", "e7"));
        Assert.False(dict3.Contains("c2", "d2", "e8"));
        Assert.False(dict3.Contains("c3", "d1", "e9"));
        Assert.True(dict3.ContainsKey1("c1"));
        Assert.False(dict3.ContainsKey1("c2"));
        Assert.False(dict3.ContainsKey1("c3"));
        Assert.True(dict3.ContainsKey2("d1"));
        Assert.False(dict3.ContainsKey2("d2"));
        Assert.True(dict3.ContainsKey3("e1"));
        Assert.False(dict3.ContainsKey3("e2"));
        Assert.False(dict3.ContainsKey3("e3"));
        Assert.False(dict3.ContainsKey3("e4"));
        Assert.False(dict3.ContainsKey3("e5"));
        Assert.False(dict3.ContainsKey3("e6"));
        Assert.False(dict3.ContainsKey3("e7"));
        Assert.False(dict3.ContainsKey3("e8"));
        Assert.False(dict3.ContainsKey3("e9"));

        var dict2 = dict3.Slice1("c1");
        Assert.Equal(1, dict2.Count);
        Assert.True(dict2.Contains("d1", "e1"));
        Assert.False(dict2.Contains("d2", "e2"));
        Assert.False(dict2.Contains("d1", "e3"));
        Assert.False(dict2.Contains("d2", "e4"));
        Assert.False(dict2.Contains("d1", "e5"));
        Assert.False(dict2.Contains("d2", "e6"));
        Assert.False(dict2.Contains("d1", "e7"));
        Assert.False(dict2.Contains("d2", "e8"));
        Assert.False(dict2.Contains("d1", "e9"));
        Assert.True(dict2.ContainsKey1("d1"));
        Assert.False(dict2.ContainsKey1("d2"));
        Assert.True(dict2.ContainsKey2("e1"));
        Assert.False(dict2.ContainsKey2("e2"));
        Assert.False(dict2.ContainsKey2("e3"));
        Assert.False(dict2.ContainsKey2("e4"));
        Assert.False(dict2.ContainsKey2("e5"));
        Assert.False(dict2.ContainsKey2("e6"));
        Assert.False(dict2.ContainsKey2("e7"));
        Assert.False(dict2.ContainsKey2("e8"));
        Assert.False(dict2.ContainsKey2("e9"));

        var dict1 = dict2.Slice1("d1");
        Assert.Equal(1, dict1.Count);
        Assert.True(dict1.Contains("e1"));
        Assert.False(dict1.Contains("e2"));
        Assert.False(dict1.Contains("e3"));
        Assert.False(dict1.Contains("e4"));
        Assert.False(dict1.Contains("e5"));
        Assert.False(dict1.Contains("e6"));
        Assert.False(dict1.Contains("e7"));
        Assert.False(dict1.Contains("e8"));
        Assert.False(dict1.Contains("e9"));
        Assert.True(dict1.ContainsKey("e1"));
        Assert.False(dict1.ContainsKey("e2"));
        Assert.False(dict1.ContainsKey("e3"));
        Assert.False(dict1.ContainsKey("e4"));
        Assert.False(dict1.ContainsKey("e5"));
        Assert.False(dict1.ContainsKey("e6"));
        Assert.False(dict1.ContainsKey("e7"));
        Assert.False(dict1.ContainsKey("e8"));
        Assert.False(dict1.ContainsKey("e9"));
    }
    [Fact]
    public void TestContainsWithManipulation()
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

        var dict4 = dict5.Slice1("a1");
        Assert.Equal(3, dict4.Count);
        dict4["b4", "c4", "d4", "e10"] = 10;
        Assert.Equal(4, dict4.Count);
        Assert.True(dict4.Contains("b4", "c4", "d4", "e10"));
        Assert.True(dict5.Contains("a1", "b4", "c4", "d4", "e10"));

        var dict3 = dict4.Slice1("b1");
        Assert.Equal(1, dict3.Count);
        dict3["c4", "d4", "e11"] = 11;
        Assert.Equal(2, dict3.Count);
        Assert.True(dict3.Contains("c4", "d4", "e11"));
        Assert.True(dict4.Contains("b1", "c4", "d4", "e11"));
        Assert.True(dict5.Contains("a1", "b1", "c4", "d4", "e11"));

        var dict2 = dict3.Slice1("c1");
        Assert.Equal(1, dict2.Count);
        dict2["d4", "e12"] = 12;
        Assert.Equal(2, dict2.Count);
        Assert.True(dict2.Contains("d4", "e12"));
        Assert.True(dict3.Contains("c1", "d4", "e12"));
        Assert.True(dict4.Contains("b1", "c1", "d4", "e12"));
        Assert.True(dict5.Contains("a1", "b1", "c1", "d4", "e12"));

        var dict1 = dict2.Slice1("d1");
        Assert.Equal(1, dict1.Count);
        dict1["e13"] = 13;
        Assert.Equal(2, dict1.Count);
        Assert.True(dict1.Contains("e13"));
        Assert.True(dict2.Contains("d1", "e13"));
        Assert.True(dict3.Contains("c1", "d1", "e13"));
        Assert.True(dict4.Contains("b1", "c1", "d1", "e13"));
        Assert.True(dict5.Contains("a1", "b1", "c1", "d1", "e13"));
    }
}