using System;
using System.Linq;
using SabreTools.Matching.Compare;
using Xunit;

namespace SabreTools.Matching.Test.Compare
{
    public class NaturalComparerTests
    {
        [Fact]
        public void NaturalComparerListSortTest()
        {
            // Setup arrays
            string[] sortable = ["0", "100", "5", "2", "1000"];
            string[] expected = ["0", "2", "5", "100", "1000"];

            // Run sorting on array
            Array.Sort(sortable, new NaturalComparer());

            // Check the output
            Assert.True(sortable.SequenceEqual(expected));
        }

        [Fact]
        public void NaturalReversedComparerListSortTest()
        {
            // Setup arrays
            string[] sortable = ["0", "100", "5", "2", "1000"];
            string[] expected = ["1000", "100", "5", "2", "0"];

            // Run sorting on array
            Array.Sort(sortable, new NaturalReversedComparer());

            // Check the output
            Assert.True(sortable.SequenceEqual(expected));
        }
    }
}