namespace MultiKeyCollections.Tests;

using Xunit;

public class AddTests
{
    [Fact]
    public void TestAdd()
    {
        var dict5 = new MultiKeyDictionary<string, string, string, string, string, int>();
        dict5.Add(("a1", "b1", "c1", "d1", "e1"), 1);
        Assert.Equal(1, dict5.Count);
        Assert.Equal(1, dict5[("a1", "b1", "c1", "d1", "e1")]);
        dict5.Add(("a1", "b2", "c1", "d2", "e2"), 2);
        Assert.Equal(2, dict5.Count);
        Assert.Equal(2, dict5[("a1", "b2", "c1", "d2", "e2")]);
        dict5.Add(("a1", "b3", "c2", "d1", "e3"), 3);
        Assert.Equal(3, dict5.Count);
        Assert.Equal(3, dict5[("a1", "b3", "c2", "d1", "e3")]);

        var dict4 = new MultiKeyDictionary<string, string, string, string, int>();
        dict4.Add(("a1", "b1", "c1", "d1"), 1);
        Assert.Equal(1, dict4.Count);
        Assert.Equal(1, dict4[("a1", "b1", "c1", "d1")]);
        dict4.Add(("a1", "b2", "c1", "d2"), 2);
        Assert.Equal(2, dict4.Count);
        Assert.Equal(2, dict4[("a1", "b2", "c1", "d2")]);
        dict4.Add(("a1", "b3", "c2", "d1"), 3);
        Assert.Equal(3, dict4.Count);
        Assert.Equal(3, dict4[("a1", "b3", "c2", "d1")]);

        var dict3 = new MultiKeyDictionary<string, string, string, int>();
        dict3.Add(("a1", "b1", "c1"), 1);
        Assert.Equal(1, dict3.Count);
        Assert.Equal(1, dict3[("a1", "b1", "c1")]);
        dict3.Add(("a1", "b2", "c1"), 2);
        Assert.Equal(2, dict3.Count);
        Assert.Equal(2, dict3[("a1", "b2", "c1")]);
        dict3.Add(("a1", "b3", "c2"), 3);
        Assert.Equal(3, dict3.Count);
        Assert.Equal(3, dict3[("a1", "b3", "c2")]);

        var dict2 = new MultiKeyDictionary<string, string, int>();
        dict2.Add(("a1", "b1"), 1);
        Assert.Equal(1, dict2.Count);
        Assert.Equal(1, dict2[("a1", "b1")]);
        dict2.Add(("a1", "b2"), 2);
        Assert.Equal(2, dict2.Count);
        Assert.Equal(2, dict2[("a1", "b2")]);
        dict2.Add(("a1", "b3"), 3);
        Assert.Equal(3, dict2.Count);
        Assert.Equal(3, dict2[("a1", "b3")]);

    }
}