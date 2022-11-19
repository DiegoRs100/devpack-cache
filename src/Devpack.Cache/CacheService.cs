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

        public async Task<TResult> GetFromMemoryAsync<TResult>(string cacheKey, Func<Task<TResult>> func)
        {
            return await GetFromServiceAsync(_memoryCacheService, cacheKey, func);
        }

        private static TResult GetFromService<TResult>(ICacheService cacheService, string cacheKey, Func<TResult> func)
        {
            var hasCache = cacheService.Get<TResult>(cacheKey, out var cachedData);

            if (hasCache)
                return cachedData;

            var result = func.Invoke();
            cacheService.Save(cacheKey, result);

            return result;
        }

        private static async Task<TResult> GetFromServiceAsync<TResult>(ICacheService cacheService, string cacheKey, Func<Task<TResult>> func)
        {
            var hasCache = cacheService.Get<TResult>(cacheKey, out var cachedData);

            if (hasCache)
                return cachedData;

            var result = await func.Invoke();
            cacheService.Save(cacheKey, result!);

            return result;
        }
    }
}