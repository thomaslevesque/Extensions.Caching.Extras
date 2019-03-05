using System;
using System.Linq;
using Extensions.Caching.Extras;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MemoryCacheServiceCollectionExtensions
    {
        public static IServiceCollection AddCachePartitioning(this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (!services.Any(t => t.ServiceType == typeof(IMemoryCache)))
                throw new InvalidOperationException("A memory cache implementation must be registered. Try registering the default implementation with services.AddMemoryCache()");

            services.TryAddSingleton(typeof(IMemoryCachePartition<>), typeof(MemoryCachePartition<>));
            services.TryAddSingleton(typeof(IEvictableMemoryCachePartition<>), typeof(EvictableMemoryCachePartition<>));
            return services;
        }
    }
}