using System;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Xunit;

namespace Extensions.Caching.Extras.Tests
{
    public class MemoryCachePartitionTests
    {
        private readonly IMemoryCache _underlyingCache;

        public MemoryCachePartitionTests()
        {
            var cacheOptions = Options.Create(new MemoryCacheOptions());
            _underlyingCache = new MemoryCache(cacheOptions);
        }

        [Fact]
        public void Can_set_and_get_value()
        {
            var cache = CreatePartition("test");
            var key = 42;
            var originalValue = new object();
            cache.Set(key, originalValue);
            var actualValue = cache.Get(key);
            actualValue.Should().BeSameAs(originalValue);
        }

        [Fact]
        public void Can_set_and_remove_value()
        {
            var cache = CreatePartition("test");
            var key = 42;
            cache.Set(key, new object());
            cache.Remove(key);
            cache.TryGetValue(key, out _).Should().BeFalse();
        }

        [Fact]
        public void Entry_is_not_in_underlying_cache()
        {
            var cache = CreatePartition("test");
            var key = 42;
            cache.Set(key, new object());
            _underlyingCache.TryGetValue(key, out _).Should().BeFalse();
        }

        [Fact]
        public void Entry_is_not_shared_with_other_partition_with_different_key()
        {
            var cache1 = CreatePartition("test1");
            var cache2 = CreatePartition("test2");
            var key = 42;
            cache1.Set(key, new object());
            cache2.TryGetValue(key, out _).Should().BeFalse();
        }

        [Fact]
        public void Entry_is_shared_with_other_partition_with_same_key()
        {
            var cache1 = CreatePartition("test");
            var cache2 = CreatePartition("test");
            var key = 42;
            var originalValue = new object();
            cache1.Set(key, originalValue);
            var actualValue = cache2.Get(key);
            actualValue.Should().Be(originalValue);
        }

        private IMemoryCachePartition CreatePartition(object partitionKey) => _underlyingCache.GetPartition(partitionKey);
    }
}
