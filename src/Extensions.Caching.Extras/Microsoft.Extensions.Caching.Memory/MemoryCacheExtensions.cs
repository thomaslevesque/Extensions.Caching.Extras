using System;
using Extensions.Caching.Extras;

namespace  Microsoft.Extensions.Caching.Memory
{
    public static class MemoryCacheExtensions
    {
        public static IMemoryCachePartition GetPartition<TPartition>(this IMemoryCache cache)
        {
            if (cache == null)
                throw new ArgumentNullException(nameof(cache));

            return new MemoryCachePartition<TPartition>(cache);
        }

        public static IMemoryCachePartition GetPartition(this IMemoryCache cache, object partitionKey)
        {
            if (partitionKey == null) 
                throw new ArgumentNullException(nameof(partitionKey));

            return new MemoryCachePartition(cache, partitionKey);
        }
    }
}
