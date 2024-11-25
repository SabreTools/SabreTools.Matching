using System;
using System.Collections.Generic;
using System.IO;
using SabreTools.Matching.Paths;
using Xunit;

namespace SabreTools.Matching.Test.Paths
{
    public class PathMatchSetTests
    {
        [Fact]
        public void InvalidNeedleThrowsException()
        {
            Assert.Throws<InvalidDataException>(() => new PathMatchSet(string.Empty, "name"));
            Assert.Throws<InvalidDataException>(() => new PathMatchSet(string.Empty, PathVersionMock, "name"));
        }

        [Fact]
        public void InvalidNeedlesThrowsException()
        {
            Assert.Throws<InvalidDataException>(() => new PathMatchSet([], "name"));
            Assert.Throws<InvalidDataException>(() => new PathMatchSet([], PathVersionMock, "name"));
        }

        [Fact]
        public void GenericConstructorSetsNoDelegates()
        {
            var needles = new List<PathMatch> { "test" };
            var cms = new PathMatchSet(needles, "name");
            Assert.Null(cms.GetVersion);
        }

        [Fact]
        public void VersionConstructorSetsDelegate()
        {
            var needles = new List<PathMatch> { "test" };
            var cms = new PathMatchSet(needles, PathVersionMock, "name");
            Assert.NotNull(cms.GetVersion);
        }

        #region Array / List

        [Fact]
        public void MatchesAllNullArrayReturnsNoMatches()
        {
            var cms = new PathMatchSet("test", "name");
            var actual = cms.MatchesAll((string[]?)null);
            Assert.Empty(actual);
        }

        [Fact]
        public void MatchesAllEmptyArrayReturnsNoMatches()
        {
            var cms = new PathMatchSet("test", "name");
            var actual = cms.MatchesAll(Array.Empty<string>());
            Assert.Empty(actual);
        }

        [Fact]
        public void MatchesAllMatchingArrayReturnsMatches()
        {
            var cms = new PathMatchSet("test", "name");
            var actual = cms.MatchesAll(new string[] { "test" });
            string path = Assert.Single(actual);
            Assert.Equal("test", path);
        }

        [Fact]
        public void MatchesAllMismatchedArrayReturnsNoMatches()
        {
            var cms = new PathMatchSet("test", "name");
            var actual = cms.MatchesAll(new string[] { "not" });
            Assert.Empty(actual);
        }

        [Fact]
        public void MatchesAnyNullArrayReturnsNoMatches()
        {
            var cms = new PathMatchSet("test", "name");
            string? actual = cms.MatchesAny((string[]?)null);
            Assert.Null(actual);
        }

        [Fact]
        public void MatchesAnyEmptyArrayReturnsNoMatches()
        {
            var cms = new PathMatchSet("test", "name");
            string? actual = cms.MatchesAny(Array.Empty<string>());
            Assert.Null(actual);
        }

        [Fact]
        public void MatchesAnyMatchingArrayReturnsMatches()
        {
            var cms = new PathMatchSet("test", "name");
            string? actual = cms.MatchesAny(new string[] { "test" });
            Assert.Equal("test", actual);
        }

        [Fact]
        public void MatchesAnyMismatchedArrayReturnsNoMatches()
        {
            var cms = new PathMatchSet("test", "name");
            string? actual = cms.MatchesAny(new string[] { "not" });
            Assert.Null(actual);
        }

        #endregion

        #region Mock Delegates

        /// <inheritdoc cref="GetPathVersion"/>
        private static string? PathVersionMock(string path, List<string>? files) => null;

        #endregion
    }
}