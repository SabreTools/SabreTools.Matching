using System.Collections.Generic;
using System.IO;
#if NET40_OR_GREATER || NETCOREAPP
using System.Linq;
#endif
using SabreTools.Matching.Content;
using SabreTools.Matching.Paths;

namespace SabreTools.Matching
{
    /// <summary>
    /// Helper class for matching
    /// </summary>
    public static class MatchUtil
    {
        #region Array Content Matching

        /// <summary>
        /// Get all content matches for a given list of matchers
        /// </summary>
        /// <param name="file">File to check for matches</param>
        /// <param name="stack">Array to search</param>
        /// <param name="matchers">Enumerable of ContentMatchSets to be run on the file</param>
        /// <param name="includeDebug">True to include positional data, false otherwise</param>
        /// <returns>List of strings representing the matches, null or empty otherwise</returns>
        public static List<string>? GetAllMatches(string file, byte[]? stack, IEnumerable<ContentMatchSet>? matchers, bool includeDebug = false)
            => FindAllMatches(file, stack, matchers, includeDebug, false);

        /// <summary>
        /// Get first content match for a given list of matchers
        /// </summary>
        /// <param name="file">File to check for matches</param>
        /// <param name="stack">Array to search</param>
        /// <param name="matchers">Enumerable of ContentMatchSets to be run on the file</param>
        /// <param name="includeDebug">True to include positional data, false otherwise</param>
        /// <returns>String representing the match, null otherwise</returns>
        public static string? GetFirstMatch(string file, byte[]? stack, IEnumerable<ContentMatchSet>? matchers, bool includeDebug = false)
        {
            var contentMatches = FindAllMatches(file, stack, matchers, includeDebug, true);
            if (contentMatches == null || contentMatches.Count == 0)
                return null;

            return contentMatches[0];
        }

        /// <summary>
        /// Get the required set of content matches on a per Matcher basis
        /// </summary>
        /// <param name="file">File to check for matches</param>
        /// <param name="stack">Array to search</param>
        /// <param name="matchers">Enumerable of ContentMatchSets to be run on the file</param>
        /// <param name="includeDebug">True to include positional data, false otherwise</param>
        /// <param name="stopAfterFirst">True to stop after the first match, false otherwise</param>
        /// <returns>List of strings representing the matches, empty otherwise</returns>
        private static List<string> FindAllMatches(string file, byte[]? stack, IEnumerable<ContentMatchSet>? matchers, bool includeDebug, bool stopAfterFirst)
        {
            // If there's no mappings, we can't match
            if (matchers == null)
                return [];

            // Initialize the list of matches
            var matchesList = new List<string>();

            // Loop through and try everything otherwise
            foreach (var matcher in matchers)
            {
                // Determine if the matcher passes
                var positions = matcher.MatchesAll(stack);
                if (positions.Count == 0)
                    continue;

                // Format the list of all positions found
#if NET20 || NET35
                var positionStrs = new List<string>();
                foreach (int pos in positions)
                {
                    positionStrs.Add(pos.ToString());
                }
                string positionsString = string.Join(", ", [.. positionStrs]);
#else
                string positionsString = string.Join(", ", positions);
#endif

                // If we there is no version method, just return the match name
                if (matcher.GetArrayVersion == null)
                {
                    matchesList.Add((matcher.MatchName ?? "Unknown") + (includeDebug ? $" (Index {positionsString})" : string.Empty));
                }

                // Otherwise, invoke the version method
                else
                {
                    // A null version returned means the check didn't pass at the version step
                    var version = matcher.GetArrayVersion(file, stack, positions);
                    if (version == null)
                        continue;

                    matchesList.Add($"{matcher.MatchName ?? "Unknown"} {version}".Trim() + (includeDebug ? $" (Index {positionsString})" : string.Empty));
                }

                // If we're stopping after the first match, bail out here
                if (stopAfterFirst)
                    return matchesList;
            }

            return matchesList;
        }

        #endregion

        #region Stream Content Matching

        /// <summary>
        /// Get all content matches for a given list of matchers
        /// </summary>
        /// <param name="file">File to check for matches</param>
        /// <param name="stack">Stream to search</param>
        /// <param name="matchers">Enumerable of ContentMatchSets to be run on the file</param>
        /// <param name="includeDebug">True to include positional data, false otherwise</param>
        /// <returns>List of strings representing the matches, null or empty otherwise</returns>
        public static List<string>? GetAllMatches(string file, Stream? stack, IEnumerable<ContentMatchSet>? matchers, bool includeDebug = false)
            => FindAllMatches(file, stack, matchers, includeDebug, false);

        /// <summary>
        /// Get first content match for a given list of matchers
        /// </summary>
        /// <param name="file">File to check for matches</param>
        /// <param name="stack">Stream to search</param>
        /// <param name="matchers">Enumerable of ContentMatchSets to be run on the file</param>
        /// <param name="includeDebug">True to include positional data, false otherwise</param>
        /// <returns>String representing the match, null otherwise</returns>
        public static string? GetFirstMatch(string file, Stream? stack, IEnumerable<ContentMatchSet>? matchers, bool includeDebug = false)
        {
            var contentMatches = FindAllMatches(file, stack, matchers, includeDebug, true);
            if (contentMatches == null || contentMatches.Count == 0)
                return null;

            return contentMatches[0];
        }

        /// <summary>
        /// Get the required set of content matches on a per Matcher basis
        /// </summary>
        /// <param name="file">File to check for matches</param>
        /// <param name="stack">Stream to search</param>
        /// <param name="matchers">Enumerable of ContentMatchSets to be run on the file</param>
        /// <param name="includeDebug">True to include positional data, false otherwise</param>
        /// <param name="stopAfterFirst">True to stop after the first match, false otherwise</param>
        /// <returns>List of strings representing the matches, empty otherwise</returns>
        private static List<string> FindAllMatches(string file, Stream? stack, IEnumerable<ContentMatchSet>? matchers, bool includeDebug, bool stopAfterFirst)
        {
            // If there's no mappings, we can't match
            if (matchers == null)
                return [];

            // Initialize the list of matches
            var matchesList = new List<string>();

            // Loop through and try everything otherwise
            foreach (var matcher in matchers)
            {
                // Determine if the matcher passes
                var positions = matcher.MatchesAll(stack);
                if (positions.Count == 0)
                    continue;

                // Format the list of all positions found
#if NET20 || NET35
                var positionStrs = new List<string>();
                foreach (int pos in positions)
                {
                    positionStrs.Add(pos.ToString());
                }
                string positionsString = string.Join(", ", [.. positionStrs]);
#else
                string positionsString = string.Join(", ", positions);
#endif

                // If we there is no version method, just return the match name
                if (matcher.GetStreamVersion == null)
                {
                    matchesList.Add((matcher.MatchName ?? "Unknown") + (includeDebug ? $" (Index {positionsString})" : string.Empty));
                }

                // Otherwise, invoke the version method
                else
                {
                    // A null version returned means the check didn't pass at the version step
                    var version = matcher.GetStreamVersion(file, stack, positions);
                    if (version == null)
                        continue;

                    matchesList.Add($"{matcher.MatchName ?? "Unknown"} {version}".Trim() + (includeDebug ? $" (Index {positionsString})" : string.Empty));
                }

                // If we're stopping after the first match, bail out here
                if (stopAfterFirst)
                    return matchesList;
            }

            return matchesList;
        }

        #endregion

        #region Path Matching

        /// <summary>
        /// Get all path matches for a given list of matchers
        /// </summary>
        /// <param name="file">File path to check for matches</param>
        /// <param name="matchers">Enumerable of PathMatchSets to be run on the file</param>
        /// <param name="any">True if any path match is a success, false if all have to match</param>
        /// <returns>List of strings representing the matches, null or empty otherwise</returns>
        public static List<string> GetAllMatches(string file, IEnumerable<PathMatchSet>? matchers, bool any = false)
            => FindAllMatches([file], matchers, any, false);

        // <summary>
        /// Get all path matches for a given list of matchers
        /// </summary>
        /// <param name="files">File paths to check for matches</param>
        /// <param name="matchers">Enumerable of PathMatchSets to be run on the file</param>
        /// <param name="any">True if any path match is a success, false if all have to match</param>
        /// <returns>List of strings representing the matches, null or empty otherwise</returns>
        public static List<string> GetAllMatches(IEnumerable<string>? files, IEnumerable<PathMatchSet>? matchers, bool any = false)
            => FindAllMatches(files, matchers, any, false);

        /// <summary>
        /// Get first path match for a given list of matchers
        /// </summary>
        /// <param name="file">File path to check for matches</param>
        /// <param name="matchers">Enumerable of PathMatchSets to be run on the file</param>
        /// <param name="any">True if any path match is a success, false if all have to match</param>
        /// <returns>String representing the match, null otherwise</returns>
        public static string? GetFirstMatch(string file, IEnumerable<PathMatchSet> matchers, bool any = false)
        {
            var contentMatches = FindAllMatches([file], matchers, any, true);
            if (contentMatches == null || contentMatches.Count == 0)
                return null;

            return contentMatches[0];
        }

        /// <summary>
        /// Get first path match for a given list of matchers
        /// </summary>
        /// <param name="files">File paths to check for matches</param>
        /// <param name="matchers">Enumerable of PathMatchSets to be run on the file</param>
        /// <param name="any">True if any path match is a success, false if all have to match</param>
        /// <returns>String representing the match, null otherwise</returns>
        public static string? GetFirstMatch(IEnumerable<string> files, IEnumerable<PathMatchSet> matchers, bool any = false)
        {
            var contentMatches = FindAllMatches(files, matchers, any, true);
            if (contentMatches == null || contentMatches.Count == 0)
                return null;

            return contentMatches[0];
        }

        /// <summary>
        /// Get the required set of path matches on a per Matcher basis
        /// </summary>
        /// <param name="files">File paths to check for matches</param>
        /// <param name="matchers">Enumerable of PathMatchSets to be run on the file</param>
        /// <param name="any">True if any path match is a success, false if all have to match</param>
        /// <param name="stopAfterFirst">True to stop after the first match, false otherwise</param>
        /// <returns>List of strings representing the matches, null or empty otherwise</returns>
        private static List<string> FindAllMatches(IEnumerable<string>? files, IEnumerable<PathMatchSet>? matchers, bool any, bool stopAfterFirst)
        {
            // If there's no mappings, we can't match
            if (matchers == null)
                return [];

            // Initialize the list of matches
            var matchesList = new List<string>();

            // Loop through and try everything otherwise
            foreach (var matcher in matchers)
            {
                // Determine if the matcher passes
                bool passes;
                string? firstMatchedString;
                if (any)
                {
                    string? matchedString = matcher.MatchesAny(files);
                    passes = matchedString != null;
                    firstMatchedString = matchedString;
                }
                else
                {
                    List<string> matchedStrings = matcher.MatchesAll(files);
                    passes = matchedStrings.Count > 0;
                    firstMatchedString = passes ? matchedStrings[0] : null;
                }

                // If we don't have a pass, just continue
                if (!passes || firstMatchedString == null)
                    continue;

                // If we there is no version method, just return the match name
                if (matcher.GetVersion == null)
                {
                    matchesList.Add(matcher.MatchName ?? "Unknown");
                }

                // Otherwise, invoke the version method
                else
                {
                    // A null version returned means the check didn't pass at the version step
                    var version = matcher.GetVersion(firstMatchedString, files);
                    if (version == null)
                        continue;

                    matchesList.Add($"{matcher.MatchName ?? "Unknown"} {version}".Trim());
                }

                // If we're stopping after the first match, bail out here
                if (stopAfterFirst)
                    return matchesList;
            }

            return matchesList;
        }

        #endregion
    }
}