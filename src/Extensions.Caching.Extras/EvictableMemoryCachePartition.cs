namespace Extensions.Caching.Extras
{
    public class EvictableMemoryCachePartition<TPartition> : EvictableMemoryCache, IEvictableMemoryCachePartition<TPartition>
    {
        public EvictableMemoryCachePartition(IMemoryCachePartition<TPartition> partition)
            : base(partition)
        {
        }
    }
}
