using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Xunit;

namespace Extensions.Caching.Extras.Tests
{
    public class EvictableMemoryCachePartitionTests
    {
        private readonly IMemoryCache _underlyingCache;

        public EvictableMemoryCachePartitionTests()
        {
            var cacheOptions = Options.Create(new MemoryCacheOptions());
            _underlyingCache = new MemoryCache(cacheOptions);
        }

        [Fact]
        public void Can_set_and_get_value()
        {
            var cache = CreateEvictablePartition("test");
            var key = 42;
            var originalValue = new object();
            cache.Set(key, originalValue);
            var actualValue = cache.Get(key);
            actualValue.Should().BeSameAs(originalValue);
        }

        [Fact]
        public void Can_set_and_remove_value()
        {
            var cache = CreateEvictablePartition("test");
            var key = 42;
            cache.Set(key, new object());
            cache.Remove(key);
            cache.TryGetValue(key, out _).Should().BeFalse();
        }

        [Fact]
        public void EvictAll_doesnt_affect_other_partition_with_different_key()
        {
            var cache1 = CreateEvictablePartition("test1");
            var cache2 = CreateEvictablePartition("test2");
            cache1.Set(42, new object());
            cache1.Set(123, "hello");
            cache1.Set("blah", "test");
            cache2.Set(42, new object());
            cache2.Set(123, "hello");
            cache2.Set("blah", "test");

            cache1.EvictAll();

            cache2.TryGetValue(42, out _).Should().BeTrue();
            cache2.TryGetValue(123, out _).Should().BeTrue();
            cache2.TryGetValue("blah", out _).Should().BeTrue();
        }

        [Fact]
        public void EvictAll_removes_all_entries_in_other_partition_with_same_key()
        {
            var cache1 = CreateEvictablePartition("test");
            var cache2 = CreateEvictablePartition("test");
            cache2.Set(42, new object());
            cache2.Set(123, "hello");
            cache2.Set("blah", "test");

            cache1.EvictAll();

            cache2.TryGetValue(42, out _).Should().BeFalse();
            cache2.TryGetValue(123, out _).Should().BeFalse();
            cache2.TryGetValue("blah", out _).Should().BeFalse();
        }

        private IEvictableMemoryCache CreateEvictablePartition(object partitionKey) => _underlyingCache.Partition(partitionKey).WithEviction();
    }
}
