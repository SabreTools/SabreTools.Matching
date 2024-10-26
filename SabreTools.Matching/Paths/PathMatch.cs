using System.Collections.Generic;
#if NET40_OR_GREATER || NETCOREAPP
using System.Linq;
#endif

namespace SabreTools.Matching.Paths
{
    /// <summary>
    /// Path matching criteria
    /// </summary>
    public class PathMatch : IMatch<string>
    {
        /// <summary>
        /// String to match
        /// </summary>
        public string? Needle { get; }

        /// <summary>
        /// Match exact casing instead of invariant
        /// </summary>
        public bool MatchExact { get; private set; }

        /// <summary>
        /// Match that values end with the needle and not just contains
        /// </summary>
        public bool UseEndsWith { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="needle">String representing the search</param>
        /// <param name="matchExact">True to match exact casing, false otherwise</param>
        /// <param name="useEndsWith">True to match the end only, false for all contents</param>
        public PathMatch(string? needle, bool matchExact = false, bool useEndsWith = false)
        {
            Needle = needle;
            MatchExact = matchExact;
            UseEndsWith = useEndsWith;
        }

        #region Matching

        /// <summary>
        /// Get if this match can be found in a stack
        /// </summary>
        /// <param name="stack">List of strings to search for the given content</param>
        /// <returns>Matched item on success, null on error</returns>
        public string? Match(IEnumerable<string>? stack)
        {
            // If either array is null or empty, we can't do anything
#if NET20 || NET35
            if (stack == null || new List<string>(stack).Count == 0 || Needle == null || Needle.Length == 0)
#else
            if (stack == null || !stack.Any() || Needle == null || Needle.Length == 0)
#endif
                return null;

            // Preprocess the needle, if necessary
            string procNeedle = MatchExact ? Needle : Needle.ToLowerInvariant();

            foreach (string stackItem in stack)
            {
                // Preprocess the stack item, if necessary
                string procStackItem = MatchExact ? stackItem : stackItem.ToLowerInvariant();

                if (UseEndsWith && procStackItem.EndsWith(procNeedle))
                    return stackItem;
                else if (!UseEndsWith && procStackItem.Contains(procNeedle))
                    return stackItem;
            }

            return null;
        }

        #endregion
    }
}