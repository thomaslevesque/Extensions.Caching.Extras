using Microsoft.Extensions.Caching.Memory;

namespace Extensions.Caching.Extras
{
    public class MemoryCachePartitionFactory : IMemoryCachePartitionFactory
    {
        private readonly IMemoryCache _cache;

        public MemoryCachePartitionFactory(IMemoryCache cache)
        {
            _cache = cache;
        }
        
        public IMemoryCachePartition Create(object partitionKey)
        {
            return new MemoryCachePartition(_cache, partitionKey);
        }
    }
}
