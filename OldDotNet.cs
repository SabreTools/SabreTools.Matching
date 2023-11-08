using System;
using System.Collections.Generic;

#if NET20

namespace System
{
    public delegate TResult Func<in T1, in T2, out TResult>(T1 arg1, T2 arg2);

    public delegate TResult Func<in T1, in T2, in T3, out TResult>(T1 arg1, T2 arg2, T3 arg3);
}

namespace System.Linq
{
    public delegate U LinqSelectDelegate<T, U>(T str);

    public delegate bool LinqWhereDelegate<T>(T str);

    public static partial class EnumerationExtensions
    {
        public static bool Any<T>(this IEnumerable<T> arr)
        {
            if (arr == null)
                return false;
            foreach (var val in arr)
            {
                return true;
            }
            return false;
        }

        public static T First<T>(this IEnumerable<T> arr)
        {
            if (arr == null)
                throw new ArgumentNullException(nameof(arr));
            foreach (var s in arr)
            {
                return s;
            }
            throw new ArgumentOutOfRangeException(nameof(arr));
        }

        public static T? FirstOrDefault<T>(this IEnumerable<T> arr)
        {
            if (arr == null)
                return default;
            foreach (var s in arr)
            {
                return s;
            }
            return default;
        }

        public static IEnumerable<U> Select<T, U>(this IEnumerable<T> arr, LinqSelectDelegate<T, U> func)
        {
            var output = new List<U>();
            foreach (var s in arr)
            {
                output.Add(func(s));
            }
            return output;
        }

        public static T[] ToArray<T>(this IEnumerable<T> arr)
        {
            var list = arr.ToList();
            var output = new T[list.Count];
            int index = 0;
            foreach (var s in list)
            {
                output[index++] = s;
            }
            return output;
        }

        public static List<T> ToList<T>(this IEnumerable<T> arr)
        {
            var output = new List<T>();
            foreach (var s in arr)
            {
                output.Add(s);
            }
            return output;
        }
    }
}

namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class ExtensionAttribute : Attribute { }
}

#endif

#if NET20 || NET35

namespace System
{
    public struct ValueTuple<T1, T2>
    {
        public T1 Item1;
        public T2 Item2;

        public ValueTuple(T1 item1, T2 item2)
        {
            Item1 = item1;
            Item2 = item2;
        }
    }

    public struct ValueTuple<T1, T2, T3>
    {
        public T1 Item1;
        public T2 Item2;
        public T3 Item3;

        public ValueTuple(T1 item1, T2 item2, T3 item3)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
        }
    }
}

namespace System.Collections.Concurrent
{
    public class ConcurrentQueue<T> : Queue<T>
    {
        // TODO: Implement
    }
}

namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Event | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue | AttributeTargets.Struct)]
    public sealed class TupleElementNamesAttribute : Attribute
    {
        private string?[] _transformNames;

        public TupleElementNamesAttribute(string?[] transformNames)
        {
            _transformNames = transformNames;
        }
    }
}

#endif