using System;
using Extensions.Caching.Extras;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MemoryCacheExtrasServiceCollectionExtensions
    {
        public static IServiceCollection AddCachePartitioning(this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            services.TryAddSingleton<IMemoryCachePartitionFactory, MemoryCachePartitionFactory>();
            services.TryAddSingleton(typeof(IMemoryCachePartition<>), typeof(MemoryCachePartition<>));
            return services;
        }
    }
}