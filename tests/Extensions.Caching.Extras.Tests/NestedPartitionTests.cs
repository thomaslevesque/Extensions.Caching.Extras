using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Xunit;

namespace Extensions.Caching.Extras.Tests
{
    public class NestedPartitionTests
    {
        private readonly IMemoryCache _underlyingCache;

        public NestedPartitionTests()
        {
            var cacheOptions = Options.Create(new MemoryCacheOptions());
            _underlyingCache = new MemoryCache(cacheOptions);
        }

        [Fact]
        public void Can_set_and_get_value()
        {
            var cache = CreatePartition("foo", "bar");
            var key = 42;
            var originalValue = new object();
            cache.Set(key, originalValue);
            var actualValue = cache.Get(key);
            actualValue.Should().BeSameAs(originalValue);
        }

        [Fact]
        public void Can_set_and_remove_value()
        {
            var cache = CreatePartition("foo", "bar");
            var key = 42;
            cache.Set(key, new object());
            cache.Remove(key);
            cache.TryGetValue(key, out _).Should().BeFalse();
        }

        [Fact]
        public void Entry_is_not_in_underlying_cache()
        {
            var cache = CreatePartition("foo", "bar");
            var key = 42;
            cache.Set(key, new object());
            _underlyingCache.TryGetValue(key, out _).Should().BeFalse();
        }

        [Theory]
        [InlineData("foo", "baz")]
        [InlineData("baz", "bar")]
        public void Entry_is_not_shared_with_other_partition_with_different_key(string key1, string key2)
        {
            var cache1 = CreatePartition("foo", "bar");
            var cache2 = CreatePartition(key1, key2);
            var key = 42;
            cache1.Set(key, new object());
            cache2.TryGetValue(key, out _).Should().BeFalse();
        }

        [Fact]
        public void Entry_is_shared_with_other_partition_with_same_key()
        {
            var cache1 = CreatePartition("foo", "bar");
            var cache2 = CreatePartition("foo", "bar");
            var key = 42;
            var originalValue = new object();
            cache1.Set(key, originalValue);
            var actualValue = cache2.Get(key);
            actualValue.Should().Be(originalValue);
        }

        private IMemoryCache CreatePartition(object key1, object key2) => _underlyingCache.Partition(key1).Partition(key2);
    }
}