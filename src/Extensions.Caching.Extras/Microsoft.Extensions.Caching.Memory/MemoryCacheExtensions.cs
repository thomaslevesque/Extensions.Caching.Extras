using System;
using Extensions.Caching.Extras;

namespace Microsoft.Extensions.Caching.Memory
{
    /// <summary>
    /// Provides methods to extend the behavior of <see cref="IMemoryCache" />.
    /// </summary>
    public static class MemoryCacheExtensions
    {
        /// <summary>
        /// Returns a partition of the <see cref="IMemoryCache"/> whose partition key is <c>typeof(TPartition)</c>.
        /// </summary>
        /// <typeparam name="TPartition">The type used as a partition key.</typeparam>
        /// <param name="cache">The <see cref="IMemoryCache"/> of which to get a partition.</param>
        /// <returns>A partition of the <see cref="IMemoryCache"/>.</returns>
        public static IMemoryCache Partition<TPartition>(this IMemoryCache cache)
        {
            if (cache == null)
                throw new ArgumentNullException(nameof(cache));

            return cache.Partition(typeof(TPartition));
        }

        /// <summary>
        /// Returns a partition of the <see cref="IMemoryCache"/> with the specified partition key.
        /// </summary>
        /// <param name="cache">The <see cref="IMemoryCache"/> of which to get a partition.</param>
        /// <param name="partitionKey">The partition key.</param>
        /// <returns>A partition of the <see cref="IMemoryCache"/>.</returns>
        public static IMemoryCache Partition(this IMemoryCache cache, object partitionKey)
        {
            if (cache == null)
                throw new ArgumentNullException(nameof(cache));

            if (partitionKey == null) 
                throw new ArgumentNullException(nameof(partitionKey));

            return new MemoryCachePartition(cache, partitionKey);
        }

        /// <summary>
        /// Returns a wrapper of the <see cref="IMemoryCache"/> that supports cache eviction.
        /// </summary>
        /// <param name="cache">The <see cref="IMemoryCache"/> to wrap.</param>
        /// <returns>An evictable memory cache.</returns>
        public static IEvictableMemoryCache WithEviction(this IMemoryCache cache)
        {
            if (cache == null)
                throw new ArgumentNullException(nameof(cache));

            return new EvictableMemoryCache(cache);
        }
    }
}
