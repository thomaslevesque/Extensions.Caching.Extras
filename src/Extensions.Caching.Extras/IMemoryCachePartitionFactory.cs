namespace Extensions.Caching.Extras
{
    public interface IMemoryCachePartitionFactory
    {
        IMemoryCachePartition Create(object partitionKey);
    }
}
