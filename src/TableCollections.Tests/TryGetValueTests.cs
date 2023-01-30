using Xunit;

namespace TableCollections.Tests
{
    public class TryGetValueTests
    {
        [Fact]
        public void TestTryGetValue()
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

            Assert.True(table5.TryGetValue("a1", "b1", "c1", "d1", "e1", out var value1));
            Assert.Equal(1, value1);
            Assert.True(table5.TryGetValue("a1", "b2", "c1", "d2", "e2", out var value2));
            Assert.Equal(2, value2);
            Assert.True(table5.TryGetValue("a1", "b3", "c2", "d1", "e3", out var value3));
            Assert.Equal(3, value3);
            Assert.True(table5.TryGetValue("a2", "b1", "c2", "d2", "e4", out var value4));
            Assert.Equal(4, value4);
            Assert.True(table5.TryGetValue("a2", "b2", "c3", "d1", "e5", out var value5));
            Assert.Equal(5, value5);
            Assert.True(table5.TryGetValue("a2", "b3", "c3", "d2", "e6", out var value6));
            Assert.Equal(6, value6);
            Assert.True(table5.TryGetValue("a3", "b1", "c1", "d1", "e7", out var value7));
            Assert.Equal(7, value7);
            Assert.True(table5.TryGetValue("a3", "b2", "c2", "d2", "e8", out var value8));
            Assert.Equal(8, value8);
            Assert.True(table5.TryGetValue("a3", "b3", "c3", "d1", "e9", out var value9));
            Assert.Equal(9, value9);
            Assert.False(table5.TryGetValue("a1", "b1", "c1", "d1", "e2", out var value10));
            Assert.False(table5.TryGetValue("a1", "b1", "c1", "d2", "e1", out var value11));
            Assert.False(table5.TryGetValue("a5", "b1", "c1", "d1", "e1", out var value12));
            Assert.False(table5.TryGetValue("a2", "b3", "c1", "d1", "e6", out var value13));

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

            Assert.True(table4.TryGetValue("a1", "b1", "c1", "d1", out var value14));
            Assert.Equal(1, value14);
            Assert.True(table4.TryGetValue("a1", "b2", "c1", "d2", out var value15));
            Assert.Equal(2, value15);
            Assert.True(table4.TryGetValue("a1", "b3", "c2", "d1", out var value16));
            Assert.Equal(3, value16);
            Assert.True(table4.TryGetValue("a2", "b1", "c2", "d2", out var value17));
            Assert.Equal(4, value17);
            Assert.True(table4.TryGetValue("a2", "b2", "c3", "d1", out var value18));
            Assert.Equal(5, value18);
            Assert.True(table4.TryGetValue("a2", "b3", "c3", "d2", out var value19));
            Assert.Equal(6, value19);
            Assert.True(table4.TryGetValue("a3", "b1", "c1", "d1", out var value20));
            Assert.Equal(7, value20);
            Assert.True(table4.TryGetValue("a3", "b2", "c2", "d2", out var value21));
            Assert.Equal(8, value21);
            Assert.True(table4.TryGetValue("a3", "b3", "c3", "d1", out var value22));
            Assert.Equal(9, value22);
            Assert.False(table4.TryGetValue("a1", "b1", "c1", "d2", out var value23));
            Assert.False(table4.TryGetValue("a1", "b1", "c2", "d1", out var value24));
            Assert.False(table4.TryGetValue("a5", "b1", "c1", "d1", out var value25));
            Assert.False(table4.TryGetValue("a2", "b3", "c1", "d2", out var value26));

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

            Assert.True(table3.TryGetValue("a1", "b1", "c1", out var value27));
            Assert.Equal(1, value27);
            Assert.True(table3.TryGetValue("a1", "b2", "c1", out var value28));
            Assert.Equal(2, value28);
            Assert.True(table3.TryGetValue("a1", "b3", "c2", out var value29));
            Assert.Equal(3, value29);
            Assert.True(table3.TryGetValue("a2", "b1", "c2", out var value30));
            Assert.Equal(4, value30);
            Assert.True(table3.TryGetValue("a2", "b2", "c3", out var value31));
            Assert.Equal(5, value31);
            Assert.True(table3.TryGetValue("a2", "b3", "c3", out var value32));
            Assert.Equal(6, value32);
            Assert.True(table3.TryGetValue("a3", "b1", "c1", out var value33));
            Assert.Equal(7, value33);
            Assert.True(table3.TryGetValue("a3", "b2", "c2", out var value34));
            Assert.Equal(8, value34);
            Assert.True(table3.TryGetValue("a3", "b3", "c3", out var value35));
            Assert.Equal(9, value35);
            Assert.False(table3.TryGetValue("a1", "b1", "c2", out var value36));
            Assert.False(table3.TryGetValue("a5", "b1", "c1", out var value37));
            Assert.False(table3.TryGetValue("a2", "b3", "c1", out var value38));

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

            Assert.True(table2.TryGetValue("a1", "b1", out var value39));
            Assert.Equal(1, value39);
            Assert.True(table2.TryGetValue("a1", "b2", out var value40));
            Assert.Equal(2, value40);
            Assert.True(table2.TryGetValue("a1", "b3", out var value41));
            Assert.Equal(3, value41);
            Assert.True(table2.TryGetValue("a2", "b1", out var value42));
            Assert.Equal(4, value42);
            Assert.True(table2.TryGetValue("a2", "b2", out var value43));
            Assert.Equal(5, value43);
            Assert.True(table2.TryGetValue("a2", "b3", out var value44));
            Assert.Equal(6, value44);
            Assert.True(table2.TryGetValue("a3", "b1", out var value45));
            Assert.Equal(7, value45);
            Assert.True(table2.TryGetValue("a3", "b2", out var value46));
            Assert.Equal(8, value46);
            Assert.True(table2.TryGetValue("a3", "b3", out var value47));
            Assert.Equal(9, value47);
            Assert.False(table2.TryGetValue("a1", "b4", out var value48));
            Assert.False(table2.TryGetValue("a4", "b1", out var value49));
            Assert.False(table2.TryGetValue("a2", "b4", out var value50));

            var table1 = new IndexedTable<string, int>();
            table1["a1"] = 1;
            table1["a2"] = 2;
            table1["a3"] = 3;

            Assert.True(table1.TryGetValue("a1", out var value51));
            Assert.Equal(1, value51);
            Assert.True(table1.TryGetValue("a2", out var value52));
            Assert.Equal(2, value52);
            Assert.True(table1.TryGetValue("a3", out var value53));
            Assert.Equal(3, value53);
            Assert.False(table1.TryGetValue("a4", out var value54));
        }

        [Fact]
        public void TestTryGetValueWithSlice()
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

            var slice4 = table5.Slice1("a1");
            Assert.True(slice4.TryGetValue("b1", "c1", "d1", "e1", out var value1));
            Assert.Equal(1, value1);
            Assert.True(slice4.TryGetValue("b2", "c1", "d2", "e2", out var value2));
            Assert.Equal(2, value2);
            Assert.True(slice4.TryGetValue("b3", "c2", "d1", "e3", out var value3));
            Assert.Equal(3, value3);
            Assert.False(slice4.TryGetValue("b1", "c2", "d2", "e4", out var value4));
            Assert.False(slice4.TryGetValue("b2", "c3", "d1", "e5", out var value5));
            Assert.False(slice4.TryGetValue("b3", "c3", "d2", "e6", out var value6));
            Assert.False(slice4.TryGetValue("b1", "c1", "d1", "e7", out var value7));
            Assert.False(slice4.TryGetValue("b2", "c2", "d2", "e8", out var value8));
            Assert.False(slice4.TryGetValue("b3", "c3", "d1", "e9", out var value9));
            Assert.False(slice4.TryGetValue("b4", "c1", "d2", "e8", out var value10));

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

            var slice3 = table4.Slice1("a1");
            Assert.True(slice3.TryGetValue("b1", "c1", "d1", out var value11));
            Assert.Equal(1, value11);
            Assert.True(slice3.TryGetValue("b2", "c1", "d2", out var value12));
            Assert.Equal(2, value12);
            Assert.True(slice3.TryGetValue("b3", "c2", "d1", out var value13));
            Assert.Equal(3, value13);
            Assert.False(slice3.TryGetValue("b1", "c2", "d2", out var value14));
            Assert.False(slice3.TryGetValue("b2", "c3", "d1", out var value15));
            Assert.False(slice3.TryGetValue("b3", "c3", "d2", out var value16));
            Assert.False(slice3.TryGetValue("b2", "c2", "d2", out var value18));
            Assert.False(slice3.TryGetValue("b3", "c3", "d1", out var value19));
            Assert.False(slice3.TryGetValue("b4", "c1", "d2", out var value20));

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

            var slice2 = table3.Slice1("a1");
            Assert.True(slice2.TryGetValue("b1", "c1", out var value21));
            Assert.Equal(1, value21);
            Assert.True(slice2.TryGetValue("b2", "c1", out var value22));
            Assert.Equal(2, value22);
            Assert.True(slice2.TryGetValue("b3", "c2", out var value23));
            Assert.Equal(3, value23);
            Assert.False(slice2.TryGetValue("b1", "c2", out var value24));
            Assert.False(slice2.TryGetValue("b2", "c3", out var value25));
            Assert.False(slice2.TryGetValue("b3", "c3", out var value26));
            Assert.False(slice2.TryGetValue("b2", "c2", out var value28));
            Assert.False(slice2.TryGetValue("b4", "c1", out var value29));

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

            var slice1 = table2.Slice1("a1");
            Assert.True(slice1.TryGetValue("b1", out var value30));
            Assert.Equal(1, value30);
            Assert.True(slice1.TryGetValue("b2", out var value31));
            Assert.Equal(2, value31);
            Assert.True(slice1.TryGetValue("b3", out var value32));
            Assert.Equal(3, value32);
            Assert.False(slice1.TryGetValue("b4", out var value33));
        }
    }
}