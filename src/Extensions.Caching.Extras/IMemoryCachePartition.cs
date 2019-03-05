using Microsoft.Extensions.Caching.Memory;

namespace Extensions.Caching.Extras
{
    public interface IMemoryCachePartition : IMemoryCache
    {
        object PartitionKey { get; }
    }

    public interface IMemoryCachePartition<TPartition> : IMemoryCachePartition
    {
    }
}
