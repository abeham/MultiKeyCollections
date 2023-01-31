using Xunit;

namespace TableCollections.Tests
{
    public class EnuemrateTests
    {
        [Fact]
        public void TestEnumerate()
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

            Assert.Equal(expected5, dict5.Enumerate());
            var expectedSlice4 = new List<(string, string, string, string, int)>()
            {
                ("b1", "c1", "d1", "e1", 1),
                ("b2", "c1", "d2", "e2", 2),
                ("b3", "c2", "d1", "e3", 3),
            };
            Assert.Equal(expectedSlice4, dict5.Slice1("a1").Enumerate());

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

            Assert.Equal(expected4, dict4.Enumerate());
            var expectedSlice3 = new List<(string, string, string, int)>()
            {
                ("b1", "c1", "d1", 1),
                ("b2", "c1", "d2", 2),
                ("b3", "c2", "d1", 3),
            };
            Assert.Equal(expectedSlice3, dict4.Slice1("a1").Enumerate());

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

            Assert.Equal(expected3, dict3.Enumerate());
            var expectedSlice2 = new List<(string, string, int)>()
            {
                ("b1", "c1", 1),
                ("b2", "c2", 2),
                ("b3", "c3", 3),
            };
            Assert.Equal(expectedSlice2, dict3.Slice1("a1").Enumerate());

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

            Assert.Equal(expected2, dict2.Enumerate());
            var expectedSlice1 = new List<(string, int)>()
            {
                ("b1", 1),
                ("b2", 2),
                ("b3", 3),
            };
            Assert.Equal(expectedSlice1, dict2.Slice1("a1").Enumerate());

            var dict = new MultiKeyDictionary<string, int>();
            dict["a"] = 1;
            dict["b"] = 2;
            dict["c"] = 3;
            dict["d"] = 4;
            dict["e"] = 5;
            dict["f"] = 6;

            var expected = new List<(string, int)>()
            {
                ("a", 1),
                ("b", 2),
                ("c", 3),
                ("d", 4),
                ("e", 5),
                ("f", 6),
            };

            Assert.Equal(expected, dict.Enumerate());
        }
    }
}