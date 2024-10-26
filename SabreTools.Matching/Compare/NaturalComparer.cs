﻿/*
 * 
 * Links for info and original source code:
 * 
 * https://blog.codinghorror.com/sorting-for-humans-natural-sort-order/
 * http://www.codeproject.com/Articles/22517/Natural-Sort-Comparer
 *
 * Exact code implementation used with permission, originally by motoschifo
 * 
 */

using System;
using System.Collections.Generic;
#if NET40_OR_GREATER || NETCOREAPP
using System.Linq;
#endif
using System.Text.RegularExpressions;

namespace SabreTools.Matching.Compare
{
    public class NaturalComparer : Comparer<string>, IDisposable
    {
        private readonly Dictionary<string, string[]> _table;

        public NaturalComparer()
        {
            _table = [];
        }

        public void Dispose()
        {
            _table.Clear();
        }

        public override int Compare(string? x, string? y)
        {
            if (x == null || y == null)
            {
                if (x == null && y != null)
                    return -1;
                else if (x != null && y == null)
                    return 1;
                else
                    return 0;
            }

            if (x.ToLowerInvariant() == y.ToLowerInvariant())
                return x.CompareTo(y);

            if (!_table.TryGetValue(x, out string[]? x1))
            {
                //x1 = Regex.Split(x.Replace(" ", string.Empty), "([0-9]+)");
#if NET20 || NET35
                var nonempty = new List<string>();
                x1 = Regex.Split(x.ToLowerInvariant(), "([0-9]+)");
                foreach (var s in x1)
                {
                    if (!string.IsNullOrEmpty(s))
                        nonempty.Add(s);
                }

                x1 = nonempty.ToArray();
#else
                x1 = Regex.Split(x.ToLowerInvariant(), "([0-9]+)")
                    .Where(s => !string.IsNullOrEmpty(s))
                    .ToArray();
#endif
                _table.Add(x, x1);
            }

            if (!_table.TryGetValue(y, out string[]? y1))
            {
                //y1 = Regex.Split(y.Replace(" ", string.Empty), "([0-9]+)");
#if NET20 || NET35
                var nonempty = new List<string>();
                y1 = Regex.Split(x.ToLowerInvariant(), "([0-9]+)");
                foreach (var s in y1)
                {
                    if (!string.IsNullOrEmpty(s))
                        nonempty.Add(s);
                }

                y1 = nonempty.ToArray();
#else
                y1 = Regex.Split(x.ToLowerInvariant(), "([0-9]+)")
                    .Where(s => !string.IsNullOrEmpty(s))
                    .ToArray();
#endif
                _table.Add(y, y1);
            }

            for (int i = 0; i < x1.Length && i < y1.Length; i++)
            {
                if (x1[i] != y1[i])
                    return PartCompare(x1[i], y1[i]);
            }

            if (y1.Length > x1.Length)
                return 1;
            else if (x1.Length > y1.Length)
                return -1;
            else
                return x.CompareTo(y);
        }

        private static int PartCompare(string left, string right)
        {
            if (!long.TryParse(left, out long x))
                return NaturalComparerUtil.CompareNumeric(left, right);

            if (!long.TryParse(right, out long y))
                return NaturalComparerUtil.CompareNumeric(left, right);

            // If we have an equal part, then make sure that "longer" ones are taken into account
            if (x.CompareTo(y) == 0)
                return left.Length - right.Length;

            return x.CompareTo(y);
        }
    }
}
