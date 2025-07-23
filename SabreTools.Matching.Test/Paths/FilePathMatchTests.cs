using System.IO;
using SabreTools.Matching.Paths;
using Xunit;

namespace SabreTools.Matching.Test.Paths
{
    /// <remarks>
    /// All other test cases are covered by <see cref="PathMatchTests"/> 
    /// </remarks>
    public class FilePathMatchTests
    {
        [Fact]
        public void ConstructorFormatsNeedle()
        {
            string needle = "test";
            string expected = $"{Path.DirectorySeparatorChar}{needle}";

            var fpm = new FilePathMatch(needle);
            Assert.Equal(expected, fpm.Needle);
        }
    }
}
