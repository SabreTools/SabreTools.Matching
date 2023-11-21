#if NET40_OR_GREATER || NETCOREAPP
using System.Collections.Concurrent;
#endif
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
        /// <returns>List of strings representing the matched protections, null or empty otherwise</returns>
#if NET20 || NET35
        public static Queue<string>? GetAllMatches(string file, byte[]? stack, IEnumerable<ContentMatchSet>? matchers, bool includeDebug = false)
#else
        public static ConcurrentQueue<string>? GetAllMatches(string file, byte[]? stack, IEnumerable<ContentMatchSet>? matchers, bool includeDebug = false)
#endif
        {
            return FindAllMatches(file, stack, matchers, includeDebug, false);
        }

        /// <summary>
        /// Get first content match for a given list of matchers
        /// </summary>
        /// <param name="file">File to check for matches</param>
        /// <param name="stack">Array to search</param>
        /// <param name="matchers">Enumerable of ContentMatchSets to be run on the file</param>
        /// <param name="includeDebug">True to include positional data, false otherwise</param>
        /// <returns>String representing the matched protection, null otherwise</returns>
        public static string? GetFirstMatch(string file, byte[]? stack, IEnumerable<ContentMatchSet>? matchers, bool includeDebug = false)
        {
            var contentMatches = FindAllMatches(file, stack, matchers, includeDebug, true);
            if (contentMatches == null || !contentMatches.Any())
                return null;

            return contentMatches.First();
        }

        /// <summary>
        /// Get the required set of content matches on a per Matcher basis
        /// </summary>
        /// <param name="file">File to check for matches</param>
        /// <param name="stack">Array to search</param>
        /// <param name="matchers">Enumerable of ContentMatchSets to be run on the file</param>
        /// <param name="includeDebug">True to include positional data, false otherwise</param>
        /// <param name="stopAfterFirst">True to stop after the first match, false otherwise</param>
        /// <returns>List of strings representing the matched protections, null or empty otherwise</returns>
#if NET20 || NET35
        private static Queue<string>? FindAllMatches(string file, byte[]? stack, IEnumerable<ContentMatchSet>? matchers, bool includeDebug, bool stopAfterFirst)
#else
        private static ConcurrentQueue<string>? FindAllMatches(string file, byte[]? stack, IEnumerable<ContentMatchSet>? matchers, bool includeDebug, bool stopAfterFirst)
#endif
        {
            // If there's no mappings, we can't match
            if (matchers == null || !matchers.Any())
                return null;

            // Initialize the queue of matched protections
#if NET20 || NET35
            var matchedProtections = new Queue<string>();
#else
            var matchedProtections = new ConcurrentQueue<string>();
#endif

            // Loop through and try everything otherwise
            foreach (var matcher in matchers)
            {
                // Determine if the matcher passes
                (bool passes, List<int> positions) = matcher.MatchesAll(stack);
                if (!passes)
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

                // If we there is no version method, just return the protection name
                if (matcher.GetArrayVersion == null)
                {
                    matchedProtections.Enqueue((matcher.ProtectionName ?? "Unknown Protection") + (includeDebug ? $" (Index {positionsString})" : string.Empty));
                }

                // Otherwise, invoke the version method
                else
                {
                    // A null version returned means the check didn't pass at the version step
                    var version = matcher.GetArrayVersion(file, stack, positions);
                    if (version == null)
                        continue;

                    matchedProtections.Enqueue($"{matcher.ProtectionName ?? "Unknown Protection"} {version}".Trim() + (includeDebug ? $" (Index {positionsString})" : string.Empty));
                }

                // If we're stopping after the first protection, bail out here
                if (stopAfterFirst)
                    return matchedProtections;
            }

            return matchedProtections;
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
        /// <returns>List of strings representing the matched protections, null or empty otherwise</returns>
#if NET20 || NET35
        public static Queue<string>? GetAllMatches(string file, Stream? stack, IEnumerable<ContentMatchSet>? matchers, bool includeDebug = false)
#else
        public static ConcurrentQueue<string>? GetAllMatches(string file, Stream? stack, IEnumerable<ContentMatchSet>? matchers, bool includeDebug = false)
#endif
        {
            return FindAllMatches(file, stack, matchers, includeDebug, false);
        }

        /// <summary>
        /// Get first content match for a given list of matchers
        /// </summary>
        /// <param name="file">File to check for matches</param>
        /// <param name="stack">Stream to search</param>
        /// <param name="matchers">Enumerable of ContentMatchSets to be run on the file</param>
        /// <param name="includeDebug">True to include positional data, false otherwise</param>
        /// <returns>String representing the matched protection, null otherwise</returns>
        public static string? GetFirstMatch(string file, Stream? stack, IEnumerable<ContentMatchSet>? matchers, bool includeDebug = false)
        {
            var contentMatches = FindAllMatches(file, stack, matchers, includeDebug, true);
            if (contentMatches == null || !contentMatches.Any())
                return null;

            return contentMatches.First();
        }

        /// <summary>
        /// Get the required set of content matches on a per Matcher basis
        /// </summary>
        /// <param name="file">File to check for matches</param>
        /// <param name="stack">Stream to search</param>
        /// <param name="matchers">Enumerable of ContentMatchSets to be run on the file</param>
        /// <param name="includeDebug">True to include positional data, false otherwise</param>
        /// <param name="stopAfterFirst">True to stop after the first match, false otherwise</param>
        /// <returns>List of strings representing the matched protections, null or empty otherwise</returns>
#if NET20 || NET35
        private static Queue<string>? FindAllMatches(string file, Stream? stack, IEnumerable<ContentMatchSet>? matchers, bool includeDebug, bool stopAfterFirst)
#else
        private static ConcurrentQueue<string>? FindAllMatches(string file, Stream? stack, IEnumerable<ContentMatchSet>? matchers, bool includeDebug, bool stopAfterFirst)
#endif
        {
            // If there's no mappings, we can't match
            if (matchers == null || !matchers.Any())
                return null;

            // Initialize the queue of matched protections
#if NET20 || NET35
            var matchedProtections = new Queue<string>();
#else
            var matchedProtections = new ConcurrentQueue<string>();
#endif

            // Loop through and try everything otherwise
            foreach (var matcher in matchers)
            {
                // Determine if the matcher passes
                (bool passes, List<int> positions) = matcher.MatchesAll(stack);
                if (!passes)
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

                // If we there is no version method, just return the protection name
                if (matcher.GetStreamVersion == null)
                {
                    matchedProtections.Enqueue((matcher.ProtectionName ?? "Unknown Protection") + (includeDebug ? $" (Index {positionsString})" : string.Empty));
                }

                // Otherwise, invoke the version method
                else
                {
                    // A null version returned means the check didn't pass at the version step
                    var version = matcher.GetStreamVersion(file, stack, positions);
                    if (version == null)
                        continue;

                    matchedProtections.Enqueue($"{matcher.ProtectionName ?? "Unknown Protection"} {version}".Trim() + (includeDebug ? $" (Index {positionsString})" : string.Empty));
                }

                // If we're stopping after the first protection, bail out here
                if (stopAfterFirst)
                    return matchedProtections;
            }

            return matchedProtections;
        }

        #endregion

        #region Path Matching

        /// <summary>
        /// Get all path matches for a given list of matchers
        /// </summary>
        /// <param name="file">File path to check for matches</param>
        /// <param name="matchers">Enumerable of PathMatchSets to be run on the file</param>
        /// <param name="any">True if any path match is a success, false if all have to match</param>
        /// <returns>List of strings representing the matched protections, null or empty otherwise</returns>
#if NET20 || NET35
        public static Queue<string> GetAllMatches(string file, IEnumerable<PathMatchSet>? matchers, bool any = false)
#else
        public static ConcurrentQueue<string> GetAllMatches(string file, IEnumerable<PathMatchSet>? matchers, bool any = false)
#endif
        {
            return FindAllMatches(new List<string> { file }, matchers, any, false);
        }

        // <summary>
        /// Get all path matches for a given list of matchers
        /// </summary>
        /// <param name="files">File paths to check for matches</param>
        /// <param name="matchers">Enumerable of PathMatchSets to be run on the file</param>
        /// <param name="any">True if any path match is a success, false if all have to match</param>
        /// <returns>List of strings representing the matched protections, null or empty otherwise</returns>
#if NET20 || NET35
        public static Queue<string> GetAllMatches(IEnumerable<string>? files, IEnumerable<PathMatchSet>? matchers, bool any = false)
#else
        public static ConcurrentQueue<string> GetAllMatches(IEnumerable<string>? files, IEnumerable<PathMatchSet>? matchers, bool any = false)
#endif
        {
            return FindAllMatches(files, matchers, any, false);
        }

        /// <summary>
        /// Get first path match for a given list of matchers
        /// </summary>
        /// <param name="file">File path to check for matches</param>
        /// <param name="matchers">Enumerable of PathMatchSets to be run on the file</param>
        /// <param name="any">True if any path match is a success, false if all have to match</param>
        /// <returns>String representing the matched protection, null otherwise</returns>
        public static string? GetFirstMatch(string file, IEnumerable<PathMatchSet> matchers, bool any = false)
        {
            var contentMatches = FindAllMatches(new List<string> { file }, matchers, any, true);
            if (contentMatches == null || !contentMatches.Any())
                return null;

            return contentMatches.First();
        }

        /// <summary>
        /// Get first path match for a given list of matchers
        /// </summary>
        /// <param name="files">File paths to check for matches</param>
        /// <param name="matchers">Enumerable of PathMatchSets to be run on the file</param>
        /// <param name="any">True if any path match is a success, false if all have to match</param>
        /// <returns>String representing the matched protection, null otherwise</returns>
        public static string? GetFirstMatch(IEnumerable<string> files, IEnumerable<PathMatchSet> matchers, bool any = false)
        {
            var contentMatches = FindAllMatches(files, matchers, any, true);
            if (contentMatches == null || !contentMatches.Any())
                return null;

            return contentMatches.First();
        }

        /// <summary>
        /// Get the required set of path matches on a per Matcher basis
        /// </summary>
        /// <param name="files">File paths to check for matches</param>
        /// <param name="matchers">Enumerable of PathMatchSets to be run on the file</param>
        /// <param name="any">True if any path match is a success, false if all have to match</param>
        /// <param name="stopAfterFirst">True to stop after the first match, false otherwise</param>
        /// <returns>List of strings representing the matched protections, null or empty otherwise</returns>
#if NET20 || NET35
        private static Queue<string> FindAllMatches(IEnumerable<string>? files, IEnumerable<PathMatchSet>? matchers, bool any, bool stopAfterFirst)
#else
        private static ConcurrentQueue<string> FindAllMatches(IEnumerable<string>? files, IEnumerable<PathMatchSet>? matchers, bool any, bool stopAfterFirst)
#endif
        {
            // If there's no mappings, we can't match
            if (matchers == null || !matchers.Any())
                return new();

            // Initialize the list of matched protections
#if NET20 || NET35
            var matchedProtections = new Queue<string>();
#else
            var matchedProtections = new ConcurrentQueue<string>();
#endif

            // Loop through and try everything otherwise
            foreach (var matcher in matchers)
            {
                // Determine if the matcher passes
                bool passes;
                string? firstMatchedString;
                if (any)
                {
                    (bool anyPasses, var matchedString) = matcher.MatchesAny(files);
                    passes = anyPasses;
                    firstMatchedString = matchedString;
                }
                else
                {
                    (bool allPasses, List<string> matchedStrings) = matcher.MatchesAll(files);
                    passes = allPasses;
                    firstMatchedString = matchedStrings.FirstOrDefault();
                }

                // If we don't have a pass, just continue
                if (!passes || firstMatchedString == null)
                    continue;

                // If we there is no version method, just return the protection name
                if (matcher.GetVersion == null)
                {
                    matchedProtections.Enqueue(matcher.ProtectionName ?? "Unknown Protection");
                }

                // Otherwise, invoke the version method
                else
                {
                    // A null version returned means the check didn't pass at the version step
                    var version = matcher.GetVersion(firstMatchedString, files);
                    if (version == null)
                        continue;

                    matchedProtections.Enqueue($"{matcher.ProtectionName ?? "Unknown Protection"} {version}".Trim());
                }

                // If we're stopping after the first protection, bail out here
                if (stopAfterFirst)
                    return matchedProtections;
            }

            return matchedProtections;
        }

        #endregion
    }
}