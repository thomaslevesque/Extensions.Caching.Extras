using System;
using Microsoft.Extensions.Caching.Memory;

namespace Extensions.Caching.Extras
{
    public class MemoryCachePartition : IMemoryCache
    {
        private readonly IMemoryCache _cache;

        public MemoryCachePartition(IMemoryCache cache, object partitionKey)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            PartitionKey = partitionKey ?? throw new ArgumentNullException(nameof(partitionKey));
        }

        public object PartitionKey { get; }

        public ICacheEntry CreateEntry(object key)
        {
            var entry = _cache.CreateEntry(CreateCacheKey(key));

            return entry;
        }

        public void Remove(object key)
        {
            _cache.Remove(CreateCacheKey(key));
        }

        public bool TryGetValue(object key, out object value)
        {
            return _cache.TryGetValue(CreateCacheKey(key), out value);
        }

        void IDisposable.Dispose()
        {
            // Do NOT dispose the underlying IMemoryCache, as there might be other cache wrappers using it
        }

        private object CreateCacheKey(object key) => (PartitionKey, key);
    }
}
