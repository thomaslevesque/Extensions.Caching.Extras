using System;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Extensions.Caching.Extras.Tests
{
    public class MemoryCacheExtrasServiceCollectionExtensionsTests
    {
        private readonly IServiceProvider _serviceProvider;
        
        public MemoryCacheExtrasServiceCollectionExtensionsTests()
        {
            _serviceProvider = new ServiceCollection()
                .AddMemoryCache()
                .AddCachePartitioning()
                .BuildServiceProvider();
        }

        [Fact]
        public void Can_resolve_typed_cache_partition()
        {
            _serviceProvider.GetRequiredService<IMemoryCachePartition<Foo>>();
        }

        [Fact]
        public void Cache_partition_with_different_types_are_independent()
        {
            var fooPartition = _serviceProvider.GetRequiredService<IMemoryCachePartition<Foo>>();
            var barPartition = _serviceProvider.GetRequiredService<IMemoryCachePartition<Bar>>();

            fooPartition.Set("blah", 42);
            barPartition.TryGetValue("blah", out _).Should().BeFalse();
        }

        [Fact]
        public void Cache_partition_with_same_type_resolve_the_same_instance()
        {
            var partition1 = _serviceProvider.GetRequiredService<IMemoryCachePartition<Foo>>();
            var partition2 = _serviceProvider.GetRequiredService<IMemoryCachePartition<Foo>>();

            partition2.Should().BeSameAs(partition1);
        }

        class Foo
        {
        }

        class Bar
        {
        }
    }
}
