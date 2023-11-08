namespace SabreTools.Matching
{
    public interface IMatch<T>
    {
#if NETFRAMEWORK || NETCOREAPP
        T? Needle { get; set; }
#else
        T? Needle { get; init; }
#endif
    }
}
