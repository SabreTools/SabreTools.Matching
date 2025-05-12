using System;
using System.Linq;
using SabreTools.Matching.Compare;
using Xunit;

namespace SabreTools.Matching.Test.Compare
{
    public class NaturalComparerTests
    {
        [Fact]
        public void NaturalComparer_ListSort_Numeric()
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
        public void NaturalComparer_ListSort_Mixed()
        {
            // Setup arrays
            string[] sortable = ["b3b", "c", "b", "a", "a1"];
            string[] expected = ["a", "a1", "b", "b3b", "c"];

            // Run sorting on array
            Array.Sort(sortable, new NaturalComparer());

            // Check the output
            Assert.True(sortable.SequenceEqual(expected));
        }

        [Fact]
        public void NaturalReversedComparer_ListSort_Numeric()
        {
            // Setup arrays
            string[] sortable = ["0", "100", "5", "2", "1000"];
            string[] expected = ["1000", "100", "5", "2", "0"];

            // Run sorting on array
            Array.Sort(sortable, new NaturalReversedComparer());

            // Check the output
            Assert.True(sortable.SequenceEqual(expected));
        }

        [Fact]
        public void NaturalReversedComparer_ListSort_Mixed()
        {
            // Setup arrays
            string[] sortable = ["b3b", "c", "b", "a", "a1"];
            string[] expected = ["c", "b3b", "b", "a1", "a"];

            // Run sorting on array
            Array.Sort(sortable, new NaturalReversedComparer());

            // Check the output
            Assert.True(sortable.SequenceEqual(expected));
        }
    }
}