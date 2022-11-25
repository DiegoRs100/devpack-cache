using Devpack.Cache.Services.MemoryCache;

namespace Devpack.Cache
{
    public class CacheService : ICache
    {
        private readonly IMemoryCacheService _memoryCacheService;

        public CacheService(IMemoryCacheService memoryCacheService)
        {
            _memoryCacheService = memoryCacheService;
        }

        public TResult GetFromMemory<TResult>(string cacheKey, Func<TResult> func)
        {
            return GetFromService(_memoryCacheService, cacheKey, func);
        }

        public TResult GetFromMemory<TResult>(string cacheKey, TimeSpan lifetime, Func<TResult> func)
        {
            return GetFromService(_memoryCacheService, cacheKey, func, lifetime);
        }

        public Task<TResult> GetFromMemoryAsync<TResult>(string cacheKey, Func<Task<TResult>> func)
        {
            return GetFromServiceAsync(_memoryCacheService, cacheKey, func);
        }

        public Task<TResult> GetFromMemoryAsync<TResult>(string cacheKey, TimeSpan lifetime, Func<Task<TResult>> func)
        {
            return GetFromServiceAsync(_memoryCacheService, cacheKey, func, lifetime);
        }

        private static TResult GetFromService<TResult>(ICacheService cacheService, string cacheKey, Func<TResult> func, 
            TimeSpan? lifetime = null)
        {
            var hasCache = cacheService.Get<TResult>(cacheKey, out var cachedData);

            if (hasCache)
                return cachedData;

            var result = func.Invoke();

            if (lifetime.HasValue)
                cacheService.Save(cacheKey, result, lifetime.Value);
            else
                cacheService.Save(cacheKey, result);

            return result;
        }

        private static async Task<TResult> GetFromServiceAsync<TResult>(ICacheService cacheService, string cacheKey, Func<Task<TResult>> func,
            TimeSpan? lifetime = null)
        {
            var hasCache = cacheService.Get<TResult>(cacheKey, out var cachedData);

            if (hasCache)
                return cachedData;

            var result = await func.Invoke();

            if (lifetime.HasValue)
                cacheService.Save(cacheKey, result, lifetime.Value);
            else
                cacheService.Save(cacheKey, result);

            return result;
        }
    }
}