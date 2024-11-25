using System;
using System.Collections.Generic;
using System.IO;
using SabreTools.Matching.Content;
using Xunit;

namespace SabreTools.Matching.Test.Content
{
    public class ContentMatchSetTests
    {
        [Fact]
        public void InvalidNeedleThrowsException()
        {
            Assert.Throws<InvalidDataException>(() => new ContentMatchSet(Array.Empty<byte>(), "name"));
            Assert.Throws<InvalidDataException>(() => new ContentMatchSet(Array.Empty<byte>(), ArrayVersionMock, "name"));
            Assert.Throws<InvalidDataException>(() => new ContentMatchSet(Array.Empty<byte>(), StreamVersionMock, "name"));
        }

        [Fact]
        public void InvalidNeedlesThrowsException()
        {
            Assert.Throws<InvalidDataException>(() => new ContentMatchSet([], "name"));
            Assert.Throws<InvalidDataException>(() => new ContentMatchSet([], ArrayVersionMock, "name"));
            Assert.Throws<InvalidDataException>(() => new ContentMatchSet([], StreamVersionMock, "name"));
        }

        [Fact]
        public void GenericConstructorSetsNoDelegates()
        {
            var needles = new List<ContentMatch> { new byte[] { 0x01, 0x02, 0x03, 0x04 } };
            var cms = new ContentMatchSet(needles, "name");
            Assert.Null(cms.GetArrayVersion);
            Assert.Null(cms.GetStreamVersion);
        }

        [Fact]
        public void ArrayConstructorSetsOneDelegate()
        {
            var needles = new List<ContentMatch> { new byte[] { 0x01, 0x02, 0x03, 0x04 } };
            var cms = new ContentMatchSet(needles, ArrayVersionMock, "name");
            Assert.NotNull(cms.GetArrayVersion);
            Assert.Null(cms.GetStreamVersion);
        }

        [Fact]
        public void StreamConstructorSetsOneDelegate()
        {
            var needles = new List<ContentMatch> { new byte[] { 0x01, 0x02, 0x03, 0x04 } };
            var cms = new ContentMatchSet(needles, StreamVersionMock, "name");
            Assert.Null(cms.GetArrayVersion);
            Assert.NotNull(cms.GetStreamVersion);
        }

        #region Array

        [Fact]
        public void MatchesAllNullArrayReturnsNoMatches()
        {
            var cms = new ContentMatchSet(new byte[] { 0x01, 0x02, 0x03, 0x04 }, "name");
            var actual = cms.MatchesAll((byte[]?)null);
            Assert.Empty(actual);
        }

        [Fact]
        public void MatchesAllEmptyArrayReturnsNoMatches()
        {
            var cms = new ContentMatchSet(new byte[] { 0x01, 0x02, 0x03, 0x04 }, "name");
            var actual = cms.MatchesAll([]);
            Assert.Empty(actual);
        }

        [Fact]
        public void MatchesAllMatchingArrayReturnsMatches()
        {
            var cms = new ContentMatchSet(new byte[] { 0x01, 0x02, 0x03, 0x04 }, "name");
            var actual = cms.MatchesAll([0x01, 0x02, 0x03, 0x04]);
            int position = Assert.Single(actual);
            Assert.Equal(0, position);
        }

        [Fact]
        public void MatchesAllMismatchedArrayReturnsNoMatches()
        {
            var cms = new ContentMatchSet(new byte[] { 0x01, 0x02, 0x03, 0x04 }, "name");
            var actual = cms.MatchesAll([0x01, 0x03]);
            Assert.Empty(actual);
        }

        [Fact]
        public void MatchesAnyNullArrayReturnsNoMatches()
        {
            var cms = new ContentMatchSet(new byte[] { 0x01, 0x02, 0x03, 0x04 }, "name");
            int actual = cms.MatchesAny((byte[]?)null);
            Assert.Equal(-1, actual);
        }

        [Fact]
        public void MatchesAnyEmptyArrayReturnsNoMatches()
        {
            var cms = new ContentMatchSet(new byte[] { 0x01, 0x02, 0x03, 0x04 }, "name");
            int actual = cms.MatchesAny([]);
            Assert.Equal(-1, actual);
        }

        [Fact]
        public void MatchesAnyMatchingArrayReturnsMatches()
        {
            var cms = new ContentMatchSet(new byte[] { 0x01, 0x02, 0x03, 0x04 }, "name");
            int actual = cms.MatchesAny([0x01, 0x02, 0x03, 0x04]);
            Assert.Equal(0, actual);
        }

        [Fact]
        public void MatchesAnyMismatchedArrayReturnsNoMatches()
        {
            var cms = new ContentMatchSet(new byte[] { 0x01, 0x02, 0x03, 0x04 }, "name");
            int actual = cms.MatchesAny([0x01, 0x03]);
            Assert.Equal(-1, actual);
        }

        #endregion

        #region Stream

        [Fact]
        public void MatchesAllNullStreamReturnsNoMatches()
        {
            var cms = new ContentMatchSet(new byte[] { 0x01, 0x02, 0x03, 0x04 }, "name");
            var actual = cms.MatchesAll((Stream?)null);
            Assert.Empty(actual);
        }

        [Fact]
        public void MatchesAllEmptyStreamReturnsNoMatches()
        {
            var cms = new ContentMatchSet(new byte[] { 0x01, 0x02, 0x03, 0x04 }, "name");
            var actual = cms.MatchesAll(new MemoryStream());
            Assert.Empty(actual);
        }

        [Fact]
        public void MatchesAllMatchingStreamReturnsMatches()
        {
            var cms = new ContentMatchSet(new byte[] { 0x01, 0x02, 0x03, 0x04 }, "name");
            var actual = cms.MatchesAll(new MemoryStream([0x01, 0x02, 0x03, 0x04]));
            int position = Assert.Single(actual);
            Assert.Equal(0, position);
        }

        [Fact]
        public void MatchesAllMismatchedStreamReturnsNoMatches()
        {
            var cms = new ContentMatchSet(new byte[] { 0x01, 0x02, 0x03, 0x04 }, "name");
            var actual = cms.MatchesAll([0x01, 0x03]);
            Assert.Empty(actual);
        }

        [Fact]
        public void MatchesAnyNullStreamReturnsNoMatches()
        {
            var cms = new ContentMatchSet(new byte[] { 0x01, 0x02, 0x03, 0x04 }, "name");
            int actual = cms.MatchesAny((Stream?)null);
            Assert.Equal(-1, actual);
        }

        [Fact]
        public void MatchesAnyEmptyStreamReturnsNoMatches()
        {
            var cms = new ContentMatchSet(new byte[] { 0x01, 0x02, 0x03, 0x04 }, "name");
            int actual = cms.MatchesAny(new MemoryStream());
            Assert.Equal(-1, actual);
        }

        [Fact]
        public void MatchesAnyMatchingStreamReturnsMatches()
        {
            var cms = new ContentMatchSet(new byte[] { 0x01, 0x02, 0x03, 0x04 }, "name");
            int actual = cms.MatchesAny(new MemoryStream([0x01, 0x02, 0x03, 0x04]));
            Assert.Equal(0, actual);
        }

        [Fact]
        public void MatchesAnyMismatchedStreamReturnsNoMatches()
        {
            var cms = new ContentMatchSet(new byte[] { 0x01, 0x02, 0x03, 0x04 }, "name");
            int actual = cms.MatchesAny([0x01, 0x03]);
            Assert.Equal(-1, actual);
        }

        #endregion

        #region Mock Delegates

        /// <inheritdoc cref="GetArrayVersion"/>
        private static string? ArrayVersionMock(string path, byte[]? content, List<int> positions) => null;

        /// <inheritdoc cref="GetStreamVersion"/>
        private static string? StreamVersionMock(string path, Stream? content, List<int> positions) => null;

        #endregion
    }
}