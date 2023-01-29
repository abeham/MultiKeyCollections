using Xunit;

namespace TableCollections.Tests
{
    public class ReadmeTests
    {
        [Fact]
        public void TestReadmeCode()
        {
            var table = new IndexedTable<int, string, string, int>();
            Assert.True(table.Add(1, "a", "x", 100));
            Assert.True(table.Add(1, "a", "y", 200));
            Assert.True(table.Add(1, "b", "x", 300));
            Assert.True(table.Add(1, "b", "y", 400));

            table.Slice2("a").Add(2, "x", 500);
            Assert.True(table.Contains(2, "a", "x")); // true
            Assert.False(table.Contains(2, "b", "x")); // false
            Assert.Equal(500, table[2, "a", "x"]); // 500

            var slice = table.Slice1(1);
            slice["c", "z"] = 600;
            Assert.True(table.Contains(1, "c", "z")); // true
            Assert.Equal(600, table[1, "c", "z"]); // 600

            Assert.Equal(6, table.ToList().Count); // returns a list of all values { 100, 200, 300, 400, 500, 600 } (order not guaranteed)
            Assert.Contains(100, table.ToList());
            Assert.Contains(200, table.ToList());
            Assert.Contains(300, table.ToList());
            Assert.Contains(400, table.ToList());
            Assert.Contains(500, table.ToList());
            Assert.Contains(600, table.ToList());
            Assert.Equal(2, table.Slice2("b").ToList().Count); // returns a list of all values { 300, 400 } (order not guaranteed)
            Assert.Contains(300, table.Slice2("b").ToList());
            Assert.Contains(400, table.Slice2("b").ToList());

            // Slices contain a view of the data and also hold only a reference to the indices. Thus, adding to the
            // higher level representation, is also reflected in any slice:
            Assert.True(table.Add(1, "c", "x", 700)); // the key (1, "c", "x") was not present when slice was created
            Assert.True(slice.Contains("c", "x")); // true
            Assert.Equal(700, slice["c", "x"]); // 700
        }
    }
}