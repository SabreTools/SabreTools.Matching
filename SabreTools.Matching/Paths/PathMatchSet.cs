using System;
using System.Collections.Generic;
#if NET40_OR_GREATER || NETCOREAPP
using System.Linq;
#endif

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
        /// in which case it will be appended to the protection name, or `null`,
        /// in which case it will cause the protection to be omitted.
        /// </remarks>
        public Func<string, IEnumerable<string>?, string?>? GetVersion { get; }

        #region Constructors

        public PathMatchSet(string needle, string protectionName)
            : this([needle], null, protectionName) { }

        public PathMatchSet(List<string> needles, string protectionName)
            : this(needles, null, protectionName) { }

        public PathMatchSet(string needle, Func<string, IEnumerable<string>?, string?>? getVersion, string protectionName)
            : this([needle], getVersion, protectionName) { }

#if NET20 || NET35
        public PathMatchSet(List<string> needles, Func<string, IEnumerable<string>?, string?>? getVersion, string protectionName)
        {
            var matchers = new List<PathMatch>();
            foreach (var n in needles)
            {
                matchers.Add(new PathMatch(n));
            }

            Matchers = matchers;
            GetVersion = getVersion;
            MatchName = protectionName;
        }
#else
        public PathMatchSet(List<string> needles, Func<string, IEnumerable<string>?, string?>? getVersion, string protectionName)
            : this(needles.Select(n => new PathMatch(n)).ToList(), getVersion, protectionName) { }
#endif

        public PathMatchSet(PathMatch needle, string protectionName)
            : this([needle], null, protectionName) { }

        public PathMatchSet(List<PathMatch> needles, string protectionName)
            : this(needles, null, protectionName) { }

        public PathMatchSet(PathMatch needle, Func<string, IEnumerable<string>?, string?>? getVersion, string protectionName)
            : this([needle], getVersion, protectionName) { }

        public PathMatchSet(List<PathMatch> needles, Func<string, IEnumerable<string>?, string?>? getVersion, string protectionName)
        {
            Matchers = needles;
            GetVersion = getVersion;
            MatchName = protectionName;
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
#if NET20 || NET35
            if (Matchers == null || new List<PathMatch>(Matchers).Count == 0)
#else
            if (Matchers == null || !Matchers.Any())
#endif
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
#if NET20 || NET35
            if (Matchers == null || new List<PathMatch>(Matchers).Count == 0)
#else
            if (Matchers == null || !Matchers.Any())
#endif
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