namespace SabreTools.Matching
{
    public interface IMatch<T>
    {
#if NETFRAMEWORK
        T? Needle { get; set; }
#else
        T? Needle { get; init; }
#endif
    }
}
