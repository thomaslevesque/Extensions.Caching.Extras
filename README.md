# Extensions.Caching.Extras

Adds features to Microsoft.Extensions.Caching such as partitioning and eviction

## Partitioning

`IMemoryCache` is registered as a singleton, which means it's shared by the whole
application. So you need to be careful to avoid key collisions. Consider this scenario:
- a `ProductManager` class uses product ids as cache keys for products
- a `CustomerManager` class uses customer ids as cache keys for customers
If a product and a customer have the same id (which can easily happen if you use
integer ids) will have the same cache key. So you could end up retrieving a product
when you try to get a customer from the cache!

A simple solution to this problem is to use a prefix for the cache key, for instance
something like `$"Products/{id}"`. But it's tedious to add the prefix every time you
need to access the cache...

The `Partition` method gives you a memory cache that is a "subset" of the global
cache. Just pass a "partition key" to the `Partition` method:

```csharp
public class ProductManager
{
    private readonly IMemoryCache _cache;

    public ProductManager(IMemoryCache cache)
    {
        _cache = cache.Partition("Products");
    }
    
    ...
}
```
When you need to get a product from the cache, you no longer need to add a prefix:
just pass the product id, and you won't get collisions with other parts of the code
using the same global cache (unless they use the same partition key, of course).

You can also use nested partitions. For instance, in a multitenant application, you
might want to partition the cache by tenant, to ensure you never access a tenant's
data from another tenant:

```csharp
    public ProductManager(IMemoryCache cache, ITenantContext tenantContext)
    {
        _cache = cache.Partition(tenantContext.TenantKey)
                      .Partition("Products");
    }
```

The partition key can be anything, it just needs to implement `Equals` and
`GetHashCode` correctly. The previous examples used strings, but sometimes it makes
sense to use types. There's a generic overload of `Partition` for partitioning by
type:

```csharp
    public ProductManager(IMemoryCache cache)
    {
        _cache = cache.Partition<ProductManager>();
    }
```
This gives you a cache partition whose key is `typeof(ProductManager)`.

You can also inject a cache partition directly. Call
`services.AddCachePartitioning()` in your service registrations, and you'll be able
to do this:

```csharp
    public ProductManager(IMemoryCachePartition<ProductManager> cache)
    {
        _cache = cache;
    }
```

This provides the same behavior as the previous example (a cache partition whose key
is `typeof(ProductManager)`).

## Eviction

Out of the box, there's no way to clear a memory cache. You can only remove items one
by one, assuming you know their keys. The `WithEviction` method provides this
functionality:

```csharp
public class MyClass
{
    private readonly IEvictableMemoryCache _cache;
    
    public MyClass(IMemoryCache cache)
    {
        _cache = cache.WithEviction();
    }
    
    public void ClearCache()
    {
        _cache.EvictAll();
    }
    
    ...
}
```

*Note: only entries added via an evictable cache will actually be removed. Any cache
entry added directly via the normal memory cache will be unaffected.*

Clearing the whole cache is rarely useful, but it can be useful to clear a subset of
it. To do this, you can combine the partitioning and eviction features:

```csharp
_cache = cache.Partition("Products").WithEviction();
...

// Only clears the "Products" partition
_cache.EvictAll();
```