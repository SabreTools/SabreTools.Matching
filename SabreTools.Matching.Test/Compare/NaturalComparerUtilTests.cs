using SabreTools.Matching.Compare;
using Xunit;

namespace SabreTools.Matching.Test.Compare
{
    public class NaturalComparerUtilTests
    {
        [Fact]
        public void CompareNumericBothNullTest()
        {
            int actual = NaturalComparerUtil.CompareNumeric(null, null);
            Assert.Equal(0, actual);
        }
        
        [Fact]
        public void CompareNumericSingleNullTest()
        {
            int actual = NaturalComparerUtil.CompareNumeric(null, "notnull");
            Assert.Equal(-1, actual);

            actual = NaturalComparerUtil.CompareNumeric("notnull", null);
            Assert.Equal(1, actual);
        }

        [Fact]
        public void CompareNumericBothEqualTest()
        {
            int actual = NaturalComparerUtil.CompareNumeric("notnull", "notnull");
            Assert.Equal(0, actual);
        }

        [Fact]
        public void CompareNumericBothEqualWithPathTest()
        {
            int actual = NaturalComparerUtil.CompareNumeric("notnull/file.ext", "notnull/file.ext");
            Assert.Equal(0, actual);
        }

        [Fact]
        public void CompareNumericNumericNonDecimalStringTest()
        {
            int actual = NaturalComparerUtil.CompareNumeric("100", "10");
            Assert.Equal(1, actual);

            actual = NaturalComparerUtil.CompareNumeric("10", "100");
            Assert.Equal(-1, actual);
        }

        [Fact]
        public void CompareNumericNumericDecimalStringTest()
        {
            int actual = NaturalComparerUtil.CompareNumeric("100.100", "100.10");
            Assert.Equal(1, actual);

            actual = NaturalComparerUtil.CompareNumeric("100.10", "100.100");
            Assert.Equal(-1, actual);
        }
    }
}