using System;
using System.Threading;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace Extensions.Caching.Extras
{
    public class EvictableMemoryCache : IEvictableMemoryCache
    {
        private static readonly object EvictionSourceCacheKey = $"{typeof(EvictableMemoryCache).FullName}/__EvictionSource__";

        private readonly IMemoryCache _cache;

        public EvictableMemoryCache(IMemoryCache cache)
        {
            _cache = cache;
        }

        public ICacheEntry CreateEntry(object key)
        {
            var entry = _cache.CreateEntry(key);
            AddEvictionToken(entry);
            return entry;
        }

        public void Remove(object key)
        {
            _cache.Remove(key);
        }

        public bool TryGetValue(object key, out object value)
        {
            return _cache.TryGetValue(key, out value);
        }

        public void EvictAll()
        {
            var cts =  GetEvictionSource(false);
            _cache.Remove(EvictionSourceCacheKey);
            cts?.Cancel();
        }

        private void AddEvictionToken(ICacheEntry entry)
        {
            var cts = GetEvictionSource(true);
            entry.AddExpirationToken(new CancellationChangeToken(cts.Token));
        }

        void IDisposable.Dispose()
        {
            // Do NOT dispose the underlying IMemoryCache, as there might be other cache wrappers using it
        }

        private CancellationTokenSource GetEvictionSource(bool createIfNotExists)
        {
            if (_cache.TryGetValue(EvictionSourceCacheKey, out var value))
            {
                return (CancellationTokenSource)value;
            }

            if (createIfNotExists)
            {
                var cts = new CancellationTokenSource();
                _cache.CreateEntry(EvictionSourceCacheKey)
                    .SetPriority(CacheItemPriority.NeverRemove)
                    .SetValue(cts)
                    .Dispose();
                return cts;
            }

            return null;
        }
    }
}
