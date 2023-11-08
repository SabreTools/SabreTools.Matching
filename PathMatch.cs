using System.Collections.Generic;
using System.Linq;

namespace SabreTools.Matching
{
    /// <summary>
    /// Path matching criteria
    /// </summary>
    public class PathMatch : IMatch<string>
    {
        /// <summary>
        /// String to match
        /// </summary>
#if NETFRAMEWORK || NETCOREAPP
        public string? Needle { get; private set; }
#else
        public string? Needle { get; init; }
#endif

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
            this.Needle = needle;
            this.MatchExact = matchExact;
            this.UseEndsWith = useEndsWith;
        }

        #region Matching

        /// <summary>
        /// Get if this match can be found in a stack
        /// </summary>
        /// <param name="stack">List of strings to search for the given content</param>
        /// <returns>Tuple of success and matched item</returns>
        public (bool, string?) Match(IEnumerable<string>? stack)
        {
            // If either array is null or empty, we can't do anything
            if (stack == null || !stack.Any() || this.Needle == null || this.Needle.Length == 0)
                return (false, null);

            // Preprocess the needle, if necessary
            string procNeedle = this.MatchExact ? this.Needle : this.Needle.ToLowerInvariant();

            foreach (string stackItem in stack)
            {
                // Preprocess the stack item, if necessary
                string procStackItem = this.MatchExact ? stackItem : stackItem.ToLowerInvariant();

                if (this.UseEndsWith && procStackItem.EndsWith(procNeedle))
                    return (true, stackItem);
                else if (!this.UseEndsWith && procStackItem.Contains(procNeedle))
                    return (true, stackItem);
            }

            return (false, null);
        }
    
        #endregion
    }
}