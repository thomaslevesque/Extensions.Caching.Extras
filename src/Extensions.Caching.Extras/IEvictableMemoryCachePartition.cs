namespace Extensions.Caching.Extras
{
    public interface IEvictableMemoryCachePartition<TPartition> : IEvictableMemoryCache, IMemoryCachePartition<TPartition>
    {
    }
}
