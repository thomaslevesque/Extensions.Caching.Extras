using Microsoft.Extensions.Caching.Memory;

namespace Extensions.Caching.Extras
{
    public interface IEvictableMemoryCache : IMemoryCache
    {
        void EvictAll();
    }
}
