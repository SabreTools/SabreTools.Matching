using System;
using System.Collections.Generic;

namespace SabreTools.Matching.Paths
{
    /// <summary>
    /// A set of path matches that work together
    /// </summary>
    public class PathMatchSet : MatchSet<PathMatch, string>
    {
        /// <summary>
        /// Function to get a path version for this Matcher
        /// </summary>
        /// <remarks>
        /// A path version method takes the matched path and an enumerable of files
        /// and returns a single string. That string is either a version string,
        /// in which case it will be appended to the match name, or `null`,
        /// in which case it will cause the match name to be omitted.
        /// </remarks>
        public Func<string, IEnumerable<string>?, string?>? GetVersion { get; }

        #region Constructors

        public PathMatchSet(string needle, string matchName)
            : this([needle], null, matchName) { }

        public PathMatchSet(List<string> needles, string matchName)
            : this(needles, null, matchName) { }

        public PathMatchSet(string needle, Func<string, IEnumerable<string>?, string?>? getVersion, string matchName)
            : this([needle], getVersion, matchName) { }

        public PathMatchSet(List<string> needles, Func<string, IEnumerable<string>?, string?>? getVersion, string matchName)
            : this(needles.ConvertAll(n => new PathMatch(n)), getVersion, matchName) { }

        public PathMatchSet(PathMatch needle, string matchName)
            : this([needle], null, matchName) { }

        public PathMatchSet(List<PathMatch> needles, string matchName)
            : this(needles, null, matchName) { }

        public PathMatchSet(PathMatch needle, Func<string, IEnumerable<string>?, string?>? getVersion, string matchName)
            : this([needle], getVersion, matchName) { }

        public PathMatchSet(List<PathMatch> needles, Func<string, IEnumerable<string>?, string?>? getVersion, string matchName)
        {
            Matchers = needles;
            GetVersion = getVersion;
            MatchName = matchName;
        }

        #endregion

        #region Matching

        /// <summary>
        /// Determine whether all path matches pass
        /// </summary>
        /// <param name="stack">List of strings to try to match</param>
        /// <returns>List of matching values, if any</returns>
        public List<string> MatchesAll(IEnumerable<string>? stack)
        {
            // If no path matches are defined, we fail out
            if (Matchers == null)
                return [];

            // Initialize the value list
            List<string> values = [];

            // Loop through all path matches and make sure all pass
            foreach (var pathMatch in Matchers)
            {
                string? value = pathMatch.Match(stack);
                if (value == null)
                    return [];
                else
                    values.Add(value);
            }

            return values;
        }

        /// <summary>
        /// Determine whether any path matches pass
        /// </summary>
        /// <param name="stack">List of strings to try to match</param>
        /// <returns>First matching value on success, null on error</returns>
        public string? MatchesAny(IEnumerable<string>? stack)
        {
            // If no path matches are defined, we fail out
            if (Matchers == null)
                return null;

            // Loop through all path matches and make sure all pass
            foreach (var pathMatch in Matchers)
            {
                string? value = pathMatch.Match(stack);
                if (value != null)
                    return value;
            }

            return null;
        }

        #endregion
    }
}