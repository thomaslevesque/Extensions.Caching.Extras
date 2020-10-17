using Microsoft.Extensions.Caching.Memory;

namespace Extensions.Caching.Extras
{
    /// <summary>
    /// Represents a wrapper around <see cref="IMemoryCache"/> that supports cache eviction.
    /// </summary>
    public interface IEvictableMemoryCache : IMemoryCache
    {
        /// <summary>
        /// Removes all entries from the cache.
        /// </summary>
        void EvictAll();
    }
}
