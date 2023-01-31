using Xunit;

namespace TableCollections.Tests
{
    public class ReadmeTests
    {
        [Fact]
        public void TestReadmeCode()
        {
            var dict = new MultiKeyDictionary<int, string, string, int>();
            dict.Add(1, "a", "x", 100);
            Assert.True(dict.Contains(1, "a", "x")); // true
            dict.Add(1, "a", "y", 200);
            Assert.True(dict.Contains(1, "a", "y")); // true
            dict.Add(1, "b", "x", 300);
            Assert.True(dict.Contains(1, "b", "x")); // true
            dict.Add(1, "b", "y", 400);

            dict.Slice2("a").Add(2, "x", 500);
            Assert.True(dict.Contains(2, "a", "x")); // true
            Assert.False(dict.Contains(2, "b", "x")); // false
            Assert.Equal(500, dict[2, "a", "x"]); // 500

            var slice = dict.Slice1(1);
            slice["c", "z"] = 600;
            Assert.True(dict.Contains(1, "c", "z")); // true
            Assert.Equal(600, dict[1, "c", "z"]); // 600

            Assert.Equal(6, dict.ToList().Count); // returns a list of all values { 100, 200, 300, 400, 500, 600 } (order not guaranteed)
            Assert.Contains(100, dict.ToList());
            Assert.Contains(200, dict.ToList());
            Assert.Contains(300, dict.ToList());
            Assert.Contains(400, dict.ToList());
            Assert.Contains(500, dict.ToList());
            Assert.Contains(600, dict.ToList());
            Assert.Equal(2, dict.Slice2("b").ToList().Count); // returns a list of all values { 300, 400 } (order not guaranteed)
            Assert.Contains(300, dict.Slice2("b").ToList());
            Assert.Contains(400, dict.Slice2("b").ToList());

            // Slices contain a view of the data and also hold only a reference to the indices. Thus, adding to the
            // higher level representation, is also reflected in any slice:
            dict.Add(1, "c", "x", 700); // the key (1, "c", "x") was not present when slice was created
            Assert.True(dict.Contains(1, "c", "x")); // true
            Assert.True(slice.Contains("c", "x")); // true
            Assert.Equal(700, slice["c", "x"]); // 700
        }
    }
}