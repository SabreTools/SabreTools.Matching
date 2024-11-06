using System;
using System.Collections.Generic;
using SabreTools.Matching.Content;

namespace SabreTools.Matching
{
    public static class Extensions
    {
        /// <summary>
        /// Indicates whether the specified array is null or has a length of zero
        /// </summary>
#if NET20
        public static bool IsNullOrEmpty(Array? array)
#else
        public static bool IsNullOrEmpty(this Array? array)
#endif
        {
            return array == null || array.Length == 0;
        }

        /// <summary>
        /// Find all positions of one array in another, if possible, if possible
        /// </summary>
#if NET20
        public static List<int> FindAllPositions(byte[] stack, byte?[]? needle, int start = 0, int end = -1)
#else
        public static List<int> FindAllPositions(this byte[] stack, byte?[]? needle, int start = 0, int end = -1)
#endif
        {
            // Get the outgoing list
            List<int> positions = [];

            // Initialize the loop variables
            int lastPosition = start;
            var matcher = new ContentMatch(needle, end: end);

            // Loop over and get all positions
            while (true)
            {
                matcher.Start = lastPosition;
                lastPosition = matcher.Match(stack, false);
                if (lastPosition < 0)
                    break;

                positions.Add(lastPosition);
            }

            return positions;
        }

        /// <summary>
        /// Find the first position of one array in another, if possible
        /// </summary>
#if NET20
        public static bool FirstPosition(byte[] stack, byte[]? needle, out int position, int start = 0, int end = -1)
#else
        public static bool FirstPosition(this byte[] stack, byte[]? needle, out int position, int start = 0, int end = -1)
#endif
        {
            // Convert the needle to a nullable byte array
            byte?[]? nullableNeedle = null;
            if (needle != null)
                nullableNeedle = Array.ConvertAll(needle, b => (byte?)b);

            return FirstPosition(stack, nullableNeedle, out position, start, end);
        }

        /// <summary>
        /// Find the first position of one array in another, if possible
        /// </summary>
#if NET20
        public static bool FirstPosition(byte[] stack, byte?[]? needle, out int position, int start = 0, int end = -1)
#else
        public static bool FirstPosition(this byte[] stack, byte?[]? needle, out int position, int start = 0, int end = -1)
#endif
        {
            var matcher = new ContentMatch(needle, start, end);
            position = matcher.Match(stack, false);
            return position >= 0;
        }

        /// <summary>
        /// Find the last position of one array in another, if possible
        /// </summary>
#if NET20
        public static bool LastPosition(byte[] stack, byte[]? needle, out int position, int start = 0, int end = -1)
#else
        public static bool LastPosition(this byte[] stack, byte[]? needle, out int position, int start = 0, int end = -1)
#endif
        {
            // Convert the needle to a nullable byte array
            byte?[]? nullableNeedle = null;
            if (needle != null)
                nullableNeedle = Array.ConvertAll(needle, b => (byte?)b);

            return LastPosition(stack, nullableNeedle, out position, start, end);
        }

        /// <summary>
        /// Find the last position of one array in another, if possible
        /// </summary>
#if NET20
        public static bool LastPosition(byte[] stack, byte?[]? needle, out int position, int start = 0, int end = -1)
#else
        public static bool LastPosition(this byte[] stack, byte?[]? needle, out int position, int start = 0, int end = -1)
#endif
        {
            var matcher = new ContentMatch(needle, start, end);
            position = matcher.Match(stack, true);
            return position >= 0;
        }

        /// <summary>
        /// See if a byte array starts with another
        /// </summary>
#if NET20
        public static bool StartsWith(byte[] stack, byte[]? needle, bool exact = false)
#else
        public static bool StartsWith(this byte[] stack, byte[]? needle, bool exact = false)
#endif
        {
            // If we have any invalid inputs, we return false
            if (needle == null
                || stack.Length == 0 || needle.Length == 0
                || needle.Length > stack.Length
                || (exact && stack.Length != needle.Length))
            {
                return false;
            }

            return FirstPosition(stack, needle, out int _, start: 0, end: 1);
        }

        /// <summary>
        /// See if a byte array starts with another
        /// </summary>
#if NET20
        public static bool StartsWith(byte[] stack, byte?[]? needle, bool exact = false)
#else
        public static bool StartsWith(this byte[] stack, byte?[]? needle, bool exact = false)
#endif
        {
            // If we have any invalid inputs, we return false
            if (needle == null
                || stack.Length == 0 || needle.Length == 0
                || needle.Length > stack.Length
                || (exact && stack.Length != needle.Length))
            {
                return false;
            }

            return FirstPosition(stack, needle, out int _, start: 0, end: 1);
        }

        /// <summary>
        /// See if a byte array ends with another
        /// </summary>
#if NET20
        public static bool EndsWith(byte[] stack, byte[]? needle, bool exact = false)
#else
        public static bool EndsWith(this byte[] stack, byte[]? needle, bool exact = false)
#endif
        {
            // If we have any invalid inputs, we return false
            if (needle == null
                || stack.Length == 0 || needle.Length == 0
                || needle.Length > stack.Length
                || (exact && stack.Length != needle.Length))
            {
                return false;
            }

            return FirstPosition(stack, needle, out int _, start: stack.Length - needle.Length);
        }

        /// <summary>
        /// See if a byte array ends with another
        /// </summary>
#if NET20
        public static bool EndsWith(byte[] stack, byte?[]? needle, bool exact = false)
#else
        public static bool EndsWith(this byte[] stack, byte?[]? needle, bool exact = false)
#endif
        {
            // If we have any invalid inputs, we return false
            if (needle == null
                || stack.Length == 0 || needle.Length == 0
                || needle.Length > stack.Length
                || (exact && stack.Length != needle.Length))
            {
                return false;
            }

            return FirstPosition(stack, needle, out int _, start: stack.Length - needle.Length);
        }
    }
}