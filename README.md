# What does it do?

Library responsible for providing caching strategies.

# How to use it?

To use the library, just perform the following configuration in **startup.cs**:

```csharp
public void Configure(IServiceCollection services)
{
    ..
    services.AddCacheServices();
}
```

# Facilities

### MemoryCache

To use the in-memory cache strategy, just perform the following coding:

```csharp
public class ProductService
{
    private readonly ICache _cacheService;
    private readonly IProductRepository _productRepository;

    public ProductService(ICache cacheService, IProductRepository productRepository)
    {
        _cacheService = cacheService;
        _productRepository = productRepository;
    }

    public async Task GetById(int productId)
    {
        ..

        var product = await _cacheService
            .GetFromMemoryAsync("product-by-id-key",() => _productRepository.GetById(productId));
    }
}
```

Where the first argument is the cache key and the second is the method that should be executed if the data does not exist in the cache.
**Note:** By default, the cache time is 1 hour, if it is necessary to work the cache with periods other than the default, just pass this time as the second parameter.

```csharp
public class ProductService
{
    private readonly ICache _cacheService;
    private readonly IProductRepository _productRepository;

    public ProductService(ICache cacheService, IProductRepository productRepository)
    {
        _cacheService = cacheService;
        _productRepository = productRepository;
    }

    public async Task GetById(int productId)
    {
        ..

        var product = await _cacheService
            .GetFromMemoryAsync("product-by-id-key", Timespan.FromMinutes(15), () => _productRepository.GetById(productId));
    }
}
```