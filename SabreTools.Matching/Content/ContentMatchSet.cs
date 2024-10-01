using System;
using System.Collections.Generic;
using System.IO;
#if NET40_OR_GREATER || NETCOREAPP
using System.Linq;
#endif

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
        /// to the protection name, or `null`, in which case it will cause
        /// the protection to be omitted.
        /// </remarks>
        public Func<string, byte[]?, List<int>, string?>? GetArrayVersion { get; private set; }

        /// <summary>
        /// Function to get a content version
        /// </summary>
        /// <remarks>
        /// A content version method takes the file path, the file contents,
        /// and a list of found positions and returns a single string. That
        /// string is either a version string, in which case it will be appended
        /// to the protection name, or `null`, in which case it will cause
        /// the protection to be omitted.
        /// </remarks>
        public Func<string, Stream?, List<int>, string?>? GetStreamVersion { get; private set; }

        #region Generic Constructors

        public ContentMatchSet(byte?[] needle, string protectionName)
            : this(new List<byte?[]> { needle }, getArrayVersion: null, protectionName) { }

        public ContentMatchSet(List<byte?[]> needles, string protectionName)
            : this(needles, getArrayVersion: null, protectionName) { }

        public ContentMatchSet(ContentMatch needle, string protectionName)
            : this(new List<ContentMatch>() { needle }, getArrayVersion: null, protectionName) { }

        public ContentMatchSet(List<ContentMatch> needles, string protectionName)
            : this(needles, getArrayVersion: null, protectionName) { }

        #endregion

        #region Array Constructors

        public ContentMatchSet(byte?[] needle, Func<string, byte[]?, List<int>, string?>? getArrayVersion, string protectionName)
            : this(new List<byte?[]> { needle }, getArrayVersion, protectionName) { }

#if NET20 || NET35
        public ContentMatchSet(List<byte?[]> needles, Func<string, byte[]?, List<int>, string?>? getArrayVersion, string protectionName)
        {
            var matchers = new List<ContentMatch>();
            foreach (var n in needles)
            {
                matchers.Add(new ContentMatch(n));
            }

            Matchers = matchers;
            GetArrayVersion = getArrayVersion;
            ProtectionName = protectionName;
        }
#else
        public ContentMatchSet(List<byte?[]> needles, Func<string, byte[]?, List<int>, string?>? getArrayVersion, string protectionName)
            : this(needles.Select(n => new ContentMatch(n)).ToList(), getArrayVersion, protectionName) { }
#endif

        public ContentMatchSet(ContentMatch needle, Func<string, byte[]?, List<int>, string?>? getArrayVersion, string protectionName)
            : this(new List<ContentMatch>() { needle }, getArrayVersion, protectionName) { }

        public ContentMatchSet(List<ContentMatch> needles, Func<string, byte[]?, List<int>, string?>? getArrayVersion, string protectionName)
        {
            Matchers = needles;
            GetArrayVersion = getArrayVersion;
            ProtectionName = protectionName;
        }

        #endregion

        #region Stream Constructors

        public ContentMatchSet(byte?[] needle, Func<string, Stream?, List<int>, string?>? getStreamVersion, string protectionName)
            : this(new List<byte?[]> { needle }, getStreamVersion, protectionName) { }

#if NET20 || NET35
        public ContentMatchSet(List<byte?[]> needles, Func<string, Stream?, List<int>, string?>? getStreamVersion, string protectionName)
        {
            var matchers = new List<ContentMatch>();
            foreach (var n in needles)
            {
                matchers.Add(new ContentMatch(n));
            }

            Matchers = matchers;
            GetStreamVersion = getStreamVersion;
            ProtectionName = protectionName;
        }
#else
        public ContentMatchSet(List<byte?[]> needles, Func<string, Stream?, List<int>, string?>? getStreamVersion, string protectionName)
            : this(needles.Select(n => new ContentMatch(n)).ToList(), getStreamVersion, protectionName) { }
#endif

        public ContentMatchSet(ContentMatch needle, Func<string, Stream?, List<int>, string?>? getStreamVersion, string protectionName)
            : this(new List<ContentMatch>() { needle }, getStreamVersion, protectionName) { }

        public ContentMatchSet(List<ContentMatch> needles, Func<string, Stream?, List<int>, string?>? getStreamVersion, string protectionName)
        {
            Matchers = needles;
            GetStreamVersion = getStreamVersion;
            ProtectionName = protectionName;
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
#if NET20 || NET35
            if (Matchers == null || new List<ContentMatch>(Matchers).Count == 0)
#else
            if (Matchers == null || !Matchers.Any())
#endif
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
#if NET20 || NET35
            if (Matchers == null || new List<ContentMatch>(Matchers).Count == 0)
#else
            if (Matchers == null || !Matchers.Any())
#endif
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
#if NET20 || NET35
            if (Matchers == null || new List<ContentMatch>(Matchers).Count == 0)
#else
            if (Matchers == null || !Matchers.Any())
#endif
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
#if NET20 || NET35
            if (Matchers == null || new List<ContentMatch>(Matchers).Count == 0)
#else
            if (Matchers == null || !Matchers.Any())
#endif
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