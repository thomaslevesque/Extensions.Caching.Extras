using System;
using Microsoft.Extensions.Caching.Memory;

namespace Extensions.Caching.Extras
{
    public class MemoryCachePartition : IMemoryCachePartition
    {
        private readonly IMemoryCache _cache;

        public MemoryCachePartition(IMemoryCache cache, object partitionKey)
        {
            _cache = cache;
            PartitionKey = partitionKey;
        }

        public object PartitionKey { get; }

        public ICacheEntry CreateEntry(object key)
        {
            return _cache.CreateEntry(CreateCacheKey(key));
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

        private CacheKey CreateCacheKey(object key) => new CacheKey(PartitionKey, key);

        private struct CacheKey : IEquatable<CacheKey>
        {
            private readonly object _partitionKey;
            private readonly object _key;

            public CacheKey(object partitionKey, object key)
            {
                _partitionKey = partitionKey ?? throw new ArgumentNullException(nameof(partitionKey));
                _key = key ?? throw new ArgumentNullException(nameof(key));
            }

            public bool Equals(CacheKey other)
            {
                return _partitionKey.Equals(other._partitionKey) && _key.Equals(other._key);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                return obj is CacheKey cacheKey && Equals(cacheKey);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return (_partitionKey.GetHashCode() * 397) ^ _key.GetHashCode();
                }
            }

            public override string ToString()
            {
                return $"{_partitionKey}/{_key}";
            }
        }
    }

    public class MemoryCachePartition<TPartition> : MemoryCachePartition
    {
        public MemoryCachePartition(IMemoryCache cache)
            : base(cache, typeof(TPartition))
        {
        }
    }
}
