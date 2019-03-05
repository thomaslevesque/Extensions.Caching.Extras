using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Xunit;

namespace Extensions.Caching.Extras.Tests
{
    public class EvictableMemoryCacheTests
    {
        private readonly IMemoryCache _underlyingCache;

        public EvictableMemoryCacheTests()
        {
            var cacheOptions = Options.Create(new MemoryCacheOptions());
            _underlyingCache = new MemoryCache(cacheOptions);
        }

        [Fact]
        public void Can_set_and_get_value()
        {
            var cache = CreateEvictableCache();
            var key = 42;
            var originalValue = new object();
            cache.Set(key, originalValue);
            var actualValue = cache.Get(key);
            actualValue.Should().BeSameAs(originalValue);
        }

        [Fact]
        public void Can_set_and_remove_value()
        {
            var cache = CreateEvictableCache();
            var key = 42;
            cache.Set(key, new object());
            cache.Remove(key);
            cache.TryGetValue(key, out _).Should().BeFalse();
        }

        [Fact]
        public void EvictAll_removes_all_entries_in_cache()
        {
            var cache = CreateEvictableCache();
            cache.Set(42, new object());
            cache.Set(123, "hello");
            cache.Set("blah", "test");

            cache.EvictAll();

            cache.TryGetValue(42, out _).Should().BeFalse();
            cache.TryGetValue(123, out _).Should().BeFalse();
            cache.TryGetValue("blah", out _).Should().BeFalse();
        }

        [Fact]
        public void EvictAll_removes_all_entries_in_underlying_cache()
        {
            var cache = CreateEvictableCache();
            cache.Set(42, new object());
            cache.Set(123, "hello");
            cache.Set("blah", "test");

            cache.EvictAll();

            _underlyingCache.TryGetValue(42, out _).Should().BeFalse();
            _underlyingCache.TryGetValue(123, out _).Should().BeFalse();
            _underlyingCache.TryGetValue("blah", out _).Should().BeFalse();
        }

        [Fact]
        public void EvictAll_works_if_there_are_no_entries()
        {
            var cache = CreateEvictableCache();
            cache.EvictAll();
        }

        private IEvictableMemoryCache CreateEvictableCache() => _underlyingCache.Evictable();
    }
}
