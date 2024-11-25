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
        public GetPathVersion? GetVersion { get; }

        #region Constructors

        public PathMatchSet(string needle, string matchName)
            : this([needle], null, matchName) { }

        public PathMatchSet(List<string> needles, string matchName)
            : this(needles, null, matchName) { }

        public PathMatchSet(string needle, GetPathVersion? getVersion, string matchName)
            : this([needle], getVersion, matchName) { }

        public PathMatchSet(List<string> needles, GetPathVersion? getVersion, string matchName)
            : this(needles.ConvertAll(n => new PathMatch(n)), getVersion, matchName) { }

        public PathMatchSet(PathMatch needle, string matchName)
            : this([needle], null, matchName) { }

        public PathMatchSet(List<PathMatch> needles, string matchName)
            : this(needles, null, matchName) { }

        public PathMatchSet(PathMatch needle, GetPathVersion? getVersion, string matchName)
            : this([needle], getVersion, matchName) { }

        public PathMatchSet(List<PathMatch> needles, GetPathVersion? getVersion, string matchName)
        {
            Matchers = needles;
            GetVersion = getVersion;
            SetName = matchName;
        }

        #endregion

        #region Matching

        /// <summary>
        /// Get if this match can be found in a stack
        /// </summary>
        /// <param name="stack">List of strings to search for the given content</param>
        /// <returns>Matched item on success, null on error</returns>
        public List<string> MatchesAll(string[]? stack)
            => MatchesAll(stack == null ? null : new List<string>(stack));

        /// <summary>
        /// Determine whether all path matches pass
        /// </summary>
        /// <param name="stack">List of strings to try to match</param>
        /// <returns>List of matching values, if any</returns>
        public List<string> MatchesAll(List<string>? stack)
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
        /// Get if this match can be found in a stack
        /// </summary>
        /// <param name="stack">List of strings to search for the given content</param>
        /// <returns>Matched item on success, null on error</returns>
        public string? MatchesAny(string[]? stack)
            => MatchesAny(stack == null ? null : new List<string>(stack));

        /// <summary>
        /// Determine whether any path matches pass
        /// </summary>
        /// <param name="stack">List of strings to try to match</param>
        /// <returns>First matching value on success, null on error</returns>
        public string? MatchesAny(List<string>? stack)
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