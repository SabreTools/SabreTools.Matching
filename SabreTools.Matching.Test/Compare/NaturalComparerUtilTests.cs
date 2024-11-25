using SabreTools.Matching.Compare;
using Xunit;

namespace SabreTools.Matching.Test.Compare
{
    public class NaturalComparerUtilTests
    {
        [Fact]
        public void CompareNumericBothNullTest()
        {
            int actual = NaturalComparerUtil.ComparePaths(null, null);
            Assert.Equal(0, actual);
        }
        
        [Fact]
        public void CompareNumericSingleNullTest()
        {
            int actual = NaturalComparerUtil.ComparePaths(null, "notnull");
            Assert.Equal(-1, actual);

            actual = NaturalComparerUtil.ComparePaths("notnull", null);
            Assert.Equal(1, actual);
        }

        [Fact]
        public void CompareNumericBothEqualTest()
        {
            int actual = NaturalComparerUtil.ComparePaths("notnull", "notnull");
            Assert.Equal(0, actual);
        }

        [Fact]
        public void CompareNumericBothEqualWithPathTest()
        {
            int actual = NaturalComparerUtil.ComparePaths("notnull/file.ext", "notnull/file.ext");
            Assert.Equal(0, actual);
        }

        [Fact]
        public void CompareNumericBothEqualWithAltPathTest()
        {
            int actual = NaturalComparerUtil.ComparePaths("notnull/file.ext", "notnull\\file.ext");
            Assert.Equal(0, actual);
        }

        [Fact]
        public void CompareNumericNumericNonDecimalStringTest()
        {
            int actual = NaturalComparerUtil.ComparePaths("100", "10");
            Assert.Equal(1, actual);

            actual = NaturalComparerUtil.ComparePaths("10", "100");
            Assert.Equal(-1, actual);
        }

        [Fact]
        public void CompareNumericNumericDecimalStringTest()
        {
            int actual = NaturalComparerUtil.ComparePaths("100.100", "100.10");
            Assert.Equal(1, actual);

            actual = NaturalComparerUtil.ComparePaths("100.10", "100.100");
            Assert.Equal(-1, actual);
        }
    }
}