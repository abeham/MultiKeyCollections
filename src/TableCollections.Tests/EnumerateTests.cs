using Xunit;

namespace TableCollections.Tests
{
    public class EnuemrateTests
    {
        [Fact]
        public void TestEnumerate()
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

            var expected5 = new List<(string, string, string, string, string, int)>()
            {
                ("a1", "b1", "c1", "d1", "e1", 1),
                ("a1", "b2", "c1", "d2", "e2", 2),
                ("a1", "b3", "c2", "d1", "e3", 3),
                ("a2", "b1", "c2", "d2", "e4", 4),
                ("a2", "b2", "c3", "d1", "e5", 5),
                ("a2", "b3", "c3", "d2", "e6", 6),
                ("a3", "b1", "c1", "d1", "e7", 7),
                ("a3", "b2", "c2", "d2", "e8", 8),
                ("a3", "b3", "c3", "d1", "e9", 9),
            };

            Assert.Equal(expected5, table5.Enumerate());

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

            var expected4 = new List<(string, string, string, string, int)>()
            {
                ("a1", "b1", "c1", "d1", 1),
                ("a1", "b2", "c1", "d2", 2),
                ("a1", "b3", "c2", "d1", 3),
                ("a2", "b1", "c2", "d2", 4),
                ("a2", "b2", "c3", "d1", 5),
                ("a2", "b3", "c3", "d2", 6),
                ("a3", "b1", "c1", "d1", 7),
                ("a3", "b2", "c2", "d2", 8),
                ("a3", "b3", "c3", "d1", 9),
            };

            Assert.Equal(expected4, table4.Enumerate());

            var table3 = new IndexedTable<string, string, string, int>();
            table3["a1", "b1", "c1"] = 1;
            table3["a1", "b2", "c2"] = 2;
            table3["a1", "b3", "c3"] = 3;
            table3["a2", "b1", "c1"] = 4;
            table3["a2", "b2", "c2"] = 5;
            table3["a2", "b3", "c3"] = 6;
            table3["a3", "b1", "c1"] = 7;
            table3["a3", "b2", "c2"] = 8;
            table3["a3", "b3", "c3"] = 9;

            var expected3 = new List<(string, string, string, int)>()
            {
                ("a1", "b1", "c1", 1),
                ("a1", "b2", "c2", 2),
                ("a1", "b3", "c3", 3),
                ("a2", "b1", "c1", 4),
                ("a2", "b2", "c2", 5),
                ("a2", "b3", "c3", 6),
                ("a3", "b1", "c1", 7),
                ("a3", "b2", "c2", 8),
                ("a3", "b3", "c3", 9),
            };

            Assert.Equal(expected3, table3.Enumerate());

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

            var expected2 = new List<(string, string, int)>()
            {
                ("a1", "b1", 1),
                ("a1", "b2", 2),
                ("a1", "b3", 3),
                ("a2", "b1", 4),
                ("a2", "b2", 5),
                ("a2", "b3", 6),
                ("a3", "b1", 7),
                ("a3", "b2", 8),
                ("a3", "b3", 9),
            };

            Assert.Equal(expected2, table2.Enumerate());

            var table = new IndexedTable<string, int>();
            table["a"] = 1;
            table["b"] = 2;
            table["c"] = 3;
            table["d"] = 4;
            table["e"] = 5;
            table["f"] = 6;

            var expected = new List<(string, int)>()
            {
                ("a", 1),
                ("b", 2),
                ("c", 3),
                ("d", 4),
                ("e", 5),
                ("f", 6),
            };

            Assert.Equal(expected, table.Enumerate());
        }
    }
}