using System;
using System.IO;
using SabreTools.Matching.Content;
using Xunit;

namespace SabreTools.Matching.Test.Content
{
    public class ContentMatchTests
    {
        [Fact]
        public void InvalidNeedleThrowsException()
        {
            Assert.Throws<InvalidDataException>(() => new ContentMatch(Array.Empty<byte>()));
            Assert.Throws<InvalidDataException>(() => new ContentMatch(Array.Empty<byte?>()));
        }

        [Fact]
        public void InvalidStartThrowsException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new ContentMatch(new byte[1], start: -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => new ContentMatch(new byte?[1], start: -1));
        }

        [Fact]
        public void InvalidEndThrowsException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new ContentMatch(new byte[1], end: -2));
            Assert.Throws<ArgumentOutOfRangeException>(() => new ContentMatch(new byte?[1], end: -2));
        }

        [Fact]
        public void ImplicitOperatorArrayReturnsSuccess()
        {
            byte[] needle = [0x01, 0x02, 0x03, 0x04];
            var cm = (ContentMatch)needle;
            Assert.NotNull(cm);
        }

        [Fact]
        public void ImplicitOperatorNullableArrayReturnsSuccess()
        {
            byte?[] needle = [0x01, 0x02, 0x03, 0x04];
            var cm = (ContentMatch)needle;
            Assert.NotNull(cm);
        }

        #region Byte Array

        [Fact]
        public void NullArrayReturnsNoMatch()
        {
            var cm = new ContentMatch(new byte?[1]);
            int actual = cm.Match((byte[]?)null);
            Assert.Equal(-1, actual);
        }

        [Fact]
        public void EmptyArrayReturnsNoMatch()
        {
            var cm = new ContentMatch(new byte?[1]);
            int actual = cm.Match([]);
            Assert.Equal(-1, actual);
        }

        [Fact]
        public void LargerNeedleArrayReturnsNoMatch()
        {
            var cm = new ContentMatch(new byte?[2]);
            int actual = cm.Match(new byte[1]);
            Assert.Equal(-1, actual);
        }

        [Fact]
        public void EqualLengthMatchingArrayReturnsMatch()
        {
            byte[] needle = [0x01, 0x02, 0x03, 0x04];
            var cm = new ContentMatch(needle);

            int actual = cm.Match(needle);
            Assert.Equal(0, actual);
        }

        [Fact]
        public void EqualLengthMatchingArrayReverseReturnsMatch()
        {
            byte[] needle = [0x01, 0x02, 0x03, 0x04];
            var cm = new ContentMatch(needle);

            int actual = cm.Match(needle, reverse: true);
            Assert.Equal(0, actual);
        }

        [Fact]
        public void EqualLengthMismatchedArrayReturnsNoMatch()
        {
            byte[] needle = [0x01, 0x02, 0x03, 0x04];
            var cm = new ContentMatch(needle);

            int actual = cm.Match(new byte[4]);
            Assert.Equal(-1, actual);
        }

        [Fact]
        public void EqualLengthMismatchedArrayReverseReturnsNoMatch()
        {
            byte[] needle = [0x01, 0x02, 0x03, 0x04];
            var cm = new ContentMatch(needle);

            int actual = cm.Match(new byte[4], reverse: true);
            Assert.Equal(-1, actual);
        }

        [Fact]
        public void InequalLengthMatchingArrayReturnsMatch()
        {
            byte[] stack = [0x01, 0x02, 0x03, 0x04];
            byte[] needle = [0x02, 0x03];
            var cm = new ContentMatch(needle);

            int actual = cm.Match(stack);
            Assert.Equal(1, actual);
        }

        [Fact]
        public void InequalLengthMatchingArrayReverseReturnsMatch()
        {
            byte[] stack = [0x01, 0x02, 0x03, 0x04];
            byte[] needle = [0x02, 0x03];
            var cm = new ContentMatch(needle);

            int actual = cm.Match(stack, reverse: true);
            Assert.Equal(1, actual);
        }

        [Fact]
        public void InequalLengthMismatchedArrayReturnsNoMatch()
        {
            byte[] stack = [0x01, 0x02, 0x03, 0x04];
            byte[] needle = [0x02, 0x04];
            var cm = new ContentMatch(needle);

            int actual = cm.Match(stack);
            Assert.Equal(-1, actual);
        }

        [Fact]
        public void InequalLengthMismatchedArrayReverseReturnsNoMatch()
        {
            byte[] stack = [0x01, 0x02, 0x03, 0x04];
            byte[] needle = [0x02, 0x04];
            var cm = new ContentMatch(needle);

            int actual = cm.Match(stack, reverse: true);
            Assert.Equal(-1, actual);
        }

        #endregion

        #region Stream

        [Fact]
        public void NullStreamReturnsNoMatch()
        {
            var cm = new ContentMatch(new byte?[1]);
            int actual = cm.Match((Stream?)null);
            Assert.Equal(-1, actual);
        }

        [Fact]
        public void EmptyStreamReturnsNoMatch()
        {
            var cm = new ContentMatch(new byte?[1]);
            int actual = cm.Match(new MemoryStream());
            Assert.Equal(-1, actual);
        }

        [Fact]
        public void LargerNeedleStreamReturnsNoMatch()
        {
            var cm = new ContentMatch(new byte?[2]);
            int actual = cm.Match(new MemoryStream(new byte[1]));
            Assert.Equal(-1, actual);
        }

        [Fact]
        public void EqualLengthMatchingStreamReturnsMatch()
        {
            byte[] needle = [0x01, 0x02, 0x03, 0x04];
            var cm = new ContentMatch(needle);

            int actual = cm.Match(new MemoryStream(needle));
            Assert.Equal(0, actual);
        }

        [Fact]
        public void EqualLengthMatchingStreamReverseReturnsMatch()
        {
            byte[] needle = [0x01, 0x02, 0x03, 0x04];
            var cm = new ContentMatch(needle);

            int actual = cm.Match(new MemoryStream(needle), reverse: true);
            Assert.Equal(0, actual);
        }

        [Fact]
        public void EqualLengthMismatchedStreamReturnsNoMatch()
        {
            byte[] needle = [0x01, 0x02, 0x03, 0x04];
            var cm = new ContentMatch(needle);

            int actual = cm.Match(new MemoryStream(new byte[4]));
            Assert.Equal(-1, actual);
        }

        [Fact]
        public void EqualLengthMismatchedStreamReverseReturnsNoMatch()
        {
            byte[] needle = [0x01, 0x02, 0x03, 0x04];
            var cm = new ContentMatch(needle);

            int actual = cm.Match(new MemoryStream(new byte[4]), reverse: true);
            Assert.Equal(-1, actual);
        }

        [Fact]
        public void InequalLengthMatchingStreamReturnsMatch()
        {
            Stream stack = new MemoryStream([0x01, 0x02, 0x03, 0x04]);
            byte[] needle = [0x02, 0x03];
            var cm = new ContentMatch(needle);

            int actual = cm.Match(stack);
            Assert.Equal(1, actual);
        }

        [Fact]
        public void InequalLengthMatchingStreamReverseReturnsMatch()
        {
            Stream stack = new MemoryStream([0x01, 0x02, 0x03, 0x04]);
            byte[] needle = [0x02, 0x03];
            var cm = new ContentMatch(needle);

            int actual = cm.Match(stack, reverse: true);
            Assert.Equal(1, actual);
        }

        [Fact]
        public void InequalLengthMismatchedStreamReturnsNoMatch()
        {
            Stream stack = new MemoryStream([0x01, 0x02, 0x03, 0x04]);
            byte[] needle = [0x02, 0x04];
            var cm = new ContentMatch(needle);

            int actual = cm.Match(stack);
            Assert.Equal(-1, actual);
        }

        [Fact]
        public void InequalLengthMismatchedStreamReverseReturnsNoMatch()
        {
            Stream stack = new MemoryStream([0x01, 0x02, 0x03, 0x04]);
            byte[] needle = [0x02, 0x04];
            var cm = new ContentMatch(needle);

            int actual = cm.Match(stack, reverse: true);
            Assert.Equal(-1, actual);
        }

        #endregion
    }
}