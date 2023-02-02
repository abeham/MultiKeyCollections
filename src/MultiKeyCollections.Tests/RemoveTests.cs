using Xunit;

namespace MultiKeyCollections.Tests;

public class RemoveTests
{
    [Fact]
    public void TestRemove()
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

        Assert.True(dict5.Remove("a1", "b1", "c1", "d1", "e1"));
        Assert.Equal(8, dict5.Count);
        Assert.Equal(8, dict5.Enumerate().Count());
        Assert.Equal(8, dict5.EnumerateValues().Count());
        Assert.False(dict5.Contains("a1", "b1", "c1", "d1", "e1"));
        Assert.False(dict5.Remove("a1", "b1", "c1", "d1", "e1"));
        Assert.True(dict5.Remove("a2", "b2", "c3", "d1", "e5"));
        Assert.Equal(7, dict5.Count);
        Assert.Equal(7, dict5.Enumerate().Count());
        Assert.Equal(7, dict5.EnumerateValues().Count());

        var slice45 = dict5.Slice1("a3");
        slice45.Remove("b2", "c2", "d2", "e8");
        Assert.Equal(6, dict5.Count);
        Assert.Equal(6, dict5.Enumerate().Count());
        Assert.Equal(6, dict5.EnumerateValues().Count());
        Assert.Equal(2, slice45.Count);
        Assert.False(dict5.Contains("a3", "b2", "c2", "d2", "e8"));
        Assert.False(slice45.Contains("b2", "c2", "d2", "e8"));

        dict5.Add("a3", "b2", "c2", "d2", "e8", 8);
        Assert.Equal(7, dict5.Count);
        Assert.Equal(7, dict5.Enumerate().Count());
        Assert.Equal(7, dict5.EnumerateValues().Count());
        Assert.Equal(3, slice45.Count);
        Assert.True(dict5.Contains("a3", "b2", "c2", "d2", "e8"));
        Assert.True(slice45.Contains("b2", "c2", "d2", "e8"));

        slice45 = dict5.Slice1("a1");
        slice45.Add("b1", "c1", "d1", "e1", 1);
        Assert.Equal(8, dict5.Count);
        Assert.Equal(8, dict5.Enumerate().Count());
        Assert.Equal(8, dict5.EnumerateValues().Count());
        Assert.Equal(3, slice45.Count);
        Assert.True(dict5.Contains("a1", "b1", "c1", "d1", "e1"));
        Assert.True(slice45.Contains("b1", "c1", "d1", "e1"));

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

        Assert.True(dict4.Remove("a1", "b1", "c1", "d1"));
        Assert.Equal(8, dict4.Count);
        Assert.Equal(8, dict4.Enumerate().Count());
        Assert.Equal(8, dict4.EnumerateValues().Count());
        Assert.False(dict4.Contains("a1", "b1", "c1", "d1"));
        Assert.False(dict4.Remove("a1", "b1", "c1", "d1"));
        Assert.True(dict4.Remove("a2", "b2", "c3", "d1"));
        Assert.Equal(7, dict4.Count);
        Assert.Equal(7, dict4.Enumerate().Count());
        Assert.Equal(7, dict4.EnumerateValues().Count());

        var slice34 = dict4.Slice1("a3");
        slice34.Remove("b2", "c2", "d2");
        Assert.Equal(6, dict4.Count);
        Assert.Equal(6, dict4.Enumerate().Count());
        Assert.Equal(6, dict4.EnumerateValues().Count());
        Assert.Equal(2, slice34.Count);
        Assert.False(dict4.Contains("a3", "b2", "c2", "d2"));
        Assert.False(slice34.Contains("b2", "c2", "d2"));

        dict4.Add("a3", "b2", "c2", "d2", 8);
        Assert.Equal(7, dict4.Count);
        Assert.Equal(7, dict4.Enumerate().Count());
        Assert.Equal(7, dict4.EnumerateValues().Count());
        Assert.Equal(3, slice34.Count);
        Assert.True(dict4.Contains("a3", "b2", "c2", "d2"));
        Assert.True(slice34.Contains("b2", "c2", "d2"));

        slice34 = dict4.Slice1("a1");
        slice34.Add("b1", "c1", "d1", 1);
        Assert.Equal(8, dict4.Count);
        Assert.Equal(8, dict4.Enumerate().Count());
        Assert.Equal(8, dict4.EnumerateValues().Count());
        Assert.Equal(3, slice34.Count);
        Assert.True(dict4.Contains("a1", "b1", "c1", "d1"));
        Assert.True(slice34.Contains("b1", "c1", "d1"));

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

        Assert.True(dict3.Remove("a1", "b1", "c1"));
        Assert.Equal(8, dict3.Count);
        Assert.Equal(8, dict3.Enumerate().Count());
        Assert.Equal(8, dict3.EnumerateValues().Count());
        Assert.False(dict3.Contains("a1", "b1", "c1"));
        Assert.False(dict3.Remove("a1", "b1", "c1"));
        Assert.True(dict3.Remove("a2", "b2", "c3"));
        Assert.Equal(7, dict3.Count);
        Assert.Equal(7, dict3.Enumerate().Count());
        Assert.Equal(7, dict3.EnumerateValues().Count());

        var slice23 = dict3.Slice1("a3");
        slice23.Remove("b2", "c2");
        Assert.Equal(6, dict3.Count);
        Assert.Equal(6, dict3.Enumerate().Count());
        Assert.Equal(6, dict3.EnumerateValues().Count());
        Assert.Equal(2, slice23.Count);
        Assert.False(dict3.Contains("a3", "b2", "c2"));
        Assert.False(slice23.Contains("b2", "c2"));

        dict3.Add("a3", "b2", "c2", 8);
        Assert.Equal(7, dict3.Count);
        Assert.Equal(7, dict3.Enumerate().Count());
        Assert.Equal(7, dict3.EnumerateValues().Count());
        Assert.Equal(3, slice23.Count);
        Assert.True(dict3.Contains("a3", "b2", "c2"));
        Assert.True(slice23.Contains("b2", "c2"));

        slice23 = dict3.Slice1("a1");
        slice23.Add("b1", "c1", 1);
        Assert.Equal(8, dict3.Count);
        Assert.Equal(8, dict3.Enumerate().Count());
        Assert.Equal(8, dict3.EnumerateValues().Count());
        Assert.Equal(3, slice23.Count);
        Assert.True(dict3.Contains("a1", "b1", "c1"));
        Assert.True(slice23.Contains("b1", "c1"));

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

        Assert.True(dict2.Remove("a1", "b1"));
        Assert.Equal(8, dict2.Count);
        Assert.Equal(8, dict2.Enumerate().Count());
        Assert.Equal(8, dict2.EnumerateValues().Count());
        Assert.False(dict2.Contains("a1", "b1"));
        Assert.False(dict2.Remove("a1", "b1"));
        Assert.True(dict2.Remove("a2", "b2"));
        Assert.Equal(7, dict2.Count);
        Assert.Equal(7, dict2.Enumerate().Count());
        Assert.Equal(7, dict2.EnumerateValues().Count());

        var slice12 = dict2.Slice1("a3");
        slice12.Remove("b2");
        Assert.Equal(6, dict2.Count);
        Assert.Equal(6, dict2.Enumerate().Count());
        Assert.Equal(6, dict2.EnumerateValues().Count());
        Assert.Equal(2, slice12.Count);
        Assert.False(dict2.Contains("a3", "b2"));
        Assert.False(slice12.Contains("b2"));

        dict2.Add("a3", "b2", 8);
        Assert.Equal(7, dict2.Count);
        Assert.Equal(7, dict2.Enumerate().Count());
        Assert.Equal(7, dict2.EnumerateValues().Count());
        Assert.Equal(3, slice12.Count);
        Assert.True(dict2.Contains("a3", "b2"));
        Assert.True(slice12.Contains("b2"));

        slice12 = dict2.Slice1("a1");
        Assert.False(slice12.ContainsKey("b1"));
        slice12.Add("b1", 1);
        Assert.Equal(8, dict2.Count);
        Assert.Equal(8, dict2.Enumerate().Count());
        Assert.Equal(8, dict2.EnumerateValues().Count());
        Assert.Equal(3, slice12.Count);
        Assert.True(dict2.Contains("a1", "b1"));
        Assert.True(slice12.Contains("b1"));

        var dict1 = new MultiKeyDictionary<string, int>();
        dict1["a1"] = 1;
        dict1["a2"] = 2;
        dict1["a3"] = 3;

        Assert.True(dict1.Remove("a1"));
        Assert.Equal(2, dict1.Count);
        Assert.Equal(2, dict1.Enumerate().Count());
        Assert.Equal(2, dict1.EnumerateValues().Count());
        Assert.False(dict1.Contains("a1"));
        Assert.False(dict1.Remove("a1"));
        Assert.True(dict1.Remove("a2"));
        Assert.Equal(1, dict1.Count);
        Assert.Equal(1, dict1.Enumerate().Count());
        Assert.Equal(1, dict1.EnumerateValues().Count());

        dict1.Add("a2", 2);
        Assert.Equal(2, dict1.Count);
        Assert.Equal(2, dict1.Enumerate().Count());
        Assert.Equal(2, dict1.EnumerateValues().Count());
        Assert.True(dict1.Contains("a2"));
    }

    [Fact]
    public void TestCompacting()
    {
        var dict5 = new MultiKeyDictionary<string, string, string, string, string, int>()
        {
            CompactingAbsoluteThreshold = 0,
            CompactingRelativeThreshold = 0
        };
        dict5["a1", "b1", "c1", "d1", "e1"] = 1;
        dict5["a1", "b2", "c1", "d2", "e2"] = 2;
        dict5["a1", "b3", "c2", "d1", "e3"] = 3;
        dict5["a2", "b1", "c2", "d2", "e4"] = 4;
        dict5["a2", "b2", "c3", "d1", "e5"] = 5;
        dict5["a2", "b3", "c3", "d2", "e6"] = 6;
        dict5["a3", "b1", "c1", "d1", "e7"] = 7;
        dict5["a3", "b2", "c2", "d2", "e8"] = 8;
        dict5["a3", "b3", "c3", "d1", "e9"] = 9;
        
        var unusedIndices = (ISet<int>)typeof(MultiKeyDictionary<string, string, string, string, string, int>)
            .GetField("_unusedIndices", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(dict5);
        Assert.Equal(9, dict5.Count);
        dict5.Remove("a1", "b1", "c1", "d1", "e1");
        Assert.Equal(8, dict5.Count);
        Assert.Equal(0, unusedIndices.Count);
        dict5.Remove("a1", "b2", "c1", "d2", "e2");
        Assert.Equal(7, dict5.Count);
        Assert.Equal(0, unusedIndices.Count);
        dict5.Remove("a1", "b3", "c2", "d1", "e3");
        Assert.Equal(6, dict5.Count);
        Assert.Equal(0, unusedIndices.Count);
        dict5.Remove("a2", "b1", "c2", "d2", "e4");
        Assert.Equal(5, dict5.Count);
        Assert.Equal(0, unusedIndices.Count);
        dict5.Remove("a2", "b2", "c3", "d1", "e5");
        Assert.Equal(4, dict5.Count);
        Assert.Equal(0, unusedIndices.Count);
        dict5.CompactingAbsoluteThreshold = 2;
        dict5.Remove("a2", "b3", "c3", "d2", "e6");
        Assert.Equal(3, dict5.Count);
        Assert.Equal(1, unusedIndices.Count);
        dict5.Remove("a3", "b1", "c1", "d1", "e7");
        Assert.Equal(2, dict5.Count);
        Assert.Equal(2, unusedIndices.Count);
        dict5.Remove("a3", "b2", "c2", "d2", "e8");
        Assert.Equal(1, dict5.Count);
        Assert.Equal(0, unusedIndices.Count);
        dict5.Remove("a3", "b3", "c3", "d1", "e9");
        Assert.Equal(0, dict5.Count);
        Assert.Equal(1, unusedIndices.Count);
        dict5.Add("a4", "b4", "c4", "d4", "e10", 10);
        Assert.Equal(1, dict5.Count);
        Assert.Equal(0, unusedIndices.Count);

        var dict4 = new MultiKeyDictionary<string, string, string, string, int>()
        {
            CompactingAbsoluteThreshold = 0,
            CompactingRelativeThreshold = 0
        };
        dict4["a1", "b1", "c1", "d1"] = 1;
        dict4["a1", "b2", "c1", "d2"] = 2;
        dict4["a1", "b3", "c2", "d1"] = 3;
        dict4["a2", "b1", "c2", "d2"] = 4;
        dict4["a2", "b2", "c3", "d1"] = 5;
        dict4["a2", "b3", "c3", "d2"] = 6;
        dict4["a3", "b1", "c1", "d1"] = 7;
        dict4["a3", "b2", "c2", "d2"] = 8;
        dict4["a3", "b3", "c3", "d1"] = 9;

        unusedIndices = (ISet<int>)typeof(MultiKeyDictionary<string, string, string, string, int>)
            .GetField("_unusedIndices", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(dict4);
        Assert.Equal(9, dict4.Count);
        dict4.Remove("a1", "b1", "c1", "d1");
        Assert.Equal(8, dict4.Count);
        Assert.Equal(0, unusedIndices.Count);
        dict4.Remove("a1", "b2", "c1", "d2");
        Assert.Equal(7, dict4.Count);
        Assert.Equal(0, unusedIndices.Count);
        dict4.Remove("a1", "b3", "c2", "d1");
        Assert.Equal(6, dict4.Count);
        Assert.Equal(0, unusedIndices.Count);
        dict4.Remove("a2", "b1", "c2", "d2");
        Assert.Equal(5, dict4.Count);
        Assert.Equal(0, unusedIndices.Count);
        dict4.Remove("a2", "b2", "c3", "d1");
        Assert.Equal(4, dict4.Count);
        Assert.Equal(0, unusedIndices.Count);
        dict4.CompactingAbsoluteThreshold = 2;
        dict4.Remove("a2", "b3", "c3", "d2");
        Assert.Equal(3, dict4.Count);
        Assert.Equal(1, unusedIndices.Count);
        dict4.Remove("a3", "b1", "c1", "d1");
        Assert.Equal(2, dict4.Count);
        Assert.Equal(2, unusedIndices.Count);
        dict4.Remove("a3", "b2", "c2", "d2");
        Assert.Equal(1, dict4.Count);
        Assert.Equal(0, unusedIndices.Count);
        dict4.Remove("a3", "b3", "c3", "d1");
        Assert.Equal(0, dict4.Count);
        Assert.Equal(1, unusedIndices.Count);
        dict4.Add("a4", "b4", "c4", "d4", 10);
        Assert.Equal(1, dict4.Count);
        Assert.Equal(0, unusedIndices.Count);

        var dict3 = new MultiKeyDictionary<string, string, string, int>()
        {
            CompactingAbsoluteThreshold = 0,
            CompactingRelativeThreshold = 0
        };
        dict3["a1", "b1", "c1"] = 1;
        dict3["a1", "b2", "c2"] = 2;
        dict3["a1", "b3", "c3"] = 3;
        dict3["a2", "b1", "c1"] = 4;
        dict3["a2", "b2", "c2"] = 5;
        dict3["a2", "b3", "c3"] = 6;
        dict3["a3", "b1", "c1"] = 7;
        dict3["a3", "b2", "c2"] = 8;
        dict3["a3", "b3", "c3"] = 9;

        unusedIndices = (ISet<int>)typeof(MultiKeyDictionary<string, string, string, int>)
            .GetField("_unusedIndices", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(dict3);
        Assert.Equal(9, dict3.Count);
        dict3.Remove("a1", "b1", "c1");
        Assert.Equal(8, dict3.Count);
        Assert.Equal(0, unusedIndices.Count);
        dict3.Remove("a1", "b2", "c2");
        Assert.Equal(7, dict3.Count);
        Assert.Equal(0, unusedIndices.Count);
        dict3.Remove("a1", "b3", "c3");
        Assert.Equal(6, dict3.Count);
        Assert.Equal(0, unusedIndices.Count);
        dict3.Remove("a2", "b1", "c1");
        Assert.Equal(5, dict3.Count);
        Assert.Equal(0, unusedIndices.Count);
        dict3.Remove("a2", "b2", "c2");
        Assert.Equal(4, dict3.Count);
        Assert.Equal(0, unusedIndices.Count);
        dict3.CompactingAbsoluteThreshold = 2;
        dict3.Remove("a2", "b3", "c3");
        Assert.Equal(3, dict3.Count);
        Assert.Equal(1, unusedIndices.Count);
        dict3.Remove("a3", "b1", "c1");
        Assert.Equal(2, dict3.Count);
        Assert.Equal(2, unusedIndices.Count);
        dict3.Remove("a3", "b2", "c2");
        Assert.Equal(1, dict3.Count);
        Assert.Equal(0, unusedIndices.Count);
        dict3.Remove("a3", "b3", "c3");
        Assert.Equal(0, dict3.Count);
        Assert.Equal(1, unusedIndices.Count);
        dict3.Add("a4", "b4", "c4", 10);
        Assert.Equal(1, dict3.Count);
        Assert.Equal(0, unusedIndices.Count);

        var dict2 = new MultiKeyDictionary<string, string, int>()
        {
            CompactingAbsoluteThreshold = 0,
            CompactingRelativeThreshold = 0
        };
        dict2["a1", "b1"] = 1;
        dict2["a1", "b2"] = 2;
        dict2["a1", "b3"] = 3;
        dict2["a2", "b1"] = 4;
        dict2["a2", "b2"] = 5;
        dict2["a2", "b3"] = 6;
        dict2["a3", "b1"] = 7;
        dict2["a3", "b2"] = 8;
        dict2["a3", "b3"] = 9;

        unusedIndices = (ISet<int>)typeof(MultiKeyDictionary<string, string, int>)
            .GetField("_unusedIndices", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(dict2);
        Assert.Equal(9, dict2.Count);
        dict2.Remove("a1", "b1");
        Assert.Equal(8, dict2.Count);
        Assert.Equal(0, unusedIndices.Count);
        dict2.Remove("a1", "b2");
        Assert.Equal(7, dict2.Count);
        Assert.Equal(0, unusedIndices.Count);
        dict2.Remove("a1", "b3");
        Assert.Equal(6, dict2.Count);
        Assert.Equal(0, unusedIndices.Count);
        dict2.Remove("a2", "b1");
        Assert.Equal(5, dict2.Count);
        Assert.Equal(0, unusedIndices.Count);
        dict2.Remove("a2", "b2");
        Assert.Equal(4, dict2.Count);
        Assert.Equal(0, unusedIndices.Count);
        dict2.CompactingAbsoluteThreshold = 2;
        dict2.Remove("a2", "b3");
        Assert.Equal(3, dict2.Count);
        Assert.Equal(1, unusedIndices.Count);
        dict2.Remove("a3", "b1");
        Assert.Equal(2, dict2.Count);
        Assert.Equal(2, unusedIndices.Count);
        dict2.Remove("a3", "b2");
        Assert.Equal(1, dict2.Count);
        Assert.Equal(0, unusedIndices.Count);
        dict2.Remove("a3", "b3");
        Assert.Equal(0, dict2.Count);
        Assert.Equal(1, unusedIndices.Count);
        dict2.Add("a4", "b4", 10);
        Assert.Equal(1, dict2.Count);
        Assert.Equal(0, unusedIndices.Count);

        var dict1 = new MultiKeyDictionary<string, int>()
        {
            CompactingAbsoluteThreshold = 0,
            CompactingRelativeThreshold = 0
        };
        dict1["a1"] = 1;
        dict1["a2"] = 2;
        dict1["a3"] = 3;
        dict1["a4"] = 4;
        dict1["a5"] = 5;
        dict1["a6"] = 6;

        unusedIndices = (ISet<int>)typeof(MultiKeyDictionary<string, int>)
            .GetField("_unusedIndices", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(dict1);
        Assert.Equal(6, dict1.Count);
        dict1.Remove("a1");
        Assert.Equal(5, dict1.Count);
        Assert.Equal(0, unusedIndices.Count);
        dict1.Remove("a2");
        Assert.Equal(4, dict1.Count);
        Assert.Equal(0, unusedIndices.Count);
        dict1.CompactingAbsoluteThreshold = 2;
        dict1.Remove("a3");
        Assert.Equal(3, dict1.Count);
        Assert.Equal(1, unusedIndices.Count);
        dict1.Remove("a4");
        Assert.Equal(2, dict1.Count);
        Assert.Equal(2, unusedIndices.Count);
        dict1.Remove("a5");
        Assert.Equal(1, dict1.Count);
        Assert.Equal(0, unusedIndices.Count);
        dict1.Remove("a6");
        Assert.Equal(0, dict1.Count);
        Assert.Equal(1, unusedIndices.Count);
        dict1.Add("a7", 7);
        Assert.Equal(1, dict1.Count);
        Assert.Equal(0, unusedIndices.Count);
    }

    [Fact]
    public void TestRemoveX()
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

        Assert.Equal(3, dict5.Remove1("a1"));
        Assert.Equal(6, dict5.Count);
        Assert.Equal(2, dict5.Remove2("b1"));
        Assert.Equal(4, dict5.Count);
        Assert.Equal(1, dict5.Remove3("c2"));
        Assert.Equal(3, dict5.Count);
        Assert.Equal(2, dict5.Remove4("d1"));
        Assert.Equal(1, dict5.Count);
        Assert.Equal(1, dict5.Remove5("e6"));
        Assert.Equal(0, dict5.Count);

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

        Assert.Equal(3, dict4.Remove1("a1"));
        Assert.Equal(6, dict4.Count);
        Assert.Equal(2, dict4.Remove2("b1"));
        Assert.Equal(4, dict4.Count);
        Assert.Equal(1, dict4.Remove3("c2"));
        Assert.Equal(3, dict4.Count);
        Assert.Equal(2, dict4.Remove4("d1"));
        Assert.Equal(1, dict4.Count);
        Assert.Equal(1, dict4.Remove4("d2"));
        Assert.Equal(0, dict4.Count);

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

        Assert.Equal(3, dict3.Remove1("a1"));
        Assert.Equal(6, dict3.Count);
        Assert.Equal(2, dict3.Remove2("b1"));
        Assert.Equal(4, dict3.Count);
        Assert.Equal(1, dict3.Remove3("c2"));
        Assert.Equal(3, dict3.Count);
        Assert.Equal(3, dict3.Remove3("c3"));
        Assert.Equal(0, dict3.Count);

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

        Assert.Equal(3, dict2.Remove1("a1"));
        Assert.Equal(6, dict2.Count);
        Assert.Equal(2, dict2.Remove2("b1"));
        Assert.Equal(4, dict2.Count);
        Assert.Equal(2, dict2.Remove2("b2"));
        Assert.Equal(2, dict2.Count);
        Assert.Equal(2, dict2.Remove2("b3"));
        Assert.Equal(0, dict2.Count);
    }
}