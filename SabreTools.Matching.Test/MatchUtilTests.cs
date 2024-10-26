using System;
using System.Collections.Generic;
using System.Linq;
using SabreTools.Matching.Content;
using Xunit;

namespace SabreTools.Matching.Test
{
    public class MatchUtilTests
    {
        [Fact]
        public void ExactSizeMatch()
        {
            byte[] source = [0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07];
            byte?[] check = [0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07];
            string expected = "match";

            var matchers = new List<ContentMatchSet>
            {
                new(check, expected),
            };
            
            string? actual = MatchUtil.GetFirstMatch("testfile", source, matchers);
            Assert.Equal(expected, actual);
        }
    }
}