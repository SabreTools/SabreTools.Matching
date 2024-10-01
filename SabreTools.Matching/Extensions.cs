using System;
using System.Collections.Generic;
using System.Linq;
using SabreTools.Matching.Content;

namespace SabreTools.Matching
{
    public static class Extensions
    {
        /// <summary>
        /// Indicates whether the specified array is null or has a length of zero
        /// </summary>
        public static bool IsNullOrEmpty(this Array? array)
        {
            return array == null || array.Length == 0;
        }

        /// <summary>
        /// Find all positions of one array in another, if possible, if possible
        /// </summary>
        public static List<int> FindAllPositions(this byte[] stack, byte?[]? needle, int start = 0, int end = -1)
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
        public static bool FirstPosition(this byte[] stack, byte[]? needle, out int position, int start = 0, int end = -1)
        {
            byte?[]? nullableNeedle = needle?.Select(b => (byte?)b).ToArray();
            return stack.FirstPosition(nullableNeedle, out position, start, end);
        }

        /// <summary>
        /// Find the first position of one array in another, if possible
        /// </summary>
        public static bool FirstPosition(this byte[] stack, byte?[]? needle, out int position, int start = 0, int end = -1)
        {
            var matcher = new ContentMatch(needle, start, end);
            position = matcher.Match(stack, false);
            return position >= 0;
        }

        /// <summary>
        /// Find the last position of one array in another, if possible
        /// </summary>
        public static bool LastPosition(this byte[] stack, byte?[]? needle, out int position, int start = 0, int end = -1)
        {
            var matcher = new ContentMatch(needle, start, end);
            position = matcher.Match(stack, true);
            return position >= 0;
        }

        /// <summary>
        /// See if a byte array starts with another
        /// </summary>
        public static bool StartsWith(this byte[] stack, byte[]? needle, bool exact = false)
        {
            // If we have any invalid inputs, we return false
            if (needle == null
                || stack.Length == 0 || needle.Length == 0
                || needle.Length > stack.Length
                || (exact && stack.Length != needle.Length))
            {
                return false;
            }

            return stack.FirstPosition(needle, out int _, start: 0, end: 1);
        }

        /// <summary>
        /// See if a byte array starts with another
        /// </summary>
        public static bool StartsWith(this byte[] stack, byte?[]? needle, bool exact = false)
        {
            // If we have any invalid inputs, we return false
            if (needle == null
                || stack.Length == 0 || needle.Length == 0
                || needle.Length > stack.Length
                || (exact && stack.Length != needle.Length))
            {
                return false;
            }

            return stack.FirstPosition(needle, out int _, start: 0, end: 1);
        }

        /// <summary>
        /// See if a byte array ends with another
        /// </summary>
        public static bool EndsWith(this byte[] stack, byte[]? needle, bool exact = false)
        {
            // If we have any invalid inputs, we return false
            if (needle == null
                || stack.Length == 0 || needle.Length == 0
                || needle.Length > stack.Length
                || (exact && stack.Length != needle.Length))
            {
                return false;
            }

            return stack.FirstPosition(needle, out int _, start: stack.Length - needle.Length);
        }

        /// <summary>
        /// See if a byte array ends with another
        /// </summary>
        public static bool EndsWith(this byte[] stack, byte?[]? needle, bool exact = false)
        {
            // If we have any invalid inputs, we return false
            if (needle == null
                || stack.Length == 0 || needle.Length == 0
                || needle.Length > stack.Length
                || (exact && stack.Length != needle.Length))
            {
                return false;
            }

            return stack.FirstPosition(needle, out int _, start: stack.Length - needle.Length);
        }
    }
}