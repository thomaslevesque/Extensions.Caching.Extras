using Microsoft.Extensions.Caching.Memory;

namespace Extensions.Caching.Extras
{
    public interface IMemoryCachePartition<TPartition> : IMemoryCache
    {
    }
}
