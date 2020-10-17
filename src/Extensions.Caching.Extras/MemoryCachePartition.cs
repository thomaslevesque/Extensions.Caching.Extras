using System;
using Microsoft.Extensions.Caching.Memory;

namespace Extensions.Caching.Extras
{
    /// <summary>
    /// A partition over an <see cref="IMemoryCache"/>.
    /// </summary>
    public class MemoryCachePartition : IMemoryCache
    {
        private readonly IMemoryCache _cache;

        /// <summary>
        /// Initializes a new instance of <see cref="MemoryCachePartition"/>.
        /// </summary>
        /// <param name="cache">The cache over which to create a partition.</param>
        /// <param name="partitionKey">The partition key.</param>
        public MemoryCachePartition(IMemoryCache cache, object partitionKey)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            PartitionKey = partitionKey ?? throw new ArgumentNullException(nameof(partitionKey));
        }

        /// <summary>
        /// The key of this partition.
        /// </summary>
        public object PartitionKey { get; }

        /// <inheritdoc/>
        public ICacheEntry CreateEntry(object key)
        {
            var entry = _cache.CreateEntry(CreateCacheKey(key));

            return entry;
        }

        /// <inheritdoc/>
        public void Remove(object key)
        {
            _cache.Remove(CreateCacheKey(key));
        }

        /// <inheritdoc/>
        public bool TryGetValue(object key, out object value)
        {
            return _cache.TryGetValue(CreateCacheKey(key), out value);
        }

        /// <inheritdoc/>
        void IDisposable.Dispose()
        {
            // Do NOT dispose the underlying IMemoryCache, as there might be other cache wrappers using it
        }

        private object CreateCacheKey(object key) => (PartitionKey, key);
    }
}
