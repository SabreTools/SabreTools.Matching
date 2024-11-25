using System;
using System.Collections.Generic;
using System.IO;
using SabreTools.Matching.Paths;
using Xunit;

namespace SabreTools.Matching.Test.Paths
{
    public class PathMatchTests
    {
        [Fact]
        public void InvalidNeedleThrowsException()
        {
            Assert.Throws<InvalidDataException>(() => new PathMatch(string.Empty));
        }

        #region Array

        [Fact]
        public void NullArrayReturnsNoMatch()
        {
            var pm = new PathMatch("test");
            string? actual = pm.Match((string[]?)null);
            Assert.Null(actual);
        }

        [Fact]
        public void EmptyArrayReturnsNoMatch()
        {
            var pm = new PathMatch("test");
            string? actual = pm.Match(Array.Empty<string>());
            Assert.Null(actual);
        }

        [Fact]
        public void SingleItemArrayMatchingReturnsMatch()
        {
            string needle = "test";
            string[] stack = [needle];
            var pm = new PathMatch(needle);

            string? actual = pm.Match(stack);
            Assert.Equal(needle, actual);
        }

        [Fact]
        public void SingleItemArrayMismatchedReturnsNoMatch()
        {
            string needle = "test";
            string[] stack = ["not"];
            var pm = new PathMatch(needle);

            string? actual = pm.Match(stack);
            Assert.Null(actual);
        }

        [Fact]
        public void MultiItemArrayMatchingReturnsMatch()
        {
            string needle = "test";
            string[] stack = ["not", needle, "far"];
            var pm = new PathMatch(needle);

            string? actual = pm.Match(stack);
            Assert.Equal(needle, actual);
        }

        [Fact]
        public void MultiItemArrayMismatchedReturnsNoMatch()
        {
            string needle = "test";
            string[] stack = ["not", "too", "far"];
            var pm = new PathMatch(needle);

            string? actual = pm.Match(stack);
            Assert.Null(actual);
        }

        #endregion

        #region List

        [Fact]
        public void NullListReturnsNoMatch()
        {
            var pm = new PathMatch("test");
            string? actual = pm.Match((List<string>?)null);
            Assert.Null(actual);
        }

        [Fact]
        public void EmptyListReturnsNoMatch()
        {
            var pm = new PathMatch("test");
            string? actual = pm.Match(new List<string>());
            Assert.Null(actual);
        }

        [Fact]
        public void SingleItemListMatchingReturnsMatch()
        {
            string needle = "test";
            List<string> stack = [needle];
            var pm = new PathMatch(needle);

            string? actual = pm.Match(stack);
            Assert.Equal(needle, actual);
        }

        [Fact]
        public void SingleItemListMismatchedReturnsNoMatch()
        {
            string needle = "test";
            List<string> stack = ["not"];
            var pm = new PathMatch(needle);

            string? actual = pm.Match(stack);
            Assert.Null(actual);
        }

        [Fact]
        public void MultiItemListMatchingReturnsMatch()
        {
            string needle = "test";
            List<string> stack = ["not", needle, "far"];
            var pm = new PathMatch(needle);

            string? actual = pm.Match(stack);
            Assert.Equal(needle, actual);
        }

        [Fact]
        public void MultiItemListMismatchedReturnsNoMatch()
        {
            string needle = "test";
            List<string> stack = ["not", "too", "far"];
            var pm = new PathMatch(needle);

            string? actual = pm.Match(stack);
            Assert.Null(actual);
        }

        #endregion

        #region Match Case

        [Fact]
        public void MatchCaseEqualReturnsMatch()
        {
            string needle = "test";
            List<string> stack = [needle];
            var pm = new PathMatch(needle, matchCase: true);

            string? actual = pm.Match(stack);
            Assert.Equal(needle, actual);
        }

        [Fact]
        public void NoMatchCaseEqualReturnsMatch()
        {
            string needle = "test";
            List<string> stack = [needle];
            var pm = new PathMatch(needle, matchCase: false);

            string? actual = pm.Match(stack);
            Assert.Equal(needle, actual);
        }

        [Fact]
        public void MatchCaseInequalReturnsNoMatch()
        {
            string needle = "test";
            List<string> stack = [needle.ToUpperInvariant()];
            var pm = new PathMatch(needle, matchCase: true);

            string? actual = pm.Match(stack);
            Assert.Null(actual);
        }

        [Fact]
        public void NoMatchCaseInequalReturnsMatch()
        {
            string needle = "test";
            List<string> stack = [needle.ToUpperInvariant()];
            var pm = new PathMatch(needle, matchCase: false);

            string? actual = pm.Match(stack);
            Assert.Equal(needle.ToUpperInvariant(), actual);
        }

        [Fact]
        public void MatchCaseContainsReturnsMatch()
        {
            string needle = "test";
            List<string> stack = [$"prefix_{needle}_postfix"];
            var pm = new PathMatch(needle, matchCase: true);

            string? actual = pm.Match(stack);
            Assert.Equal($"prefix_{needle}_postfix", actual);
        }

        [Fact]
        public void NoMatchCaseContainsReturnsMatch()
        {
            string needle = "test";
            List<string> stack = [$"prefix_{needle}_postfix"];
            var pm = new PathMatch(needle, matchCase: false);

            string? actual = pm.Match(stack);
            Assert.Equal($"prefix_{needle}_postfix", actual);
        }

        #endregion

        #region Use Ends With

        [Fact]
        public void EndsWithEqualReturnsMatch()
        {
            string needle = "test";
            List<string> stack = [needle];
            var pm = new PathMatch(needle, useEndsWith: true);

            string? actual = pm.Match(stack);
            Assert.Equal(needle, actual);
        }

        [Fact]
        public void NoEndsWithEqualReturnsMatch()
        {
            string needle = "test";
            List<string> stack = [needle];
            var pm = new PathMatch(needle, useEndsWith: false);

            string? actual = pm.Match(stack);
            Assert.Equal(needle, actual);
        }

        [Fact]
        public void EndsWithInequalReturnsMatch()
        {
            string needle = "test";
            List<string> stack = [needle.ToUpperInvariant()];
            var pm = new PathMatch(needle, useEndsWith: true);

            string? actual = pm.Match(stack);
            Assert.Equal(needle.ToUpperInvariant(), actual);
        }

        [Fact]
        public void NoEndsWithInequalReturnsMatch()
        {
            string needle = "test";
            List<string> stack = [needle.ToUpperInvariant()];
            var pm = new PathMatch(needle, useEndsWith: false);

            string? actual = pm.Match(stack);
            Assert.Equal(needle.ToUpperInvariant(), actual);
        }

        [Fact]
        public void EndsWithContainsReturnsNoMatch()
        {
            string needle = "test";
            List<string> stack = [$"prefix_{needle}_postfix"];
            var pm = new PathMatch(needle, useEndsWith: true);

            string? actual = pm.Match(stack);
            Assert.Null(actual);
        }

        [Fact]
        public void NoEndsWithContainsReturnsMatch()
        {
            string needle = "test";
            List<string> stack = [$"prefix_{needle}_postfix"];
            var pm = new PathMatch(needle, useEndsWith: false);

            string? actual = pm.Match(stack);
            Assert.Equal($"prefix_{needle}_postfix", actual);
        }

        #endregion
    }
}