using Devpack.Cache.Services.MemoryCache;
using Microsoft.Extensions.DependencyInjection;

namespace Devpack.Cache
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddCacheServices(this IServiceCollection services)
        {
            services.AddSingleton<IMemoryCacheService, MemoryCacheService>();
            services.AddSingleton<ICache, CacheService>();

            services.AddMemoryCache();

            return services;
        }
    }
}