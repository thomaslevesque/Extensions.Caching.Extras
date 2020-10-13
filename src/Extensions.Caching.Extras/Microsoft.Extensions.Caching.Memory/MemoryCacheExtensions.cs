using System;
using Extensions.Caching.Extras;

namespace Microsoft.Extensions.Caching.Memory
{
    public static class MemoryCacheExtensions
    {
        public static IMemoryCachePartition<TPartition> Partition<TPartition>(this IMemoryCache cache)
        {
            if (cache == null)
                throw new ArgumentNullException(nameof(cache));

            return new MemoryCachePartition<TPartition>(cache);
        }

        public static IMemoryCache Partition(this IMemoryCache cache, object partitionKey)
        {
            if (cache == null)
                throw new ArgumentNullException(nameof(cache));

            if (partitionKey == null) 
                throw new ArgumentNullException(nameof(partitionKey));

            return new MemoryCachePartition(cache, partitionKey);
        }

        public static IEvictableMemoryCache WithEviction(this IMemoryCache cache)
        {
            if (cache == null)
                throw new ArgumentNullException(nameof(cache));

            return new EvictableMemoryCache(cache);
        }
    }
}
