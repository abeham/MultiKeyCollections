using Xunit;

namespace MultiKeyCollections.Tests;

public class SetTests
{
    [Fact]
    public void TestSet()
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

        dict5.Set(1);
        Assert.Equal(1, dict5["a1", "b1", "c1", "d1", "e1"]);
        Assert.Equal(1, dict5["a1", "b2", "c1", "d2", "e2"]);
        Assert.Equal(1, dict5["a1", "b3", "c2", "d1", "e3"]);
        Assert.Equal(1, dict5["a2", "b1", "c2", "d2", "e4"]);
        Assert.Equal(1, dict5["a2", "b2", "c3", "d1", "e5"]);
        Assert.Equal(1, dict5["a2", "b3", "c3", "d2", "e6"]);
        Assert.Equal(1, dict5["a3", "b1", "c1", "d1", "e7"]);
        Assert.Equal(1, dict5["a3", "b2", "c2", "d2", "e8"]);
        Assert.Equal(1, dict5["a3", "b3", "c3", "d1", "e9"]);
        Assert.Equal(9, dict5.Count);
        Assert.Equal(9, dict5.Sum());

        var slice45 = dict5.Slice1("a1");
        slice45.Set(2);
        Assert.Equal(2, dict5["a1", "b1", "c1", "d1", "e1"]);
        Assert.Equal(2, dict5["a1", "b2", "c1", "d2", "e2"]);
        Assert.Equal(2, dict5["a1", "b3", "c2", "d1", "e3"]);
        Assert.Equal(1, dict5["a2", "b1", "c2", "d2", "e4"]);
        Assert.Equal(1, dict5["a2", "b2", "c3", "d1", "e5"]);
        Assert.Equal(1, dict5["a2", "b3", "c3", "d2", "e6"]);
        Assert.Equal(1, dict5["a3", "b1", "c1", "d1", "e7"]);
        Assert.Equal(1, dict5["a3", "b2", "c2", "d2", "e8"]);
        Assert.Equal(1, dict5["a3", "b3", "c3", "d1", "e9"]);
        Assert.Equal(6, slice45.Sum());
        Assert.Equal(3, slice45.Count);
        Assert.Equal(9, dict5.Count);
        Assert.Equal(12, dict5.Sum());

        var slice345 = slice45.Slice2("c1");
        slice345.Set(3);
        Assert.Equal(3, dict5["a1", "b1", "c1", "d1", "e1"]);
        Assert.Equal(3, dict5["a1", "b2", "c1", "d2", "e2"]);
        Assert.Equal(2, dict5["a1", "b3", "c2", "d1", "e3"]);
        Assert.Equal(1, dict5["a2", "b1", "c2", "d2", "e4"]);
        Assert.Equal(1, dict5["a2", "b2", "c3", "d1", "e5"]);
        Assert.Equal(1, dict5["a2", "b3", "c3", "d2", "e6"]);
        Assert.Equal(1, dict5["a3", "b1", "c1", "d1", "e7"]);
        Assert.Equal(1, dict5["a3", "b2", "c2", "d2", "e8"]);
        Assert.Equal(1, dict5["a3", "b3", "c3", "d1", "e9"]);
        Assert.Equal(6, slice345.Sum());
        Assert.Equal(2, slice345.Count);
        Assert.Equal(8, slice45.Sum());
        Assert.Equal(14, dict5.Sum());

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

        dict4.Set(1);
        Assert.Equal(1, dict4["a1", "b1", "c1", "d1"]);
        Assert.Equal(1, dict4["a1", "b2", "c1", "d2"]);
        Assert.Equal(1, dict4["a1", "b3", "c2", "d1"]);
        Assert.Equal(1, dict4["a2", "b1", "c2", "d2"]);
        Assert.Equal(1, dict4["a2", "b2", "c3", "d1"]);
        Assert.Equal(1, dict4["a2", "b3", "c3", "d2"]);
        Assert.Equal(1, dict4["a3", "b1", "c1", "d1"]);
        Assert.Equal(1, dict4["a3", "b2", "c2", "d2"]);
        Assert.Equal(1, dict4["a3", "b3", "c3", "d1"]);
        Assert.Equal(9, dict4.Count);
        Assert.Equal(9, dict4.Sum());

        var slice34 = dict4.Slice1("a1");
        slice34.Set(2);
        Assert.Equal(2, dict4["a1", "b1", "c1", "d1"]);
        Assert.Equal(2, dict4["a1", "b2", "c1", "d2"]);
        Assert.Equal(2, dict4["a1", "b3", "c2", "d1"]);
        Assert.Equal(1, dict4["a2", "b1", "c2", "d2"]);
        Assert.Equal(1, dict4["a2", "b2", "c3", "d1"]);
        Assert.Equal(1, dict4["a2", "b3", "c3", "d2"]);
        Assert.Equal(1, dict4["a3", "b1", "c1", "d1"]);
        Assert.Equal(1, dict4["a3", "b2", "c2", "d2"]);
        Assert.Equal(1, dict4["a3", "b3", "c3", "d1"]);
        Assert.Equal(6, slice34.Sum());
        Assert.Equal(3, slice34.Count);
        Assert.Equal(9, dict4.Count);
        Assert.Equal(12, dict4.Sum());

        var slice234 = slice34.Slice2("c1");
        slice234.Set(3);
        Assert.Equal(3, dict4["a1", "b1", "c1", "d1"]);
        Assert.Equal(3, dict4["a1", "b2", "c1", "d2"]);
        Assert.Equal(2, dict4["a1", "b3", "c2", "d1"]);
        Assert.Equal(1, dict4["a2", "b1", "c2", "d2"]);
        Assert.Equal(1, dict4["a2", "b2", "c3", "d1"]);
        Assert.Equal(1, dict4["a2", "b3", "c3", "d2"]);
        Assert.Equal(1, dict4["a3", "b1", "c1", "d1"]);
        Assert.Equal(1, dict4["a3", "b2", "c2", "d2"]);
        Assert.Equal(1, dict4["a3", "b3", "c3", "d1"]);
        Assert.Equal(6, slice234.Sum());
        Assert.Equal(2, slice234.Count);
        Assert.Equal(8, slice34.Sum());
        Assert.Equal(3, slice34.Count);
        Assert.Equal(9, dict4.Count);
        Assert.Equal(14, dict4.Sum());

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

        dict3.Set(1);
        Assert.Equal(1, dict3["a1", "b1", "c1"]);
        Assert.Equal(1, dict3["a1", "b2", "c1"]);
        Assert.Equal(1, dict3["a1", "b3", "c2"]);
        Assert.Equal(1, dict3["a2", "b1", "c2"]);
        Assert.Equal(1, dict3["a2", "b2", "c3"]);
        Assert.Equal(1, dict3["a2", "b3", "c3"]);
        Assert.Equal(1, dict3["a3", "b1", "c1"]);
        Assert.Equal(1, dict3["a3", "b2", "c2"]);
        Assert.Equal(1, dict3["a3", "b3", "c3"]);
        Assert.Equal(9, dict3.Count);
        Assert.Equal(9, dict3.Sum());
        
        var slice23 = dict3.Slice1("a1");
        slice23.Set(2);
        Assert.Equal(2, dict3["a1", "b1", "c1"]);
        Assert.Equal(2, dict3["a1", "b2", "c1"]);
        Assert.Equal(2, dict3["a1", "b3", "c2"]);
        Assert.Equal(1, dict3["a2", "b1", "c2"]);
        Assert.Equal(1, dict3["a2", "b2", "c3"]);
        Assert.Equal(1, dict3["a2", "b3", "c3"]);
        Assert.Equal(1, dict3["a3", "b1", "c1"]);
        Assert.Equal(1, dict3["a3", "b2", "c2"]);
        Assert.Equal(1, dict3["a3", "b3", "c3"]);
        Assert.Equal(6, slice23.Sum());
        Assert.Equal(3, slice23.Count);
        Assert.Equal(9, dict3.Count);
        Assert.Equal(12, dict3.Sum());

        var slice123 = slice23.Slice2("c1");
        slice123.Set(3);
        Assert.Equal(3, dict3["a1", "b1", "c1"]);
        Assert.Equal(3, dict3["a1", "b2", "c1"]);
        Assert.Equal(2, dict3["a1", "b3", "c2"]);
        Assert.Equal(1, dict3["a2", "b1", "c2"]);
        Assert.Equal(1, dict3["a2", "b2", "c3"]);
        Assert.Equal(1, dict3["a2", "b3", "c3"]);
        Assert.Equal(1, dict3["a3", "b1", "c1"]);
        Assert.Equal(1, dict3["a3", "b2", "c2"]);
        Assert.Equal(1, dict3["a3", "b3", "c3"]);
        Assert.Equal(6, slice123.Sum());
        Assert.Equal(2, slice123.Count);
        Assert.Equal(8, slice23.Sum());
        Assert.Equal(3, slice23.Count);
        Assert.Equal(9, dict3.Count);
        Assert.Equal(14, dict3.Sum());

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

        dict2.Set(1);
        Assert.Equal(1, dict2["a1", "b1"]);
        Assert.Equal(1, dict2["a1", "b2"]);
        Assert.Equal(1, dict2["a1", "b3"]);
        Assert.Equal(1, dict2["a2", "b1"]);
        Assert.Equal(1, dict2["a2", "b2"]);
        Assert.Equal(1, dict2["a2", "b3"]);
        Assert.Equal(1, dict2["a3", "b1"]);
        Assert.Equal(1, dict2["a3", "b2"]);
        Assert.Equal(1, dict2["a3", "b3"]);
        Assert.Equal(9, dict2.Count);
        Assert.Equal(9, dict2.Sum());
        
        var slice12 = dict2.Slice1("a1");
        slice12.Set(2);
        Assert.Equal(2, dict2["a1", "b1"]);
        Assert.Equal(2, dict2["a1", "b2"]);
        Assert.Equal(2, dict2["a1", "b3"]);
        Assert.Equal(1, dict2["a2", "b1"]);
        Assert.Equal(1, dict2["a2", "b2"]);
        Assert.Equal(1, dict2["a2", "b3"]);
        Assert.Equal(1, dict2["a3", "b1"]);
        Assert.Equal(1, dict2["a3", "b2"]);
        Assert.Equal(1, dict2["a3", "b3"]);
        Assert.Equal(6, slice12.Sum());
        Assert.Equal(3, slice12.Count);
        Assert.Equal(9, dict2.Count);
        Assert.Equal(12, dict2.Sum());

        var dict1 = new MultiKeyDictionary<string, int>();
        dict1["a1"] = 1;
        dict1["a2"] = 2;
        dict1["a3"] = 3;

        dict1.Set(1);
        Assert.Equal(1, dict1["a1"]);
        Assert.Equal(1, dict1["a2"]);
        Assert.Equal(1, dict1["a3"]);
        Assert.Equal(3, dict1.Count);
        Assert.Equal(3, dict1.Sum());
        
    }
}