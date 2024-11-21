using System.Collections.Generic;
using System.IO;

namespace SabreTools.Matching.Content
{
    /// <summary>
    /// A set of content matches that work together
    /// </summary>
    public class ContentMatchSet : MatchSet<ContentMatch, byte?[]>
    {
        /// <summary>
        /// Function to get a content version
        /// </summary>
        /// <remarks>
        /// A content version method takes the file path, the file contents,
        /// and a list of found positions and returns a single string. That
        /// string is either a version string, in which case it will be appended
        /// to the match name, or `null`, in which case it will cause
        /// the match name to be omitted.
        /// </remarks>
        public GetArrayVersion? GetArrayVersion { get; }

        /// <summary>
        /// Function to get a content version
        /// </summary>
        /// <remarks>
        /// A content version method takes the file path, the file contents,
        /// and a list of found positions and returns a single string. That
        /// string is either a version string, in which case it will be appended
        /// to the match name, or `null`, in which case it will cause
        /// the match name to be omitted.
        /// </remarks>
        public GetStreamVersion? GetStreamVersion { get; }

        #region Generic Constructors

        public ContentMatchSet(byte[] needle, string matchName)
            : this([needle], getArrayVersion: null, matchName) { }

        public ContentMatchSet(byte?[] needle, string matchName)
            : this([needle], getArrayVersion: null, matchName) { }

        public ContentMatchSet(List<byte[]> needles, string matchName)
            : this(needles, getArrayVersion: null, matchName) { }

        public ContentMatchSet(List<byte?[]> needles, string matchName)
            : this(needles, getArrayVersion: null, matchName) { }

        public ContentMatchSet(ContentMatch needle, string matchName)
            : this([needle], getArrayVersion: null, matchName) { }

        public ContentMatchSet(List<ContentMatch> needles, string matchName)
            : this(needles, getArrayVersion: null, matchName) { }

        #endregion

        #region Array Constructors

        public ContentMatchSet(byte[] needle, GetArrayVersion? getArrayVersion, string matchName)
            : this([needle], getArrayVersion, matchName) { }

        public ContentMatchSet(byte?[] needle, GetArrayVersion? getArrayVersion, string matchName)
            : this([needle], getArrayVersion, matchName) { }

        public ContentMatchSet(List<byte[]> needles, GetArrayVersion? getArrayVersion, string matchName)
            : this(needles.ConvertAll(n => new ContentMatch(n)), getArrayVersion, matchName) { }

        public ContentMatchSet(List<byte?[]> needles, GetArrayVersion? getArrayVersion, string matchName)
            : this(needles.ConvertAll(n => new ContentMatch(n)), getArrayVersion, matchName) { }

        public ContentMatchSet(ContentMatch needle, GetArrayVersion? getArrayVersion, string matchName)
            : this([needle], getArrayVersion, matchName) { }

        public ContentMatchSet(List<ContentMatch> needles, GetArrayVersion? getArrayVersion, string matchName)
        {
            Matchers = needles;
            GetArrayVersion = getArrayVersion;
            MatchName = matchName;
        }

        #endregion

        #region Stream Constructors

        public ContentMatchSet(byte[] needle, GetStreamVersion? getStreamVersion, string matchName)
            : this([needle], getStreamVersion, matchName) { }

        public ContentMatchSet(byte?[] needle, GetStreamVersion? getStreamVersion, string matchName)
            : this([needle], getStreamVersion, matchName) { }

        public ContentMatchSet(List<byte[]> needles, GetStreamVersion? getStreamVersion, string matchName)
            : this(needles.ConvertAll(n => new ContentMatch(n)), getStreamVersion, matchName) { }

        public ContentMatchSet(List<byte?[]> needles, GetStreamVersion? getStreamVersion, string matchName)
            : this(needles.ConvertAll(n => new ContentMatch(n)), getStreamVersion, matchName) { }

        public ContentMatchSet(ContentMatch needle, GetStreamVersion? getStreamVersion, string matchName)
            : this([needle], getStreamVersion, matchName) { }

        public ContentMatchSet(List<ContentMatch> needles, GetStreamVersion? getStreamVersion, string matchName)
        {
            Matchers = needles;
            GetStreamVersion = getStreamVersion;
            MatchName = matchName;
        }

        #endregion

        #region Array Matching

        /// <summary>
        /// Determine whether all content matches pass
        /// </summary>
        /// <param name="stack">Array to search</param>
        /// <returns>List of matching positions, if any</returns>
        public List<int> MatchesAll(byte[]? stack)
        {
            // If no content matches are defined, we fail out
            if (Matchers == null)
                return [];

            // Initialize the position list
            var positions = new List<int>();

            // Loop through all content matches and make sure all pass
            foreach (var contentMatch in Matchers)
            {
                int position = contentMatch.Match(stack);
                if (position < 0)
                    return [];
                else
                    positions.Add(position);
            }

            return positions;
        }

        /// <summary>
        /// Determine whether any content matches pass
        /// </summary>
        /// <param name="stack">Array to search</param>
        /// <returns>First matching position on success, -1 on error</returns>
        public int MatchesAny(byte[]? stack)
        {
            // If no content matches are defined, we fail out
            if (Matchers == null)
                return -1;

            // Loop through all content matches and make sure all pass
            foreach (var contentMatch in Matchers)
            {
                int position = contentMatch.Match(stack);
                if (position >= 0)
                    return position;
            }

            return -1;
        }

        #endregion

        #region Stream Matching

        /// <summary>
        /// Determine whether all content matches pass
        /// </summary>
        /// <param name="stack">Stream to search</param>
        /// <returns>List of matching positions, if any</returns>
        public List<int> MatchesAll(Stream? stack)
        {
            // If no content matches are defined, we fail out
            if (Matchers == null)
                return [];

            // Initialize the position list
            var positions = new List<int>();

            // Loop through all content matches and make sure all pass
            foreach (var contentMatch in Matchers)
            {
                int position = contentMatch.Match(stack);
                if (position < 0)
                    return [];
                else
                    positions.Add(position);
            }

            return positions;
        }

        /// <summary>
        /// Determine whether any content matches pass
        /// </summary>
        /// <param name="stack">Stream to search</param>
        /// <returns>First matching position on success, -1 on error</returns>
        public int MatchesAny(Stream? stack)
        {
            // If no content matches are defined, we fail out
            if (Matchers == null)
                return -1;

            // Loop through all content matches and make sure all pass
            foreach (var contentMatch in Matchers)
            {
                int position = contentMatch.Match(stack);
                if (position >= 0)
                    return position;
            }

            return -1;
        }

        #endregion
    }
}